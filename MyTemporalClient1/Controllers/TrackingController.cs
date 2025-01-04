using Microsoft.AspNetCore.Mvc;
using MyWorkFlows.Workflows;
using Temporalio.Client;

namespace MyTemporalClient1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackingController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TrackingController> _logger;
        private readonly TemporalClient _temporalClient;

        public TrackingController(ILogger<TrackingController> logger, TemporalClient temporalClient)
        {
            _logger = logger;
            _temporalClient = temporalClient;
        }

        [HttpGet(Name = "RunTracker")]
        public async Task Get()
        {
            await _temporalClient.ExecuteWorkflowAsync(
                (TrackingWorkflow wf) => wf.RunAsync(),
                new(id: $"aspnet-sample-workflow-{Guid.NewGuid()}", taskQueue: "TaskQueue1"));
        }
    }
}
