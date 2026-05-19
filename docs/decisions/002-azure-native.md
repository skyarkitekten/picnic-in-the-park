# ADR-002: Adopt Azure-Native Architecture

## Status

**Accepted**

## Context

The Picnic Planner project needs a cloud platform and an approach to service selection. The organisation is an established Azure shop — all existing infrastructure, identity, billing, and operational tooling runs on Azure. There is no mandate or business case to evaluate AWS or GCP.

The real decision is _how_ we use Azure: do we adopt Azure-native PaaS services, or do we favour open-source / cloud-agnostic alternatives that happen to run on Azure (e.g., self-managed Kubernetes, OSS databases on VMs, Terraform-only IaC)?

Key forces:

- The team already holds Azure certifications and operational experience.
- Azure PaaS services offer built-in scaling, patching, monitoring, and SLA-backed availability — reducing undifferentiated operational burden.
- Cloud-agnostic abstractions add complexity and limit access to platform-specific capabilities (managed identity, Entra ID integration, Azure Monitor, Private Link).
- The project includes .NET Aspire orchestration, which has first-class Azure integration.
- Portability to another cloud is not a business requirement.

## Decision

We will adopt an Azure-native architecture, preferring Azure PaaS services over open-source self-managed or cloud-agnostic alternatives. Specifically:

- Use Azure PaaS offerings as the default choice for compute, data, messaging, and AI (e.g., Azure App Service / Container Apps, Azure Cosmos DB / Azure SQL, Azure Service Bus, Azure AI Services).
- Use Azure-native identity and security primitives (Entra ID, Managed Identity, Key Vault) rather than third-party equivalents.
- Use Azure-native observability (Azure Monitor, Application Insights) rather than self-hosted stacks.
- Use Bicep / ARM as the primary IaC tooling, complementing the Aspire deployment model.
- Only introduce OSS or third-party services when Azure has no equivalent offering, or when a specific Azure service has a documented gap for the use case at hand.

## Consequences

### Positive

- **POS-001**: Reduced operational overhead — Microsoft manages patching, scaling, and high-availability for PaaS services.
- **POS-002**: Seamless integration across the Azure ecosystem (identity, networking, monitoring, cost management).
- **POS-003**: Leverages the team's existing Azure expertise; no ramp-up cost for a new platform.
- **POS-004**: .NET Aspire's Azure-native integrations work out of the box, accelerating local-to-cloud parity.
- **POS-005**: Consistent SLA coverage backed by Microsoft support agreements.

### Negative

- **NEG-001**: Vendor lock-in — migrating to another cloud provider would require significant rework.
- **NEG-002**: Azure-specific IaC and SDK patterns reduce the pool of reusable community tooling compared to cloud-agnostic approaches.
- **NEG-003**: Some Azure PaaS services have opinionated defaults that may constrain design flexibility in edge cases.

## Alternatives Considered

### Cloud-Agnostic / OSS-on-Azure

- **ALT-001**: **Description**: Use cloud-agnostic services (e.g., self-managed PostgreSQL, Kubernetes, Terraform, Prometheus/Grafana) deployed on Azure VMs or AKS to preserve portability.
- **ALT-002**: **Rejection Reason**: Portability is not a business requirement. The added complexity of managing OSS infrastructure, writing abstraction layers, and forgoing Azure-native integrations is cost without benefit for this project.

### Multi-Cloud

- **ALT-003**: **Description**: Design for deployment across Azure, AWS, and GCP simultaneously.
- **ALT-004**: **Rejection Reason**: The organisation runs exclusively on Azure. Multi-cloud adds architectural complexity and operational burden with no corresponding business driver.

## Implementation Notes

- **IMP-001**: When evaluating a new service need, check the Azure PaaS catalogue first. Only propose an OSS alternative if Azure has no suitable offering or a documented limitation applies.
- **IMP-002**: All Azure resources must use Managed Identity for authentication where supported — no connection-string secrets.
- **IMP-003**: Tag all Azure resources consistently for cost tracking and environment identification.

## References

- **REF-001**: [Azure Well-Architected Framework](https://learn.microsoft.com/azure/well-architected/) — guiding principles for Azure-native design.
- **REF-002**: [ADR-001](001-adopt-adrs.md) — establishes the ADR process used here.
