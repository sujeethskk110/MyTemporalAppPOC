using Temporalio.Client;
using Temporalio.Worker;
using Microsoft.AspNetCore.Mvc;
using MyTemporalApp1.Workflow;
using MyTemporalApp1.Workflow.Activity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(ctx =>
    TemporalClient.ConnectAsync(new()
    {
        TargetHost = "localhost:7233",
        Namespace = "default"
    }));
var activity = new MyActivity();

builder.Services.AddSingleton<TemporalWorker>(ctx =>
{
    var client = ctx.GetRequiredService<Task<TemporalClient>>().Result;
    var temporalWorkerOptions = new TemporalWorkerOptions("asp-net-sample")
    .AddWorkflow<MyWorkflow>()
    .AddActivity(activity.AppendNameAsync);
    return new TemporalWorker(client, temporalWorkerOptions);
});

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
app.MapGet("/", async ([FromServices] Task<TemporalClient> clientTask, string? name) =>
{
    var client = await clientTask;
    return await client.ExecuteWorkflowAsync(
        (MyWorkflow wf) => wf.RunAsync(name ?? "Temporal"),
        new(id: $"aspnet-sample-workflow-{Guid.NewGuid()}", taskQueue: MyWorkflow.TaskQueue));
});
var worker = app.Services.GetRequiredService<TemporalWorker>();
Task task = Task.Run(() => worker.ExecuteAsync(new CancellationToken()));

app.Run();
