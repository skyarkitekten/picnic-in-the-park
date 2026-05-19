# Service Blueprint — Picnic In The Park

Swim lanes for the headline scenario, including the re-plan loop. This blueprint should be
read alongside [journey-map.md](journey-map.md) — the journey is what the user experiences;
the blueprint is what the system does to make that experience happen.

## Lanes

1. **User** — what the human says, does, and sees.
2. **Frontend** — React/Vite chat UI, wizard component, AG-UI host, diff renderer.
3. **Coordinator** — hosted Agent Framework agent; orchestrates and re-plans.
4. **Specialist agents** — Weather, Activity, Menu, Grocery, Reservation, Budget.
5. **Backend & services** — Planner API, Parks Service, Weather Service.
6. **Event bus** — Azure-native (pending [PRD OQ-1](prd.md#11-open-questions--assumptions)).
7. **External / simulated** — forecast feed, reservation mock, grocery mock.

## Plan-creation flow

```mermaid
sequenceDiagram
    autonumber
    actor U as User
    participant FE as Frontend (Wizard + Chat + AG-UI)
    participant CO as Coordinator
    participant W as Weather Agent
    participant A as Activity Agent
    participant M as Menu Agent
    participant G as Grocery Agent
    participant R as Reservation Agent
    participant B as Budget Agent
    participant SVC as Backend Services<br/>(Parks, Weather)

    U->>FE: "Picnic Saturday, 4 people, $80"
    FE->>CO: PicnicPlanRequest (structured)
    CO->>W: forecast(date, location)
    W->>SVC: GET /weather
    SVC-->>W: forecast payload
    W-->>CO: WeatherClassification
    CO->>A: rank parks (radius, classification)
    A->>SVC: GET /parks
    SVC-->>A: parks
    A-->>CO: park shortlist
    CO->>M: propose menu (party, budget, dietary, weather)
    M-->>G: ingredient list (A2A)
    G-->>M: priced list + substitutions (A2A)
    M-->>CO: final menu
    G-->>CO: shopping list
    CO->>R: hold pavilion (park, party)
    R-->>CO: hold token
    CO->>B: aggregate costs
    B-->>CO: budget summary
    CO-->>FE: PicnicPlan + AG-UI cards
    FE-->>U: cards stream into chat
    U->>FE: Confirm plan
    FE->>CO: Confirm
    CO->>R: keep hold (still not booked)
    CO-->>FE: plan = Confirmed
```

## Re-plan flow (the headline)

```mermaid
sequenceDiagram
    autonumber
    participant SIM as Simulated Forecast
    participant W as Weather Agent
    participant BUS as Event Bus
    participant CO as Coordinator
    participant A as Activity Agent
    participant M as Menu Agent
    participant G as Grocery Agent
    participant R as Reservation Agent
    participant B as Budget Agent
    participant FE as Frontend
    actor U as User

    SIM->>W: forecast changed (sunny -> storms)
    W->>W: re-classify
    W->>BUS: WeatherChanged event
    BUS->>CO: deliver event
    CO->>CO: diff plan vs new classification
    Note over CO: Impacted agents:<br/>Activity, Menu, Reservation
    par Re-run impacted only
        CO->>A: re-rank with new classification
        A-->>CO: updated shortlist
    and
        CO->>M: re-plan menu for cold/wet
        M-->>G: updated ingredient list (A2A)
        G-->>M: updated priced list (A2A)
        M-->>CO: updated menu
        G-->>CO: updated shopping list
    and
        CO->>R: refresh hold (pavilion-capable park)
        R-->>CO: new hold token
    end
    CO->>B: recompute budget
    B-->>CO: new budget summary
    CO->>CO: build PlanDiff (only deltas)
    CO-->>FE: PlanDiff
    FE-->>U: notification + diff card
    U->>FE: Confirm changes
    FE->>CO: Confirm diff
    CO->>R: keep new hold
```

## Front-stage vs. back-stage

| Front-stage (visible to user)  | Back-stage (hidden)                                  |
| ------------------------------ | ---------------------------------------------------- |
| Chat messages, wizard prompts. | Coordinator intent parsing.                          |
| AG-UI cards per agent.         | Specialist agent invocations and tool calls.         |
| Diff notification on re-plan.  | Event-bus delivery, plan-diff computation.           |
| Confirmation buttons.          | Reservation hold lifecycle (held → kept → released). |
| Budget total.                  | Cost-event aggregation across agents.                |

## Failure & degradation modes (non-exhaustive)

| Failure                                  | Behaviour                                                                                                                           |
| ---------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| Weather Service unavailable.             | Weather Agent returns `Unknown` classification; Activity ranks conservatively; UI labels degraded confidence.                       |
| Parks Service slow.                      | Activity Agent times out gracefully; Coordinator surfaces partial plan with retry option.                                           |
| Grocery mock returns missing items.      | Grocery → Menu A2A initiates substitution loop; bounded to N rounds (anti-archetype, see [system-dynamics.md](system-dynamics.md)). |
| Reservation hold expires before confirm. | Reservation emits event; Coordinator surfaces "hold expired — re-acquire?" diff.                                                    |
| Event-bus delivery delayed.              | Re-plan is late but eventually consistent; UI shows "checking for updates".                                                         |

## Observability touchpoints

- Every arrow in the diagrams above corresponds to a trace span (Aspire + Foundry).
- `PlanDiff` artefacts are persisted per re-plan for replay and demo capture.
- Cost events flow through the Budget Agent and are independently inspectable.

## Mapping to the PRD

- Plan-creation flow → functional requirements FR-C1..C3, FR-W1..W2, FR-A1..A3, FR-M1..M2,
  FR-G1, FR-R1..R2, FR-B1..B3, FR-U1..U3.
- Re-plan flow → FR-C3..C5, FR-W3, FR-A4, FR-M3, FR-G2, FR-U4.
- Failure modes → NFR-6 (resilience), NFR-3 (observability).
