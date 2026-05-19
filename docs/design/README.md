# Design Docs — Picnic In The Park

This directory holds the product and design artefacts for the Picnic In The Park reference
application. Architecture _decisions_ live in [../decisions/](../decisions/README.md); this
directory holds the _thinking_ — product intent, user-experience design, and systems
analysis — that those decisions ride on top of.

The artefacts are organised across three lenses:

- **Viability** — should we build this, for whom, and what does success look like?
- **Desirability** — who are the users, what do they need, and what is their journey?
- **Feasibility & dynamics** — how does the system behave, where are the loops and leverage
  points?

## Table of contents

| #   | Document                                     | Lens                       | Purpose                                                                                                                                |
| --- | -------------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| 1   | [notes.md](notes.md)                         | Source material            | Original architecture sketch — agent layers, patterns to showcase, the re-planning "killer demo", A2A candidates.                      |
| 2   | [prd.md](prd.md)                             | Product Summary            | Product Requirements Document — vision, audiences, goals/non-goals, headline scenario, functional & non-functional requirements.       |
| 3   | [value-proposition.md](value-proposition.md) | Viability                  | Dual value-proposition canvas — developer audience (primary) and end-user audience (in-fiction).                                       |
| 4   | [personas.md](personas.md)                   | Desirability               | Four personas — two developer/architect, two in-fiction end users — with the cross-persona insight that legibility is the throughline. |
| 5   | [journey-map.md](journey-map.md)             | Desirability               | Eight-stage end-user journey, including the weather-delta re-plan, mapped to demonstrated framework patterns.                          |
| 6   | [service-blueprint.md](service-blueprint.md) | Desirability + Feasibility | Swim-lane blueprint with Mermaid sequence diagrams for plan-creation and re-plan flows, plus failure modes and observability points.   |
| 7   | [system-dynamics.md](system-dynamics.md)     | Feasibility                | Systems-thinking analysis — boundaries, stocks/flows, causal loops, delays, archetypes, and eight concrete design interventions.       |

## Suggested reading orders

| If you are…                                  | Read in this order                                                                                                                         |
| -------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
| New to the project                           | [prd.md](prd.md) → [journey-map.md](journey-map.md) → [notes.md](notes.md)                                                                 |
| Designing or building an agent               | [prd.md §7](prd.md#7-functional-requirements) → [service-blueprint.md](service-blueprint.md) → [system-dynamics.md](system-dynamics.md)    |
| Preparing a demo or workshop                 | [prd.md §6](prd.md#6-headline-scenario--the-killer-demo) → [journey-map.md](journey-map.md) → [service-blueprint.md](service-blueprint.md) |
| Evaluating the patterns for your own product | [value-proposition.md](value-proposition.md) → [personas.md](personas.md) → [system-dynamics.md](system-dynamics.md)                       |

## Status

All documents are **drafts** living on the `feat/00-product-specs` branch. Architectural
decisions extracted from these drafts land as ADRs in [../decisions/](../decisions/README.md).

## Open questions tracker

The current set of open questions and assumptions lives in
[prd.md §11](prd.md#11-open-questions--assumptions). Resolving an open question typically
results in either an ADR or an update to the relevant design doc here.
