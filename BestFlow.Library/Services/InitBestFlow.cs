using Microsoft.Extensions.Hosting;

namespace BestFlow.Library.Services;
public class InitBestFlow : IHostedService
{
    private readonly BestFlowService _bestFlowService;

    public InitBestFlow(BestFlowService bestFlowservice)
    {
        _bestFlowService = bestFlowservice;
    }

    /// <summary>
    /// Init by loading venue data
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Init
        _bestFlowService.LoadVenues(["ob1.json", "ob2.json"]);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
