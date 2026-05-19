var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var api = builder.AddProject<Projects.PicnicPlanner_Planner_Api>("planner-api")
    .WithReference(cache)
    .WaitFor(cache);

var web = builder.AddViteApp("planner-web", "../frontend")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
