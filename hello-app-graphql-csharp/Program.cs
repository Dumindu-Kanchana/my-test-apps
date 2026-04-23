using HelloGraphQL.Schema;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AllowIntrospection(true);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting HelloGraphQL — Environment: {Environment}", app.Environment.EnvironmentName);
logger.LogDebug("Debug logging is active. Listening on http://0.0.0.0:8080");

app.MapGraphQL("/");

app.Run();
