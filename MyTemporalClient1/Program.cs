using Microsoft.AspNetCore.Mvc;
using MyWorkFlows.Workflows;
using Temporalio.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole().SetMinimumLevel(LogLevel.Information);

builder.Services.AddSingleton(ctx =>
    TemporalClient.ConnectAsync(new()
    {
        TargetHost = "localhost:7233",
        Namespace = "default"
    }).Result); // Use .Result to get the TemporalClient instance synchronously

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", async context =>
{
    var client = context.RequestServices.GetRequiredService<TemporalClient>();
    await client.ExecuteWorkflowAsync(
        (TrackingWorkflow wf) => wf.RunAsync(),
        new(id: $"aspnet-sample-workflow-{Guid.NewGuid()}", taskQueue: "TaskQueue1"));
    await context.Response.WriteAsJsonAsync("Workflow Executed");
});

app.Run();
