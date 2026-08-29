# Product

<!-- impeccable:product-schema 1 -->

## Platform

web

## Users

**Primary — developers and architects.** .NET/Python developers and solutions architects
evaluating the Microsoft Agent Framework and Microsoft Foundry for multi-agent workloads.
Two documented composites: Maya, a senior .NET backend developer who has shipped
single-LLM-call features and now needs to understand multi-agent boundaries before
committing; and Aiden, a consulting solutions architect who needs operable, traceable,
deployable evidence to recommend Foundry to a client. Both arrive having already seen
single-agent chatbot demos and are here specifically for the multi-agent orchestration and
event-driven re-planning story.

**Secondary — in-fiction end users.** Linnea, a working parent planning a Saturday park
outing with two kids; Jonas, the friend-group coordinator handling a 6–10 person picnic.
They exist to keep the demo domain plausible, not to be served as a real market.

Full personas: `docs/design/personas.md`. They are stated design hypotheses, not research
findings.

## Product Purpose

Picnic In The Park is a runnable **reference implementation** of a multi-agent system built
on the Microsoft Agent Framework, hosted in Microsoft Foundry, and deployed on Azure-native
services. It has two intertwined purposes, and the pedagogical one wins every trade-off:

1. **Pedagogical (primary).** Demonstrate orchestrator + specialist composition,
   agent-to-agent (A2A) negotiation, per-agent UI via AG-UI, structured Human-In-The-Loop
   confirmation gates, and — the headline pattern — **selective re-planning in response to
   real-world events**.
2. **In-fiction (secondary).** Be a plausible agentic assistant that turns "let's picnic
   Saturday" into an end-to-end plan that adapts as conditions change.

Success means a developer can clone, run `aspire run`, and reach the headline re-plan
scenario without reading more than the README; every demonstrated pattern has a "find it
here" pointer in the code; and a facilitator can run the demo from a one-page script.

## Positioning

The differentiating mechanism is the **event-driven selective re-plan with a diff-only
surface**: a simulated forecast change wakes the Coordinator, which diffs new world-state
against the existing plan, re-runs only the impacted specialists, and surfaces to the user
*only what changed* rather than a regenerated plan. Neighbouring samples demonstrate single
agents, tool calls, or static orchestration; they do not demonstrate a plan that revises
itself and shows its own delta. This is documented as the single feature that serves both
the developer and end-user audiences, and the one that most deserves design investment.

## Operating Context

- **Primary local workflow.** `aspire run` from the repo root brings up AppHost, backend,
  services, agents, and frontend. Cold start to first plan target is ≤ 60 seconds.
- **Deployment workflow.** A single `azd up` provisions and deploys to Azure.
- **Observability workflow.** Agent invocations, tool calls, and re-plan events are
  inspected in the Aspire dashboard and Foundry traces; this observation is part of the
  product experience for the primary audience, not backstage plumbing.
- **Viewing context (confirmed).** The frontend must work equally well in two situations:
  projected live in front of a conference or workshop room by a presenter, **and** read
  close-up on a developer's own laptop while exploring solo. Future design work must hold
  up at projector legibility without becoming coarse at reading distance.
- **Headline demo scene.** Thursday: user requests a Saturday picnic (party size, budget,
  radius, dietary notes) through a wizard; each specialist returns a card. Friday: the
  simulated forecast flips to thunderstorms; the wizard wakes showing only the deltas for
  confirmation. Day-of: a confirmed plan view. Mapped in
  `docs/design/journey-map.md` and `docs/design/service-blueprint.md`.

## Capabilities and Constraints

**Confirmed capabilities.** Coordinator agent producing a `PicnicPlan` aggregate and a
`PlanDiff` artefact; Weather agent with a small risk taxonomy (`Ideal`, `Acceptable`,
`Risk:Rain`, `Risk:Heat`, `Unsafe`) that emits classification-change events; Activity/Park
agent ranking parks by conditions and returning map coordinates; Menu Planner; Grocery
agent with per-item simulated costs; Reservation agent that holds but never confirms
without an explicit gate; Budget agent maintaining a running total with over-budget
warnings. Frontend surfaces: chat, structured wizard, AG-UI card host, and a diff view.

**Technical constraints.** .NET 10 / C# for backend, services, and agents; React 19 +
Vite + TypeScript for the frontend; .NET Aspire 13+ for orchestration; Azure Developer CLI
and Bicep for infrastructure; Azure-native event bus. Seeded weather, parks, and
reservation data keeps the demo deterministic. A specialist agent failure must degrade the
plan, not crash the Coordinator. No secrets in source — credentials flow through Aspire and
Azure managed identity. Demo cost ceiling: a 30-minute Azure run under $5.

**Explicitly out of scope.** Real reservations, payments, or grocery ordering (all
simulated); production multi-tenant identity, billing, or quotas; a mobile app — the
reference UI is web only; geographic coverage beyond a small seeded set of demo locations;
accounts, personalisation, or memory beyond the active plan session; dietary, allergen, or
medical compliance guarantees.

**Terminology.** Coordinator, specialist agent, A2A, AG-UI, HITL, `PicnicPlan`, `PlanDiff`,
re-plan, confirmation gate, hold vs. confirm.

**Open product decisions (do not invent answers).** Which Azure-native event bus (Service
Bus topics vs. Event Grid vs. Web PubSub); whether Activity ranking uses a model call or a
deterministic ranker; how the simulated weather event is triggered in the UI (dev panel,
time-warp control, or random tick); whether Budget is an agent or a deterministic service;
whether the seeded park dataset covers one city or several.

## Brand Commitments

**None binding (confirmed).** The demo is free to carry its own identity. There is no
requirement to display Microsoft, Azure, or Aspire branding, and no external brand
guideline to follow.

Current state is an unclaimed default rather than a commitment: the page title is still
"Aspire Starter", the favicon is `public/Aspire.png`, and typography falls back to
`system-ui`. The product name **Picnic In The Park** is the one fixed element.

## Evidence on Hand

- `docs/design/prd.md` — full product requirements, goals, functional and non-functional
  requirements, success criteria.
- `docs/design/personas.md`, `value-proposition.md`, `journey-map.md`,
  `service-blueprint.md`, `system-dynamics.md`, `notes.md` — design artefacts.
- `docs/decisions/` — ADR trail (ADRs 001–004: adopt ADRs, Azure-native, Microsoft Foundry,
  Microsoft Agent Framework). The ADR trail is itself a deliverable Aiden shows clients.
- Working code: `src/agents/`, `src/backend/PicnicPlanner.Planner.Api/`,
  `src/services/`, `src/frontend/`, `src/app-host/`, `infra/`.
- Seeded weather, parks, and reservation data for deterministic demo runs.

**Absences future work must not fabricate.** There is no user research — every persona and
end-user claim is a stated assumption. There are no customers, testimonials, adoption
numbers, benchmarks, press mentions, or pricing. There is no real booking, payment, or
grocery integration, and no photography or illustration library in the repo beyond
`Aspire.png` and `github.svg`.

## Product Principles

1. **Pedagogy outranks fiction.** When the demo story and the teaching story conflict,
   optimise for what a developer learns.
2. **Legibility over automation.** Across every persona, the system must show what it did
   and why; visible reasoning, traces, and diffs are the product.
3. **Nothing irreversible without an explicit gate.** Confirmation is a designed feature,
   not friction to minimise away.
4. **Only what changed.** Re-planning surfaces deltas, never a regenerated wall of plan.
5. **Determinism protects the demo.** Seeded data and simulated events mean the headline
   scenario lands the same way every time.

## Accessibility & Inclusion

**WCAG 2.2 AA (confirmed).** As a reference implementation, the project should model good
practice rather than merely meet a floor. Existing code already uses ARIA labelling on
cards, wizard, and chat, and honours `prefers-reduced-motion`. The dual viewing context —
projected in a room and read on a laptop — makes contrast and type legibility load-bearing
rather than cosmetic.
