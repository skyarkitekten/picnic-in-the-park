// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Concurrent;
using System.Text.Json;

using Azure.AI.AgentServer.Core;
using Azure.AI.Projects;
using Azure.Identity;

using CoordinatorAgent;
using CoordinatorAgent.Stubs;

using DotNetEnv;

using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry.Hosting;

Env.TraversePath().Load();

var projectEndpoint = new Uri(Environment.GetEnvironmentVariable("AZURE_AI_PROJECT_ENDPOINT")
    ?? throw new InvalidOperationException("AZURE_AI_PROJECT_ENDPOINT environment variable is not set."));
var deployment = Environment.GetEnvironmentVariable("AZURE_AI_MODEL_DEPLOYMENT_NAME") ?? "gpt-4o";
var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");

// ── In-memory plan store ──
var plans = new ConcurrentDictionary<string, PicnicPlan>();

var credentialOptions = new DefaultAzureCredentialOptions();
if (!string.IsNullOrWhiteSpace(tenantId))
{
    credentialOptions.TenantId = tenantId;
    credentialOptions.AdditionallyAllowedTenants.Add(tenantId);
}

AIAgent agent = new AIProjectClient(projectEndpoint, new DefaultAzureCredential(credentialOptions))
    .AsAIAgent(
        model: deployment,
        instructions: """
            You are the Picnic Plan Coordinator — an orchestrator that creates a complete
            picnic plan by calling specialist agents in order.

            When you receive a plan request you MUST call every tool exactly once in this
            order:
              1. get_weather   – fetch forecast for the event date
              2. get_parks     – recommend parks using weather risk
              3. plan_menu     – propose a menu considering party size, budget, dietary
                                 notes, and weather
              4. get_groceries – build a shopping list from the menu
              5. hold_shelter  – reserve the top-ranked park shelter
              6. calculate_budget – compute the budget summary

            After all tools return, respond with **only** the assembled JSON plan using
            this exact schema (no markdown fences, no extra keys):

            {
              "planId": "<guid>",
              "request": { ... },
              "weather": { ... },
              "parks": [ ... ],
              "menu": { ... },
              "grocery": { ... },
              "reservation": { ... },
              "budget": { ... },
              "status": "Draft"
            }

            Never skip a tool. Never reorder the tools. Always return valid JSON.
            """,
        name: "coordinator-agent",
        description: "Orchestrates specialist agents to create a complete picnic plan");

var builder = AgentHost.CreateBuilder(args);
builder.Services.AddFoundryResponses(agent);
builder.RegisterProtocol("responses", endpoints => endpoints.MapFoundryResponses());

var app = builder.Build();
app.Run();
