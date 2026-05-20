var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var weather = builder.AddProject<Projects.PicnicPlanner_WeatherService>("weather-service");
var parks = builder.AddProject<Projects.PicnicPlanner_ParksService>("parks-service");

var coordinator = builder.AddProject<Projects.coordinatoragent>("coordinator-agent")
    .WithHttpEndpoint(targetPort: 8088)
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

var web = builder.AddViteApp("planner-web", "../../frontend")
    .WithReference(api)
    .WithReference(coordinator)
    .WaitFor(api)
    .WaitFor(coordinator);

builder.Build().Run();
