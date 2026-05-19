# Picnic In The Park — Product Requirements Document (PRD)

**Status:** Draft · **Owner:** Skyarkitekten · **Last updated:** 2026-05-19

## 1. Purpose

Picnic In The Park is a **reference implementation** of a multi-agent system built on the
Microsoft Agent Framework, hosted in Microsoft Foundry, and deployed on Azure-native services
(see [ADR-002](../decisions/002-azure-native.md), [ADR-003](../decisions/003-microsoft-foundry.md),
[ADR-004](../decisions/004-microsoft-agent-framework.md)).

The product has **two intertwined purposes**:

1. **Pedagogical (primary).** Show developers, architects, and demo audiences how to compose
   an orchestrator + specialist agents, do agent-to-agent (A2A) negotiation, render per-agent
   UI through AG-UI, gate decisions with structured Human-In-The-Loop (HITL), and — the
   headline pattern — **re-plan in response to real-world events**.
2. **In-fiction (secondary).** Be a plausible end-user product: an agentic assistant that
   turns "let's picnic Saturday" into an end-to-end plan that adapts as conditions change.

The in-fiction story exists to keep the demo grounded. The pedagogical story is what we
optimise for when trade-offs arise.

## 2. Vision

> A weekend picnic plan that updates itself.
>
> You ask once. The agents collaborate. When the forecast shifts, the plan shifts — and only
> what changed shows up in your wizard.

## 3. Audiences

### Primary — Developers & architects

- .NET / Python developers exploring the Microsoft Agent Framework.
- Solutions architects evaluating Foundry for multi-agent workloads.
- Conference / workshop attendees who need a runnable end-to-end example.

### Secondary — In-fiction end users

- A parent organising a family picnic.
- A friend-group coordinator handling logistics for 6–10 people.
- A couple planning a low-effort outdoor afternoon.

Detailed personas live in [personas.md](personas.md).

## 4. Goals

- **G1 — Showcase orchestration.** Coordinator sequences ≥ 4 specialist agents with clear,
  observable handoffs.
- **G2 — Demonstrate event-driven re-planning.** A simulated weather delta triggers a
  selective re-plan that surfaces _only what changed_ to the user.
- **G3 — Demonstrate A2A.** At least two pairs of agents negotiate directly (Menu ↔ Grocery
  on quantities; Weather ↔ Activity on conditions).
- **G4 — Demonstrate AG-UI.** Each agent renders its own card (weather, menu, map, budget)
  composed into the chat surface.
- **G5 — Demonstrate structured HITL.** A wizard collects plan inputs; explicit confirmation
  gates protect irreversible actions (e.g., simulated reservation booking).
- **G6 — Be runnable.** `aspire run` brings the whole system up locally in under 5 minutes
  on a developer laptop with the documented prerequisites.
- **G7 — Be deployable.** A single `azd up` provisions and deploys the demo to Azure.
- **G8 — Be observable.** Every agent decision, tool call, and re-plan event is visible in
  the Aspire dashboard and Foundry traces.

## 5. Non-goals

- **NG1.** Real reservation, payment, or grocery-ordering integrations. All external bookings
  are simulated.
- **NG2.** Production-grade multi-tenant identity, billing, or quota management.
- **NG3.** A mobile app. The reference UI is a web app (React/Vite) only.
- **NG4.** Geographic coverage beyond a small set of demo locations (e.g., one or two cities
  with seeded park data).
- **NG5.** Personalisation, accounts, or long-term memory beyond the active plan session.
- **NG6.** Dietary, allergen, or medical compliance guarantees in menu suggestions.

## 6. Headline scenario — the killer demo

> **Setup.** A user opens the app on Thursday and asks the assistant to plan a picnic for
> Saturday afternoon for four people, $80 budget, near the lake.
>
> **Plan creation (HITL wizard).** The Coordinator collects structured inputs: date, party
> size, budget, location radius, dietary notes. Each specialist agent returns a card:
> Weather (forecast + risk), Activity (park shortlist + map), Menu (proposed menu), Grocery
> (shopping list + cost), Reservation (pavilion options), Budget (running total). The user
> confirms.
>
> **Event arrives.** Friday morning, the simulated forecast service flips Saturday from
> "sunny 24°C" to "thunderstorms 60% chance, 17°C".
>
> **Selective re-plan.** The event bus notifies the Coordinator. The Coordinator diffs the
> new forecast against the plan, identifies impacted agents (Activity, Menu, Reservation),
> and re-runs _only_ those. Budget recomputes. Grocery is notified of menu changes and
> updates quantities via A2A.
>
> **Diff surface.** The wizard wakes up showing **only the deltas**: "Activity changed:
> pavilion picnic instead of lakeside. Menu changed: hot soup added, ice cream removed.
> Budget: +$6. Confirm?" The user confirms.
>
> **Day-of.** A "plan executed" view shows the final, confirmed plan.

This scenario is mapped step-by-step in [journey-map.md](journey-map.md) and
[service-blueprint.md](service-blueprint.md).

## 7. Functional requirements

### 7.1 Coordinator Agent

- **FR-C1.** Accept a structured plan request (date, party size, budget, location radius,
  preferences) and produce a `PicnicPlan` aggregate.
- **FR-C2.** Sequence specialist agents in a declared dependency order.
- **FR-C3.** Subscribe to plan-relevant events from the event bus (weather delta,
  reservation change).
- **FR-C4.** On event receipt, diff the new world-state against the current plan and
  re-invoke only impacted specialists.
- **FR-C5.** Produce a `PlanDiff` artefact (added / removed / changed items) for the UI.
- **FR-C6.** Block on explicit user confirmation before any irreversible specialist action.

### 7.2 Weather Agent

- **FR-W1.** Fetch forecast for date and location from the Weather Service.
- **FR-W2.** Classify conditions into a small risk taxonomy (e.g., `Ideal`, `Acceptable`,
  `Risk:Rain`, `Risk:Heat`, `Unsafe`).
- **FR-W3.** Push an event to the bus when a previously-fetched forecast changes
  classification.

### 7.3 Activity / Park Agent

- **FR-A1.** Query the Parks Service for parks within the location radius.
- **FR-A2.** Filter and rank parks by conditions (shade, pavilion availability, water
  features) and current weather classification.
- **FR-A3.** Return a shortlist with map coordinates for AG-UI rendering.
- **FR-A4.** Re-rank on weather-classification change (Weather → Activity A2A).

### 7.4 Menu Planner Agent

- **FR-M1.** Propose a menu appropriate for party size, budget share, dietary notes, and
  weather classification (e.g., hot food in cold weather).
- **FR-M2.** Emit a structured ingredient list for the Grocery Agent.
- **FR-M3.** Negotiate quantities and substitutions with Grocery via A2A when items are
  unavailable or over-budget.

### 7.5 Grocery Agent

- **FR-G1.** Take an ingredient list and resolve to a simulated shopping list with
  per-item costs.
- **FR-G2.** Surface substitutions or quantity adjustments back to Menu Planner via A2A.
- **FR-G3.** Render an AG-UI shopping-list card with a per-line cost.

### 7.6 Reservation Agent

- **FR-R1.** Query the (simulated) reservation system for pavilion / table availability.
- **FR-R2.** Hold an option; never confirm without an explicit HITL confirmation gate.
- **FR-R3.** Emit an event when a held option expires or changes.

### 7.7 Budget Agent

- **FR-B1.** Subscribe to cost events from all agents and maintain a running total.
- **FR-B2.** Warn when projected cost exceeds the user budget.
- **FR-B3.** Render an AG-UI budget card with breakdown by category.

### 7.8 User Interface

- **FR-U1.** Chat surface (React + Vite) where the user converses with the Coordinator.
- **FR-U2.** Wizard component for structured plan input and confirmation gates.
- **FR-U3.** AG-UI host that renders per-agent cards inline in the chat.
- **FR-U4.** Diff view that highlights what changed during a re-plan.

### 7.9 Platform

- **FR-P1.** All agents are hosted Microsoft Agent Framework agents, deployed to Foundry
  (per [ADR-004](../decisions/004-microsoft-agent-framework.md)).
- **FR-P2.** Backend, services, and frontend are orchestrated locally via .NET Aspire.
- **FR-P3.** Event bus implementation is an Azure-native service
  (per [ADR-002](../decisions/002-azure-native.md)).

## 8. Non-functional requirements

| ID    | Requirement                                                                                                        |
| ----- | ------------------------------------------------------------------------------------------------------------------ |
| NFR-1 | **Demo-ability.** Cold start to first plan in ≤ 60 seconds after `aspire run`.                                     |
| NFR-2 | **Reproducibility.** Seeded weather / parks / reservation data so the killer demo is deterministic.                |
| NFR-3 | **Observability.** Every agent invocation, tool call, and event is visible in Aspire dashboard and Foundry traces. |
| NFR-4 | **Cost ceiling.** A 30-minute demo run on Azure should cost < $5 in consumption.                                   |
| NFR-5 | **Approachability.** A new developer can read the solution and locate any agent in ≤ 5 minutes.                    |
| NFR-6 | **Resilience.** A specialist agent failure must not crash the Coordinator; degraded plans are acceptable.          |
| NFR-7 | **Security.** No secrets in source; all credentials via Aspire / Azure managed identity.                           |

## 9. Success criteria

The reference implementation is successful when:

- **SC-1.** A developer can clone, `aspire run`, and reach the headline scenario without
  reading more than the README.
- **SC-2.** The re-plan demo runs end-to-end with a triggerable simulated weather event.
- **SC-3.** Each demonstrated pattern (orchestration, A2A, AG-UI, HITL, event-driven
  re-planning) is documented with a "find it here" pointer in the code.
- **SC-4.** ADR trail is complete for every significant architectural decision.
- **SC-5.** Conference / workshop facilitators can run the demo with no presenter notes
  beyond a one-page script.

## 10. Out of scope

See [§5 Non-goals](#5-non-goals). Specifically excluded: real bookings, real payments,
multi-tenant identity, mobile UI, broad geographic coverage, persistent user accounts,
dietary compliance guarantees.

## 11. Open questions & assumptions

| #    | Item                                                                                                                                                                                                                                             | Type       |
| ---- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ---------- |
| OQ-1 | Which Azure-native event bus? (Service Bus topics vs. Event Grid vs. Web PubSub) — pending an ADR.                                                                                                                                               | Open       |
| OQ-2 | Will Activity ranking use a lightweight model call or a deterministic ranker? Affects model cost and demo footprint.                                                                                                                             | Open       |
| OQ-3 | How is the simulated weather event triggered in the UI? (Dev panel? Time-warp button? Random tick?)                                                                                                                                              | Open       |
| OQ-4 | Should Budget be an agent or a deterministic service? Agent is more demonstrative; service is cheaper and faster.                                                                                                                                | Open       |
| OQ-5 | Do we ship a seeded park dataset for one city, or several? Affects demo richness vs. data maintenance.                                                                                                                                           | Open       |
| A-1  | We assume forecast volatility is the most legible event for the re-plan demo (vs. reservation cancellation).                                                                                                                                     | Assumption |
| A-2  | We assume AG-UI rendering can be hosted inside our React/Vite frontend without a separate framework.                                                                                                                                             | Assumption |
| A-3  | The audience has seen single-agent demos and wants the _multi-agent_ and _re-planning_ story specifically. This is now a confirmed product direction: optimise for multi-agent orchestration + event-driven re-planning over single-agent depth. | Confirmed  |

## 12. Related documents

- [notes.md](notes.md) — original architecture sketch (source of truth for patterns)
- [value-proposition.md](value-proposition.md) — value-proposition canvas (dev + end user)
- [personas.md](personas.md) — developer and end-user personas
- [journey-map.md](journey-map.md) — end-to-end journey including re-plan
- [service-blueprint.md](service-blueprint.md) — swim-lane blueprint of the system
- [system-dynamics.md](system-dynamics.md) — systems-thinking analysis (loops, leverage, archetypes)
- [ADR-002](../decisions/002-azure-native.md), [ADR-003](../decisions/003-microsoft-foundry.md),
  [ADR-004](../decisions/004-microsoft-agent-framework.md)
