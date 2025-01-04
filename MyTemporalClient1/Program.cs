using MyWorkFlows.Workflows;
using Temporalio.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole().SetMinimumLevel(LogLevel.Information);

var client = await TemporalClient.ConnectAsync(new("localhost:7233")
{
    LoggerFactory = LoggerFactory.Create(builder =>
        builder.AddSimpleConsole(options => options.TimestampFormat = "[HH:mm:ss] ")
               .SetMinimumLevel(LogLevel.Information)),
});

builder.Services.AddSingleton(ctx =>client);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

//await TrackingScheduler.ScheduleStartAsync(client);
//await TrackingScheduler.PausescheduleAsync(client);
//await TrackingScheduler.UnPausescheduleAsync(client);
//await TrackingScheduler.ScheduleGoFasterAsync(client);
//await TrackingScheduler.DeletescheduleAsync(client);

app.Run();
