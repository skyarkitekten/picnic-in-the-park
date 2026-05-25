# AGENTS.md

---

## Repository Structure

```text
picnic-in-the-park/
├── src/
│   ├── agents/                     # Application agents (Microsoft Agent Framework)
│   │   ├── coordinator-agent/      # Orchestrates specialist agents into a picnic plan (.NET)
│   │   ├── invitation-agent/       # Sends picnic invitations (.NET)
│   │   └── weather-agent/          # Fetches forecasts & classifies picnic risk (.NET)
│   ├── backend/
│   │   └── PicnicPlanner.Planner.Api/  # REST API for the planner
│   ├── frontend/                # Vite + TypeScript UI
│   ├── services/
│   │   ├── PicnicPlanner.ParksService/   # Parks ranking service
│   │   └── PicnicPlanner.WeatherService/ # Weather forecast service
│   └── app-host/                # .NET Aspire AppHost (orchestration)
├── docs/
│   ├── decisions/               # Architecture Decision Records (ADRs)
│   └── design/                  # Design artifacts (PRD, personas, journey maps)
├── infra/                       # Azure infrastructure (AZD / Bicep)
├── .github/
│   ├── agents/                  # Copilot CLI custom agent definitions
│   ├── skills/                  # Copilot CLI skill definitions
│   └── instructions/            # Language-specific coding instructions
└── .mcp.json                    # MCP server configuration (Aspire)
```

### Key Technology Choices

| Layer               | Technology                        |
| ------------------- | --------------------------------- |
| Multi-agent runtime | Microsoft Agent Framework (MAF)   |
| Orchestration       | .NET Aspire 13+                   |
| Backend / Agents    | .NET 10 (C#)                      |
| Frontend            | Vite + TypeScript (React)         |
| Infra               | Azure Developer CLI (AZD) + Bicep |

---

## Coding Agents and Subagents(`.github/agents/`)

Custom agents available for coding assistants with specific expertise.

| Agent file                      | Name                             | When to use                                                                                    |
| ------------------------------- | -------------------------------- | ---------------------------------------------------------------------------------------------- |
| `neo.arch.adr.agent.md`         | **ADR Author**                   | Creating Architecture Decision Records in `docs/decisions/`                                    |
| `neo.design.systems.agent.md`   | **Systems Thinking Facilitator** | Mapping feedback loops, stocks & flows, leverage points, and system dynamics                   |
| `neo.design.thinking.agent.md`  | **Design Thinking Facilitator**  | Empathy mapping, persona work, problem framing, journey mapping, ideation                      |
| `neo.product.coach.agent.md`    | **Product Coach**                | Validating feature decisions, product-market fit, Business Model Canvas, PRDs                  |
| `neo.product.engineer.agent.md` | **Business Engineer**            | End-to-end product discovery — orchestrates Product Coach + Design Thinking + Systems Thinking |

---

## Coding Agent Skills (`.github/skills/`)

Skills extend coding agents with domain-specific knowledge.

| Skill                 | When to use                                                                                                          |
| --------------------- | -------------------------------------------------------------------------------------------------------------------- |
| `agent-framework`     | Working with Microsoft Agent Framework — agent creation, communication patterns, multi-agent coordination            |
| `aspire`              | Operating the Aspire AppHost — start/stop/restart resources, inspect logs & traces, add integrations, manage secrets |
| `dotnet-inspect`      | Querying .NET APIs — searching NuGet packages, listing API surfaces, finding extension methods and implementors      |
| `neo-create-adr`      | Recording an architecture decision — creates a new ADR in `docs/decisions/`                                          |
| `neo-design-thinking` | Facilitating design thinking — stakeholder maps, empathy maps, journey maps, ideation workshops                      |
| `neo-system-thinking` | Facilitating systems thinking — boundary mapping, feedback loops, archetype analysis, leverage points                |

---

## Coding Instructions (`.github/instructions/`)

Language-specific instructions applied automatically when editing matching files.

| File                         | Applies to                                                  |
| ---------------------------- | ----------------------------------------------------------- |
| `typescript.instructions.md` | `**/*.{ts,tsx}` — TypeScript 5.x conventions, strict types  |
| `csharp.instructions.md`     | `**/*.cs` — C# conventions, DTO patterns, minimal API style |

---

## Common Tasks

- **Run locally:** `aspire run` from the repo root
- **Build all:** `dotnet build ./src/PicnicPlanner.slnx` and `npm run build --prefix ./src/frontend`
- **Run tests:** `dotnet test ./src/PicnicPlanner.slnx` and `npm test --prefix ./src/frontend`
- **Add an ADR:** Use the `/agent` → **ADR Author** or `/skills` → `neo-create-adr`
- **Explore Aspire resources:** Use the `/skills` → `aspire` skill
- **Understand agent wiring:** Use the `/skills` → `agent-framework` skill
