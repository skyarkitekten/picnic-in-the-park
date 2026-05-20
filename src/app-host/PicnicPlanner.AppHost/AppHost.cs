var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var weather = builder.AddProject<Projects.PicnicPlanner_WeatherService>("weather-service");
var parks = builder.AddProject<Projects.PicnicPlanner_ParksService>("parks-service");

var api = builder.AddProject<Projects.PicnicPlanner_Planner_Api>("planner-api")
    .WithReference(cache)
    .WithReference(weather)
    .WithReference(parks)
    .WaitFor(cache)
    .WaitFor(weather)
    .WaitFor(parks);

var web = builder.AddViteApp("planner-web", "../../frontend")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
