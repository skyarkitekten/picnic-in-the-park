# Coordinator Agent

Orchestrates specialist agents to produce a complete picnic plan.

## Specialist agents (currently stubbed)

| Agent | Purpose |
|-------|---------|
| Weather | Forecast + risk classification for the event date |
| Parks | Rank parks / shelters by conditions + weather risk |
| Menu | Propose menu for party size, budget, dietary notes, weather |
| Grocery | Build a priced shopping list from the menu |
| Reservation | Hold a shelter at the top-ranked park |
| Budget | Aggregate costs and compute remaining budget |

## Local testing

```bash
# Set required environment variables
export AZURE_AI_PROJECT_ENDPOINT="https://<your-project>.services.ai.azure.com/api/projects/<id>"
export AZURE_AI_MODEL_DEPLOYMENT_NAME="gpt-4o"

# Run
dotnet run

# POST a plan request
curl -X POST http://localhost:8088/responses \
  -H "Content-Type: application/json" \
  -d '{
    "input": "Plan a picnic for Saturday June 7 2026, 4 people, $80 budget, near the lake, no dietary restrictions",
    "stream": false
  }'
```

## Docker

```bash
docker build -t coordinator-agent .
docker run -p 8088:8088 \
  -e AZURE_AI_PROJECT_ENDPOINT="..." \
  -e AZURE_AI_MODEL_DEPLOYMENT_NAME="gpt-4o" \
  coordinator-agent
```

## Foundry deployment

Use VS Code AI Toolkit → **Microsoft Foundry: Deploy Hosted Agent**.
The `agent.yaml` in this directory is read automatically.

### Required RBAC roles

The agent's managed identity needs:
- `Cognitive Services OpenAI User` on the Foundry project
- `Azure AI User` on the Foundry project
