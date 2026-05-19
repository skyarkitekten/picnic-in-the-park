# Personas — Picnic In The Park

> Personas below are **composite assumptions** drawn from typical audiences for a Microsoft
> Agent Framework reference implementation and from imagined end users. They are not based
> on user research and should be treated as design hypotheses, not findings.

## Primary — Developer & architect personas

### P1. Maya — Backend developer exploring agentic AI

- **Role.** Senior .NET backend developer at a mid-size B2B SaaS company.
- **Context.** Her team has been asked to "add AI" to an existing product. She has
  experimented with single-LLM-call features and now needs to understand multi-agent
  patterns before committing.
- **Goals.**
  - Understand what an _agent_ actually is in the Microsoft Agent Framework, vs. a tool
    call vs. a plain LLM call.
  - See a working orchestrator + specialists example she can map onto her own services.
  - Decide whether to use prompt agents or hosted agents for her own use case.
- **Frustrations.**
  - Most examples are toy single-agent chatbots.
  - Architecture diagrams without runnable code.
  - "Hello world" agents that don't call any internal services.
- **What she needs from Picnic In The Park.**
  - A repo she can `git clone` and `aspire run`.
  - One file per agent so the boundaries are obvious.
  - A clear pointer to "this is where Coordinator hands off to Weather".

### P2. Aiden — Solutions architect evaluating Foundry

- **Role.** Cloud solutions architect at a consulting firm; advises enterprise clients on
  Azure AI adoption.
- **Context.** A client is deciding between Foundry hosted agents, third-party frameworks,
  and rolling their own. Aiden needs evidence to recommend.
- **Goals.**
  - Validate that hosted Agent Framework agents can be observed, traced, and operated.
  - See an end-to-end deployment story (`azd up`) so he can scope a client engagement.
  - Confirm that A2A and event-driven patterns are achievable without custom glue.
- **Frustrations.**
  - Vendor demos that skip ops, observability, and cost.
  - Sample apps that don't survive contact with a real Azure subscription.
- **What he needs from Picnic In The Park.**
  - Working ADR trail he can show clients.
  - Aspire dashboard + Foundry traces visible in the demo.
  - A cost ceiling he can quote (see NFR-4 in [prd.md](prd.md)).

## Secondary — End-user personas (in-fiction)

### P3. Linnea — Parent planning a weekend outing

- **Role.** Parent of two (ages 6 and 9), works full-time.
- **Context.** Wants to take the kids to a park on Saturday afternoon. Has done this enough
  times to know that weather, food, and "is there a toilet near the playground" all matter.
- **Goals.**
  - Pick a park where the kids can run, with shade and a toilet.
  - Bring food the kids will actually eat, within a modest budget.
  - Not spend Friday evening re-planning when the forecast moves.
- **Frustrations.**
  - Last-minute weather changes wreck plans.
  - Coordinating with their partner via texts and screenshots.
  - Forgetting one critical item (ice, sunscreen, the football).
- **What she needs from Picnic In The Park.**
  - A wizard that asks the right questions and remembers them.
  - A plan that updates without making her start over.
  - A clear shopping list she can hand off or do herself.

### P4. Jonas — Friend-group coordinator

- **Role.** Always the one who organises the group hang. Single, late 20s, lives in the
  city.
- **Context.** Six to ten friends, casual Saturday picnic in a city park. Budget split
  later, but he fronts the planning.
- **Goals.**
  - Pick a park that works for a larger group (table or pavilion).
  - Reserve when possible; have a backup when not.
  - Keep the menu interesting but cheap per person.
- **Frustrations.**
  - Reservation systems differ per park and city.
  - Drift between "we said 6 people" and "actually 9 are coming".
  - Cost transparency for the group split.
- **What he needs from Picnic In The Park.**
  - A budget card he can screenshot and send to the group.
  - Reservation options with clear hold-vs-confirm semantics.
  - Easy party-size change that propagates through the plan.

## Cross-persona insight

| Across all four                                                                            |
| ------------------------------------------------------------------------------------------ |
| All personas value **legibility** — they want to see _what the system did and why_.        |
| All personas distrust **silent automation** — confirmation gates are a feature, not a tax. |
| All personas benefit from the **diff-only re-plan surface** — it's the universal feature.  |

This insight motivates [§6 of the PRD](prd.md#6-headline-scenario--the-killer-demo) and
the leverage points in [system-dynamics.md](system-dynamics.md).
