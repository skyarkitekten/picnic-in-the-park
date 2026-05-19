# ADR-003: Adopt Microsoft Foundry as the Agent and Model Hosting Platform

## Status

**Accepted**

## Context

Picnic Planner uses AI agents to deliver its core planning capabilities. Those agents require a managed platform to host, version, and serve both the models they rely on and the agent runtimes themselves. We need to decide where agents and models are deployed and operated.

Key forces:

- The project is committed to an Azure-native architecture (see [ADR-002](002-azure-native.md)).
- AI agent development follows the Microsoft Agent Framework (see [ADR-004](004-microsoft-agent-framework.md)).
- We need a platform that supports model deployment, prompt management, agent hosting, evaluation, and observability — without managing underlying infrastructure.
- The team needs a consistent developer experience from local development through to production deployment.
- Security and data residency requirements favour a managed Azure service over third-party model providers.

## Decision

We will use **Microsoft Foundry** (Azure AI Foundry) as the unified platform for hosting AI agents and models in Picnic Planner. This covers:

- **Model hosting**: Deploy and version models (including Azure OpenAI models) through the Foundry model catalogue.
- **Agent hosting**: Deploy Microsoft Agent Framework agents as Foundry-hosted agents, leveraging the Foundry runtime for lifecycle management.
- **Prompt management**: Manage and version system prompts and agent configurations within Foundry.
- **Evaluation**: Use Foundry's built-in evaluation tooling to measure agent quality and track regressions.
- **Observability**: Route agent traces and metrics through Foundry's monitoring integration with Azure Monitor / Application Insights.

All agent and model deployments go through Foundry; direct calls to the Azure OpenAI endpoint outside of a Foundry-managed deployment are not permitted except for local developer testing.

## Consequences

### Positive

- **POS-001**: Unified platform for the full agent lifecycle — develop, deploy, evaluate, monitor — reducing integration effort.
- **POS-002**: Native integration with Azure identity (Managed Identity, Entra ID) and Azure networking, consistent with ADR-002.
- **POS-003**: Built-in evaluation and continuous monitoring reduces the effort to track agent quality over time.
- **POS-004**: Foundry's deployment model abstracts compute management, keeping operational overhead low.
- **POS-005**: First-class support for Microsoft Agent Framework, aligning with ADR-004.

### Negative

- **NEG-001**: Increases dependency on Microsoft Foundry's release cadence — breaking changes in the platform require coordinated updates.
- **NEG-002**: Foundry is a relatively new platform; some features are still maturing and may require workarounds.
- **NEG-003**: Local developer experience requires Foundry connectivity or emulation for end-to-end testing.

## Alternatives Considered

### Direct Azure OpenAI Service (no Foundry)

- **ALT-001**: **Description**: Call Azure OpenAI endpoints directly, managing model deployments and agent state in application code.
- **ALT-002**: **Rejection Reason**: Lacks built-in agent lifecycle management, evaluation, and prompt versioning. Requires bespoke tooling to replicate what Foundry provides out of the box.

### Third-Party Agent Platforms (e.g., LangSmith, Vertex AI Agent Builder)

- **ALT-003**: **Description**: Use a non-Microsoft platform for agent hosting and evaluation.
- **ALT-004**: **Rejection Reason**: Conflicts with the Azure-native mandate (ADR-002). Introduces cross-cloud data flows and additional identity federation complexity.

### Self-Managed Agent Runtime on AKS

- **ALT-005**: **Description**: Package agents as containers and operate them on AKS with a custom runtime.
- **ALT-006**: **Rejection Reason**: High operational burden. Foundry provides the same capabilities as a managed service, consistent with our preference for Azure PaaS.

## Implementation Notes

- **IMP-001**: Each agent maps to a named Foundry agent resource; deployments are versioned.
- **IMP-002**: Foundry connections to models must use Managed Identity — no API keys in application configuration.
- **IMP-003**: Evaluation datasets and runs should be stored in Foundry and reviewed as part of the PR / release process.
- **IMP-004**: Use the Foundry SDK (azure-ai-projects) to interact with agents and models from application code.

## References

- **REF-001**: [Azure AI Foundry documentation](https://learn.microsoft.com/azure/ai-foundry/)
- **REF-002**: [ADR-002](002-azure-native.md) — Azure-native architecture mandate.
- **REF-003**: [ADR-004](004-microsoft-agent-framework.md) — Microsoft Agent Framework adoption.
