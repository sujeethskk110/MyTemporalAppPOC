namespace MyTemporalApp1.Workflow;

using MyTemporalApp1.Workflow.Activity;
using Temporalio.Workflows;

[Workflow]
public class MyWorkflow
{
    public const string TaskQueue = "asp-net-sample";

    [WorkflowRun]
    public async Task<string> RunAsync(string name)
    {
        var appendedName = await Workflow.ExecuteActivityAsync(
            (MyActivity act) => act.AppendNameAsync(name),
            new ActivityOptions { TaskQueue = TaskQueue, ScheduleToCloseTimeout = TimeSpan.FromSeconds(5) });
        return $"Hello, {appendedName}!";
    }
}
