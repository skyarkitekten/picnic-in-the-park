# ADR-004: Adopt Microsoft Agent Framework for Pro-Code Agent Development

## Status

**Accepted**

## Context

Picnic Planner is an AI-driven application where agents handle core planning logic — composing information from weather, parks, and other services to produce recommendations. We need a consistent, supported framework for building, testing, and operating those agents in code.

Microsoft Foundry (see [ADR-003](003-microsoft-foundry.md)) supports two distinct agent authoring models:

- **Prompt agents**: Configured entirely in the Foundry portal or via YAML/JSON — no custom code. Suitable for simple, stateless question-and-answer interactions with a fixed set of tools.
- **Hosted agents via the Microsoft Agent Framework**: Pro-code agents authored in C# or Python using the Agent Framework SDK, deployed to Foundry as hosted agents. Suitable for complex, stateful, or multi-step workflows that require custom logic, conditional branching, or orchestration of multiple downstream services.

The team needs a clear rule for which model to use and, crucially, which third-party frameworks are off the table.

## Decision

We will use the **Microsoft Agent Framework** as the sole framework for authoring agents and agentic workflows in Picnic Planner. No other agent framework (LangChain, Semantic Kernel standalone, AutoGen, CrewAI, etc.) is permitted.

### When to use a Prompt Agent (Foundry portal / YAML)

Use a prompt agent when **all** of the following are true:

- The agent's behaviour is fully expressible as a system prompt and a fixed set of Foundry-native tools (e.g., Bing grounding, Azure AI Search, OpenAPI tool).
- The agent is stateless — each invocation is independent with no cross-turn orchestration logic.
- No custom code is required to transform inputs, call internal APIs, or conditionally route between sub-agents.

Prompt agents are appropriate for low-complexity assistants, FAQ bots, or simple retrieval-augmented generation (RAG) interactions.

### When to use a Hosted Agent (Microsoft Agent Framework)

Use a hosted Agent Framework agent when **any** of the following are true:

- The agent must call internal services (e.g., the Parks Service, Weather Service) that are not exposed as Foundry-native tools.
- The agent implements multi-step or conditional workflow logic (e.g., gather weather → evaluate conditions → select parks → compose recommendation).
- The agent participates in a multi-agent pattern (orchestrator / sub-agent) requiring programmatic handoff.
- The agent maintains state across turns within a session.
- Custom input/output transformation, validation, or error-handling logic is required.

All agents in Picnic Planner that interact with backend services or implement planning logic must be implemented as hosted agents using the Agent Framework.

## Consequences

### Positive
- **POS-001**: A single, supported framework reduces cognitive load — the team learns one programming model for all agent development.
- **POS-002**: The Agent Framework is designed for Foundry integration, giving first-class support for deployment, evaluation, and tracing (ADR-003).
- **POS-003**: Pro-code agents are testable with standard .NET tooling — unit tests, integration tests, and CI pipelines apply naturally.
- **POS-004**: The hosted agent model supports multi-agent orchestration patterns without bespoke glue code.
- **POS-005**: Clear selection criteria (prompt agent vs. hosted agent) prevent over-engineering simple interactions and under-engineering complex ones.

### Negative
- **NEG-001**: The Microsoft Agent Framework is newer and evolving; some patterns and APIs may change between releases.
- **NEG-002**: Developers familiar with LangChain or Semantic Kernel alone will need to learn the Agent Framework SDK.
- **NEG-003**: Prompt agents are limited to Foundry-native tools — any gap must be bridged by converting to a hosted agent, which adds development effort.

## Alternatives Considered

### LangChain / LangGraph
- **ALT-001**: **Description**: Use LangChain (Python) or LangGraph for agent and workflow orchestration.
- **ALT-002**: **Rejection Reason**: Not a Microsoft-supported framework; conflicts with the Azure-native mandate (ADR-002). Introduces a Python-first dependency in an otherwise .NET stack, and has no first-class Foundry deployment integration.

### Semantic Kernel (standalone, without Agent Framework)
- **ALT-003**: **Description**: Use Semantic Kernel directly for orchestration without adopting the Agent Framework abstractions.
- **ALT-004**: **Rejection Reason**: The Agent Framework is built on top of Semantic Kernel and adds agent lifecycle, multi-agent coordination, and Foundry deployment — capabilities we need. Using SK standalone would require rebuilding those concerns.

### AutoGen / CrewAI
- **ALT-005**: **Description**: Use AutoGen or CrewAI for multi-agent coordination patterns.
- **ALT-006**: **Rejection Reason**: Third-party frameworks with no Foundry integration path. Violates the Azure-native mandate and would require maintaining a separate deployment and observability pipeline.

### Prompt Agents Only (no pro-code)
- **ALT-007**: **Description**: Implement all agents as Foundry prompt agents, avoiding custom code entirely.
- **ALT-008**: **Rejection Reason**: Picnic Planner agents must call internal APIs and implement conditional planning logic that is not expressible in a static system prompt with Foundry-native tools alone.

## Implementation Notes
- **IMP-001**: All new agents must be reviewed against the prompt agent vs. hosted agent criteria in this ADR before implementation begins.
- **IMP-002**: Agent Framework agents are authored in C# and deployed to Foundry as hosted agents — refer to ADR-003 for deployment guidance.
- **IMP-003**: No NuGet or npm packages for alternative agent frameworks (LangChain, AutoGen, CrewAI, etc.) may be added to any project in this solution.
- **IMP-004**: Multi-agent patterns must use the Agent Framework's built-in orchestrator/sub-agent model — do not implement custom agent-to-agent communication protocols.

## References
- **REF-001**: [Microsoft Agent Framework documentation](https://learn.microsoft.com/azure/ai-foundry/agent-service/)
- **REF-002**: [ADR-003](003-microsoft-foundry.md) — Microsoft Foundry as the hosting platform.
- **REF-003**: [ADR-002](002-azure-native.md) — Azure-native architecture mandate.
