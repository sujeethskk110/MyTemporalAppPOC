namespace MyTemporalApp1.Workflow.Activity;

using Temporalio.Activities;

public class MyActivity
{
    [Activity]
    public Task<string> AppendNameAsync(string name)
    {
        return Task.FromResult($"{name} Smith");
    }
}
