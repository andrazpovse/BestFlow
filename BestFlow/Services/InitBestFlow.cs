using BestFlow.Models;
using System.Text.Json;
using OrderType = BestFlow.Models.OrderType;

namespace BestFlow.Services;
public class InitBestFlow : IHostedService
{
    private readonly BestFlowService _bestFlowService;
    private readonly ILogger<InitBestFlow> _logger;
    public InitBestFlow(BestFlowService bestFlowservice, ILogger<InitBestFlow> logger)
    {
        _bestFlowService = bestFlowservice;
        _logger = logger;
    }

    /// <summary>
    /// Init venues and act as a console application, that runs on startup
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Init
        _bestFlowService.LoadVenues(["ob1.json", "ob2.json"]);

        // We want to sell 1 BTC
        var ourSide = OrderType.Sell;
        var quantity = 1.0f;
        var actions = _bestFlowService.CalculateBestFlow(quantity, ourSide);
        _logger.LogInformation("Perform {actions} for best {ourSide} of {quantity} BTC.",
            actions, ourSide, quantity);

        // We want to buy 1 BTC
        ourSide = OrderType.Buy;
        quantity = 1.0f;
        actions = _bestFlowService.CalculateBestFlow(quantity, ourSide);
        _logger.LogInformation("Perform {actions} for best {ourSide} of {quantity} BTC.",
            actions, ourSide, quantity);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
