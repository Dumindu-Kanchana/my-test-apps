using HelloGraphQL.Schema;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .ModifyOptions(o => o.EnableSchemaIntrospection = true);

var app = builder.Build();

app.MapGraphQL("/");

app.Run();
