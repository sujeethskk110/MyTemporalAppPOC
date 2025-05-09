﻿using Microsoft.Extensions.Logging;
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


        var trackingStatus = await Workflow.ExecuteActivityAsync(
            (MyActivities myActivities) => myActivities.GetTrackingStatus(trackingNumbers),
            new()
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(1)
            });

        var status = await Workflow.ExecuteActivityAsync(
            (MyActivities myActivities) => myActivities.UpdateStatus(trackingStatus),
            new()
            {
                StartToCloseTimeout = TimeSpan.FromMinutes(1)
            });
        Workflow.Logger.LogInformation("Workflow Completed with Status : {Status}.....w", status);
    }
}