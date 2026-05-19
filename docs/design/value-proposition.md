# Value Proposition Canvas — Picnic In The Park

Two audiences, two canvases. The developer canvas is the one we optimise; the end-user
canvas keeps the demo honest.

## 1. Developer / architect audience (primary)

### Customer profile

| Jobs to be done                                               | Pains                                                        | Gains                                                          |
| ------------------------------------------------------------- | ------------------------------------------------------------ | -------------------------------------------------------------- |
| Learn the Microsoft Agent Framework end-to-end.               | Concept overload — too many patterns described in isolation. | A single repo where orchestration, A2A, HITL, AG-UI co-exist.  |
| Evaluate Foundry for hosted multi-agent workloads.            | Lack of runnable, deployable, non-trivial examples.          | `aspire run` and `azd up` both work.                           |
| Justify a pattern (re-planning, A2A) to their own team.       | Slideware patterns with no code.                             | A demo they can show, fork, and adapt.                         |
| Wire agents to internal services (not just public APIs).      | Most demos call OpenAI directly with no real backend.        | Backend (Planner API) + services (Parks, Weather) are present. |
| Observe what an agent actually did when something went wrong. | Black-box LLM calls; no trace, no diff.                      | Aspire dashboard + Foundry traces + plan diffs.                |

### Value map

| Products & services                                         | Pain relievers                                                             | Gain creators                                                        |
| ----------------------------------------------------------- | -------------------------------------------------------------------------- | -------------------------------------------------------------------- |
| Reference solution (`PicnicPlanner.slnx`) with ADRs.        | Patterns are co-located and labelled — "this file is the A2A negotiation". | Forkable starting point for the developer's own multi-agent project. |
| AppHost + ServiceDefaults + agents + services + frontend.   | One command brings the whole system up; one command deploys it.            | Confidence the pattern works outside slideware.                      |
| ADR trail (Azure-native, Foundry, Agent Framework).         | Decisions are written down — no archaeology to figure out _why_.           | Easier to defend pattern choices internally.                         |
| Seeded data + simulated event triggers.                     | The killer demo runs deterministically — no flaky live APIs.               | The re-plan scenario lands every time.                               |
| Inline documentation pointing at each demonstrated pattern. | A new reader can find "where's the HITL gate?" without grepping blind.     | Faster time-to-understanding when teaching others.                   |

## 2. End-user audience (secondary, in-fiction)

> ⚠️ We have no real user research. Every entry below is an **assumption** that would need
> validation if this were a real product.

### Customer profile

| Jobs to be done                                              | Pains                                                | Gains                                           |
| ------------------------------------------------------------ | ---------------------------------------------------- | ----------------------------------------------- |
| Plan a picnic for a small group in a few minutes.            | Coordination overhead — texts, polls, separate apps. | One conversation, one plan.                     |
| Pick a park appropriate for the day's weather.               | Forecasts change between planning and the day.       | The plan updates itself when conditions change. |
| Buy the right amount of food without leftovers or shortages. | Guessing quantities for _N_ people.                  | A grocery list sized to the party.              |
| Reserve a pavilion or table when needed.                     | Different reservation systems per park.              | One place to see options.                       |
| Stay within budget.                                          | Cost creeps as items are added piecemeal.            | A live running total with warnings.             |
| Avoid wasted planning effort when plans change.              | Re-doing the whole plan because of one variable.     | Only what changed surfaces; the rest stays put. |

### Value map

| Products & services                                 | Pain relievers                                 | Gain creators                                    |
| --------------------------------------------------- | ---------------------------------------------- | ------------------------------------------------ |
| Conversational wizard for plan creation.            | No app-hopping; one structured intake.         | A picnic plan in ~2 minutes.                     |
| Weather-aware activity & menu recommendations.      | Conditions are factored in from the start.     | Plan that suits the actual day.                  |
| Auto-sized grocery list and budget tracker.         | No mental math on quantities or cost.          | Confidence the budget will hold.                 |
| Reservation holds with explicit confirmation gates. | Nothing is booked without the user saying yes. | User retains control over irreversible actions.  |
| Event-driven re-plan with diff-only surface.        | No "start over" when the forecast moves.       | Trust that the plan reflects the latest reality. |

## 3. Fit check

| Audience            | Highest-leverage promise                            | Biggest unproven assumption                                                 |
| ------------------- | --------------------------------------------------- | --------------------------------------------------------------------------- |
| Developers          | Runnable multi-agent + re-plan + AG-UI in one repo. | Devs will read and adapt rather than skim and forget.                       |
| End users (fiction) | "Only what changed" re-planning.                    | Users want adaptive plans more than they want stable ones; framing matters. |

**Audience direction (confirmed).** The primary developer audience has seen single-agent
demos and is explicitly here for the **multi-agent orchestration + event-driven re-planning**
story. We optimise for breadth across agents and the re-plan loop over depth in any single
agent. This resolves PRD assumption A-3 and locks the editorial centre of gravity of the
demo.

The re-plan diff surface is the single feature that serves _both_ audiences strongly. It is
the right feature to invest the most design attention in.
