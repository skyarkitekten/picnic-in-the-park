# Picnic Design Notes

**Architecture Layers Worth Calling Out**

```
User Layer       → Chat UI + Wizard (structured HITL) + AG-UI rendered components
Orchestration    → Plan Coordinator (stateful, event-aware)
Agent Layer      → Specialists (your current 3 + additions)
Integration      → Tools / APIs per agent
Event Bus        → Triggers re-planning (weather delta, reservation change)
```

---

**Key Patterns to Showcase**

| Pattern                       | How it surfaces in Picnic                                                                          |
| ----------------------------- | -------------------------------------------------------------------------------------------------- |
| **Orchestrator → Specialist** | Coordinator sequences agents, manages dependencies                                                 |
| **Event-driven re-planning**  | Forecast changes → Coordinator re-evaluates → notifies affected agents → surfaces delta to user    |
| **A2A**                       | Menu Planner ↔ Grocery Agent negotiate quantities; Weather ↔ Activity Agent adjust recommendations |
| **AG-UI**                     | Each agent renders its own UI card (weather widget, menu card, map) — great visual demo            |
| **HITL via Wizard**           | Structured input at plan creation; confirmation gates before reservation booking                   |
| **Memory/State**              | Plan context persists across re-planning cycles                                                    |

---

**The Re-Planning Story is Your Killer Demo**

This is what separates "chatbot PoC" from "agentic system" — your signature thesis, applied to a simple domain. The flow:

> Forecast updates → Event bus fires → Coordinator wakes → diffs old vs new plan → selectively re-runs affected agents → presents user with _only what changed_

That's a publishable pattern right there.

---

**A2A Candidates Worth Highlighting**

- **Weather → Activity Planner** (push: conditions changed)
- **Menu Planner → Grocery Agent** (pull: finalize quantities)
- **Reservation Agent → Coordinator** (async callback: confirmation received)
- **Budget Agent** (subscribes to all agents, aggregates cost events)
