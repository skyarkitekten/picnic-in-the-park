# Journey Map — Picnic In The Park (End-User, In-Fiction)

This map traces the in-fiction end user (Linnea, see [personas.md](personas.md)) through the
headline scenario from the [PRD §6](prd.md#6-headline-scenario--the-killer-demo). At each
step we call out which agents are active and which framework pattern is on display — so the
journey doubles as a tour of the demo.

## Stages

```mermaid
flowchart LR
    T[Trigger:<br/>"Let's picnic Saturday"] --> W[Plan via<br/>Wizard]
    W --> A[Agents<br/>Collaborate]
    A --> R[Review &<br/>Confirm]
    R --> E[Event:<br/>Weather Delta]
    E --> RP[Selective<br/>Re-Plan]
    RP --> RR[Diff Review &<br/>Re-Confirm]
    RR --> D[Day-Of<br/>Plan Executed]
```

## Step-by-step

### Stage 1 — Trigger

| Aspect        | Detail                                                                     |
| ------------- | -------------------------------------------------------------------------- |
| User says     | "Plan a picnic for four on Saturday afternoon, near the lake, around $80." |
| User thinks   | "I hope I don't have to fill in twelve fields."                            |
| User feels    | Hopeful, a little impatient.                                               |
| System action | Coordinator parses intent; identifies missing structured inputs.           |
| Agents active | Coordinator.                                                               |
| Patterns      | Intent capture; transition from chat to structured wizard.                 |

### Stage 2 — Plan via wizard (HITL intake)

| Aspect        | Detail                                                                                      |
| ------------- | ------------------------------------------------------------------------------------------- |
| User does     | Confirms date, party size (4), budget ($80), radius (5 km), dietary notes (one vegetarian). |
| User thinks   | "Good — it asked the right things and nothing extra."                                       |
| User feels    | In control.                                                                                 |
| System action | Wizard returns a structured `PicnicPlanRequest` to the Coordinator.                         |
| Agents active | Coordinator.                                                                                |
| **Patterns**  | **HITL via structured wizard** (FR-U2).                                                     |

### Stage 3 — Agents collaborate

| Aspect        | Detail                                                                                      |
| ------------- | ------------------------------------------------------------------------------------------- |
| User does     | Watches cards stream in.                                                                    |
| User thinks   | "Oh — it's actually doing things. Weather first, then parks, then menu…"                    |
| User feels    | Curious; reassured by visibility.                                                           |
| System action | Coordinator invokes specialists in dependency order; each agent renders its own AG-UI card. |
| Agents active | Weather → Activity → Menu → Grocery → Reservation → Budget.                                 |
| **Patterns**  | **Orchestrator → specialists**, **AG-UI per-agent rendering**, **A2A** (Menu ↔ Grocery).    |

### Stage 4 — Review & confirm

| Aspect        | Detail                                                                                        |
| ------------- | --------------------------------------------------------------------------------------------- |
| User does     | Reviews the assembled plan; clicks "Confirm plan".                                            |
| User thinks   | "$78. Pavilion at Lakeside Park. Looks fine."                                                 |
| User feels    | Committed.                                                                                    |
| System action | Coordinator marks plan `Confirmed`. Reservation Agent holds the pavilion (does **not** book). |
| Agents active | Coordinator, Reservation, Budget.                                                             |
| **Patterns**  | **HITL confirmation gate** before any irreversible action (FR-C6, FR-R2).                     |

### Stage 5 — Event arrives (the trigger that makes the demo)

| Aspect        | Detail                                                                                                                                      |
| ------------- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| User does     | Nothing — they're at work on Friday.                                                                                                        |
| User thinks   | _(not present)_                                                                                                                             |
| System action | Simulated weather service flips Saturday: sunny 24°C → thunderstorms 60%, 17°C. Weather Agent re-classifies → emits `WeatherChanged` event. |
| Agents active | Weather (push), Event Bus.                                                                                                                  |
| **Patterns**  | **Event-driven trigger** for re-planning (FR-W3).                                                                                           |

### Stage 6 — Selective re-plan (the headline)

| Aspect        | Detail                                                                                                                                                                                                 |
| ------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| System action | Coordinator wakes, diffs new conditions vs. current plan, identifies impacted agents (Activity, Menu, Reservation). Re-runs only those. Grocery updates via A2A from Menu's change. Budget recomputes. |
| Agents active | Coordinator, Activity, Menu ↔ Grocery (A2A), Reservation, Budget.                                                                                                                                      |
| **Patterns**  | **Event-driven selective re-planning**, **A2A negotiation**, **diff production** (FR-C4, FR-C5).                                                                                                       |

### Stage 7 — Diff review & re-confirm

| Aspect        | Detail                                                                                    |
| ------------- | ----------------------------------------------------------------------------------------- |
| User does     | Opens a notification: "Your Saturday plan changed". Reviews diff card.                    |
| User thinks   | "Pavilion instead of lakeside. Hot soup instead of ice cream. +$6. Sure."                 |
| User feels    | Slight relief — didn't have to re-plan from scratch.                                      |
| System action | Renders **only the deltas**. Waits for confirmation before updating the held reservation. |
| Agents active | Coordinator, Reservation.                                                                 |
| **Patterns**  | **Diff-only surface**, **HITL re-confirmation gate**.                                     |

### Stage 8 — Day-of

| Aspect        | Detail                                                                  |
| ------------- | ----------------------------------------------------------------------- |
| User does     | Opens the app Saturday morning; sees a "Today's plan" view.             |
| User thinks   | "Right. Grocery list, pavilion address, weather check. Got it."         |
| User feels    | Prepared.                                                               |
| System action | Final, confirmed plan view. (Out of scope: actual booking, navigation.) |
| Agents active | Read-only render.                                                       |

## Moments that matter

| Moment                          | Why it matters                                                      | Risk if mishandled                                             |
| ------------------------------- | ------------------------------------------------------------------- | -------------------------------------------------------------- |
| First card streams in (Stage 3) | Establishes that the system is _doing things_, not pretending.      | Latency or silence here reads as a broken demo.                |
| Confirm-plan click (Stage 4)    | Sets the user's mental model: "nothing irreversible without my OK". | Implicit booking would break trust permanently.                |
| Re-plan notification (Stage 7)  | The headline moment of the entire demo.                             | If full plan re-renders instead of diff, demo loses its point. |

## Mapping to the PRD

Stages 1–4 satisfy goals **G1, G4, G5**.
Stages 5–7 satisfy goals **G2, G3** — the killer demo.
Stage 8 closes the loop and exercises non-functional **NFR-2** (reproducibility) for
workshop facilitation.
