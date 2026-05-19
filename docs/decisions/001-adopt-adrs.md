# ADR-001: Adopt Architecture Decision Records

## Status

**Accepted**

## Context

The Picnic Planner project is a distributed system spanning multiple services, an Aspire AppHost, a React frontend, and AI agent integrations. As the architecture evolves, the team needs a reliable way to capture, communicate, and revisit significant design decisions. Without a structured record, decisions are lost in chat threads, meetings, or tribal knowledge — making it difficult for new contributors to understand why the system is shaped the way it is, and for the team to evaluate whether past decisions still hold.

Key forces at play:

- Multiple components and services with interconnected architectural concerns
- Decisions often involve trade-offs that are not obvious without context
- The project will onboard new contributors who need to ramp up quickly
- Reversing undocumented decisions is risky and expensive

## Decision

We will use Architecture Decision Records (ADRs) to document all significant architectural decisions for the Picnic Planner project. Each ADR will be stored as a Markdown file in `docs/decisions/` following a sequential numbering scheme (`{NNN}-{slug}.md`). An index in `docs/decisions/README.md` will provide a quick-reference table of all ADRs.

Each ADR will include at minimum: a title, status, context, decision, consequences, and alternatives considered.

## Consequences

### Positive

- **POS-001**: Architectural decisions are discoverable and durable — anyone can read the history.
- **POS-002**: Forces explicit consideration of trade-offs and alternatives before committing to a direction.
- **POS-003**: Simplifies onboarding by giving new contributors a clear trail of "why" behind the architecture.
- **POS-004**: Provides a lightweight review mechanism — ADRs can be discussed in PRs before acceptance.

### Negative

- **NEG-001**: Adds a small amount of overhead to the decision-making process (writing and reviewing the record).
- **NEG-002**: ADRs can become stale if not actively maintained when decisions are superseded or reversed.

## Alternatives Considered

### Informal Documentation (Wiki / Confluence)

- **ALT-001**: **Description**: Store decisions in a wiki or external documentation tool.
- **ALT-002**: **Rejection Reason**: Wikis drift out of sync with the codebase. Keeping ADRs in-repo ties decisions to the code they affect and leverages the existing PR review workflow.

### No Formal Record

- **ALT-003**: **Description**: Rely on commit messages, PR descriptions, and team memory.
- **ALT-004**: **Rejection Reason**: Decisions are scattered and unsearchable. Context is lost when team members rotate or memory fades.

## Implementation Notes

- **IMP-001**: ADRs are numbered sequentially starting at `001`.
- **IMP-002**: Once accepted, an ADR is immutable. If a decision is reversed or superseded, a new ADR is created that references the original.
- **IMP-003**: The `docs/decisions/README.md` index must be updated whenever a new ADR is added.

## References

- **REF-001**: Michael Nygard, "Documenting Architecture Decisions" — the original ADR proposal.
- **REF-002**: [adr.github.io](https://adr.github.io/) — community resources and tooling for ADRs.
