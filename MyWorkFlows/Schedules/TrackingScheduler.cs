using MyWorkFlows.Workflows;
using Temporalio.Client.Schedules;
using Temporalio.Client;
using Temporalio.Api.Enums.V1;

namespace MyWorkFlows.Schedules;

public class TrackingScheduler 
{
    public static async Task ScheduleStartAsync(TemporalClient client)
    {
        Console.WriteLine("Scheduling workflow");

        var action = ScheduleActionStartWorkflow.Create<TrackingWorkflow>(wf => wf.RunAsync(), new()
        {
            Id = "schedule-workflow-id",
            TaskQueue = "TaskQueue1", // Use the task queue for scheduling
        });

        var spec = new ScheduleSpec
        {
            Intervals = new List<ScheduleIntervalSpec>
        {
            new(Every: TimeSpan.FromSeconds(60)), 
        },
        };

        var schedule = new Schedule(action, spec)
        {
            Policy = new()
            {
                CatchupWindow = TimeSpan.FromDays(1), // Define the catch-up window in case workflows are missed
                Overlap = ScheduleOverlapPolicy.AllowAll, // Allow overlapping workflows if needed
            },
        };

        // Here we create the schedule
        await client.CreateScheduleAsync("Schedule 1", schedule);
        Console.WriteLine("Workflow scheduled successfully");
    }


    public static async Task DeletescheduleAsync(TemporalClient client)
    {
        var handle = client.GetScheduleHandle("Schedule 1");
        await handle.DeleteAsync();
        Console.WriteLine("Schedule is now deleted.");
    }

    public static async Task PausescheduleAsync(TemporalClient client)
    {
        var handle = client.GetScheduleHandle("Schedule 1");
        await handle.PauseAsync();
        Console.WriteLine("Schedule is now deleted.");
    }

    public static async Task UnPausescheduleAsync(TemporalClient client)
    {
        var handle = client.GetScheduleHandle("Schedule 1");
        await handle.UnpauseAsync();
        Console.WriteLine("Schedule is now deleted.");
    }

    public static async Task ScheduleGoFasterAsync(TemporalClient client)
    {
        var handle = client.GetScheduleHandle("Schedule 1");

        await handle.UpdateAsync(input =>
        {
            var spec = new ScheduleSpec
            {
                Intervals = new List<ScheduleIntervalSpec>
            {
                new(Every: TimeSpan.FromMinutes(5)),
            },
            };
            var schedule = input.Description.Schedule with { Spec = spec };
            return new ScheduleUpdate(schedule);
        });

        Console.WriteLine("Schedule is now triggered every 5 minutes.");
    }
}
