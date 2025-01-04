using Microsoft.Extensions.Logging;
using MyWorkFlows.Activities;
using Temporalio.Workflows;

namespace MyWorkFlows.Workflows;

[Workflow]
public class TrackingWorkflow
{
    [WorkflowRun]
    public async Task RunAsync()
    {
        Workflow.Logger.LogInformation("Workflow Started....W");

        var trackingNumbers = await Workflow.ExecuteActivityAsync(
            (MyActivities myActivities) => myActivities.LoadTrackingNumbers(),
            new(){
                StartToCloseTimeout = TimeSpan.FromMinutes(1)
            });
            //await myActivities.LoadTrackingNumbers();


        var trackingStatus = await Workflow.ExecuteActivityAsync(
            (MyActivities myActivities) => myActivities.GetTrackingStatus(trackingNumbers),
            new()
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(1)
            });
        //await myActivities.GetTrackingStatus(trackingNumbers);

        var status = await Workflow.ExecuteActivityAsync(
            (MyActivities myActivities) => myActivities.UpdateStatus(trackingStatus),
            new()
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(1)
            });
        //await myActivities.UpdateStatus(trackingStatus);
        Workflow.Logger.LogInformation("Workflow Completed with Status : {Status}.....w", status);
    }
}