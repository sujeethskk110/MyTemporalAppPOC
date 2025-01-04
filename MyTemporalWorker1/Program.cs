using MyWorkFlows.Activities;
using MyWorkFlows.Workflows;
using Temporalio.Client;
using Temporalio.Worker;

Console.WriteLine("Starting Worker!.....");

var client = await TemporalClient.ConnectAsync(new()
{
    TargetHost = "localhost:7233",
    Namespace = "default"
});

MyActivities myActivities = new MyActivities();

var worker = new TemporalWorker(client, new TemporalWorkerOptions(taskQueue: "TaskQueue1")
    .AddWorkflow<TrackingWorkflow>()
    .AddActivity(myActivities.LoadTrackingNumbers)
    .AddActivity(myActivities.GetTrackingStatus)
    .AddActivity(myActivities.UpdateStatus));

await worker.ExecuteAsync(new CancellationToken());

Console.WriteLine("Worker Started!.....");