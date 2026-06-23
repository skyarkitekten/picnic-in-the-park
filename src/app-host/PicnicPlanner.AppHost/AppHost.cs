var builder = DistributedApplication.CreateBuilder(args);

var cachePassword = builder.AddParameter("cache-password", secret: true);
var cache = builder.AddRedis("cache", password: cachePassword);

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