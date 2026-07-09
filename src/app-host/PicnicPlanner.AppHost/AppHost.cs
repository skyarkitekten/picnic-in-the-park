var builder = DistributedApplication.CreateBuilder(args);

// The cache is a password-protected, internal-only dependency: it has no HTTP
// endpoint and must never be given external ingress when deployed to Azure
// Container Apps. Do not add `.WithExternalHttpEndpoints()` (or any other
// public-facing endpoint) to this resource.
//
// No explicit password parameter is passed here: AddRedis auto-generates a
// secured "cache-password" parameter with a generated default value. Supplying
// our own AddParameter("cache-password", secret: true) with no default left
// azd unable to resolve the securedParameter("cache_password") during
// `azd deploy`, since there was no value and no prompt is allowed in CI
// (--no-prompt), causing the CD pipeline to fail.
var cache = builder.AddRedis("cache");

var weather = builder.AddProject<Projects.PicnicPlanner_WeatherService>("weather-service");
var parks = builder.AddProject<Projects.PicnicPlanner_ParksService>("parks-service");

var coordinator = builder.AddProject<Projects.coordinatoragent>("coordinator-agent", launchProfileName: null)
    .WithHttpEndpoint(targetPort: 8088)
    .WithEnvironment("PORT", "8088")
    .WithEnvironment("AZURE_AI_PROJECT_ENDPOINT", builder.Configuration["AZURE_AI_PROJECT_ENDPOINT"])
    .WithEnvironment("AZURE_AI_MODEL_DEPLOYMENT_NAME", builder.Configuration["AZURE_AI_MODEL_DEPLOYMENT_NAME"])
    .WithReference(weather)
    .WithReference(parks)
    .WaitFor(weather)
    .WaitFor(parks);

var api = builder.AddProject<Projects.PicnicPlanner_Planner_Api>("planner-api")
    .WithReference(cache)
    .WithReference(weather)
    .WithReference(parks)
    .WaitFor(cache)
    .WaitFor(weather)
    .WaitFor(parks);

var weatherAgent = builder.AddProject<Projects.weatheragent>("weather-agent")
    .WithHttpEndpoint(targetPort: 8089)
    .WithEnvironment("PORT", "8089")
    .WithEnvironment("AZURE_AI_PROJECT_ENDPOINT", builder.Configuration["AZURE_AI_PROJECT_ENDPOINT"])
    .WithEnvironment("AZURE_AI_MODEL_DEPLOYMENT_NAME", builder.Configuration["AZURE_AI_MODEL_DEPLOYMENT_NAME"])
    .WithReference(weather)
    .WaitFor(weather);

var web = builder.AddViteApp("planner-web", "../../frontend")
    .WithReference(api)
    .WithReference(coordinator)
    .WaitFor(api)
    .WaitFor(coordinator);

builder.Build().Run();