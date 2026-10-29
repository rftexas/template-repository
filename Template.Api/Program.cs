using OpenTelemetry.Trace;
using Template.Domain;
using Template.Core;
using Template.Core.Items;
using Microsoft.AspNetCore.Mvc;
using Template.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddTemplateServices()
    .AddPersistenceServices();

builder.Services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource(Observability.ActivitySource.Name)
        .AddOtlpExporter()
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/items/{itemId}", async (Guid itemId, [FromServices] IQueryBus bus) =>
{
    var result = await bus.Send<GetItem.ById, Template.Domain.Items.Item>(GetItem.ById.From(itemId));
    return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
});

app.MapPost("/items", async ([FromBody] AddItem.Command command, [FromServices] ICommandBus bus) =>
{
    var result = await bus.Send<AddItem.Command, Template.Domain.Items.Item>(command);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest();
});

app.UseHttpsRedirection();

app.Run();
