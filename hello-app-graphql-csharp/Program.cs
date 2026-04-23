using HelloGraphQL.Schema;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL("/").WithOptions(new HotChocolate.AspNetCore.GraphQLServerOptions
{
    EnableSchemaIntrospection = true
});

app.Run();
