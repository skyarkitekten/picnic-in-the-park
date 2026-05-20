# Weather Agent

The Weather Agent helps a caller pick a sunny, not-too-hot day for a picnic. It is a
Microsoft Agent Framework agent hosted with the Foundry responses API, and it uses the
`PicnicPlanner.WeatherService` `/forecast` endpoint as a tool.

It implements **FR-W1** and **FR-W2** from the [PRD](../../../docs/design/prd.md):
fetching forecasts and classifying each day into the picnic risk taxonomy
(`Ideal`, `Acceptable`, `Risk:Rain`, `Risk:Heat`, `Unsafe`).

> **Deferred:** the event-bus push on classification change (**FR-W3**) is not yet
> implemented — it is blocked on the event-bus ADR (PRD open question OQ-1).

## Prerequisites

- .NET 10.0 SDK
- An Azure AI Foundry project with a deployed model
- A running `PicnicPlanner.WeatherService` (or run the whole solution via the Aspire AppHost)

## Getting Started

1. Set the environment variables in a `.env` file:

   ```env
   AZURE_AI_PROJECT_ENDPOINT=https://<your-project>.services.ai.azure.com/api/projects/<project>
   AZURE_AI_MODEL_DEPLOYMENT_NAME=gpt-4o
   WEATHER_SERVICE_URL=http://localhost:5000
   ```

   When run from the Aspire AppHost, `WEATHER_SERVICE_URL` is supplied automatically via
   service discovery (`services__weather-service__https__0`).

2. Log in to Azure:

   ```bash
   az login
   ```

3. Build and run the agent:

   ```bash
   dotnet run
   ```

## Interacting with the Agent

### Test with cURL

Ask the agent to recommend a picnic day from a few candidates:

```bash
curl -X POST http://localhost:8088/responses \
  -H "Content-Type: application/json" \
  -d '{"input": "I want to picnic this weekend. Pick the best day from 2026-05-23, 2026-05-24, or 2026-05-25.", "stream": false}'
```

#### Multi-turn Conversation

Use the previous response ID to continue the conversation (e.g. to refine the
recommendation):

```bash
curl -X POST http://localhost:8088/responses \
  -H "Content-Type: application/json" \
  -d '{"input": "What if I can also do Friday 2026-05-22?", "previous_response_id": "<RESPONSE_ID>"}'
```

### Test with Agent Inspector

While the agent is running locally, you can interact with it visually using the
**Agent Inspector** (AI Toolkit → Developer Tools → Build → Agent Inspector), or run
`AI Toolkit: Open Agent Inspector` from the VS Code command palette.

## Tool

The agent exposes a single tool to the model:

| Tool           | Parameters                       | Returns                                                                                          |
| -------------- | -------------------------------- | ------------------------------------------------------------------------------------------------ |
| `get_forecast` | `eventDate` (`DateOnly`, ISO)    | The 5-day forecast (E-2…E+2) with temperature (°F), condition, and a derived classification per day. |

Classification thresholds (see `WeatherClassifier` in `Program.cs`):

| Classification | Rule                                |
| -------------- | ----------------------------------- |
| `Ideal`        | `Sunny` and 65–80 °F                |
| `Risk:Heat`    | > 85 °F (any condition)             |
| `Risk:Rain`    | `Rain` (and not already `Risk:Heat`)|
| `Acceptable`   | Everything else (e.g. `Cloudy`, or `Sunny` 80–85 °F) |
| `Unsafe`       | Reserved for severe-weather extensions |

## Deploying to Microsoft Foundry

To deploy your agent to Microsoft Foundry:

1. Open the Command Palette (`Ctrl+Shift+P`).
2. Run **Microsoft Foundry: Deploy Hosted Agent**.
3. The extension reads `agent.yaml` and auto-populates what it can. You may be prompted for:
   - **Agent name** -- the name registered with the hosting service.
   - **Dockerfile** -- auto-detected from workspace root, or select manually.
   - **Container registry** -- defaults to auto-created; optionally provide your own ACR.
   - **Resource size** -- CPU and memory allocation:

     | Option                        | CPU  | Memory |
     | ----------------------------- | ---- | ------ |
     | 0.25 CPU cores, 0.5 Gi memory | 0.25 | 0.5 Gi |
     | 0.5 CPU cores, 1 Gi memory    | 0.5  | 1.0 Gi |
     | 1 CPU cores, 2 Gi memory      | 1.0  | 2.0 Gi |
     | 2 CPU cores, 4 Gi memory      | 2.0  | 4.0 Gi |

4. The extension builds the container image in ACR, creates the agent version, and assigns required RBAC roles automatically.

## Troubleshooting

### Azure OpenAI Permission Denied (401)

The identity running the agent does not have the required RBAC roles on the Azure AI
Foundry project. Assign the following roles:

- **Cognitive Services OpenAI User**
- **Azure AI User**

### Weather service URL not set

The agent throws on startup if it cannot resolve a weather service URL. Either set
`WEATHER_SERVICE_URL` in your `.env`, or run the agent through the Aspire AppHost so
service discovery injects `services__weather-service__https__0`.
