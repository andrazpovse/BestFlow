using BestFlow.Library.Models;
using BestFlow.Library.Services;

if (args.Length != 2)
{
    Console.WriteLine("Usage for Buy/Sell BTC: BestFlow.Console.exe [Buy|Sell] [quantity(decimal)]");
    return;
}

if (!Enum.TryParse<OrderType>(args[0], ignoreCase: true, out var ourOrderType))
{
    Console.WriteLine("Invalid type. Must be 'Buy' or 'Sell'.");
    return;
}

if (!decimal.TryParse(args[1], out decimal quantity))
{
    Console.WriteLine("Invalid quantity. Must be a decimal.");
    return;
}

// Init
var bestFlowService = new BestFlowService();
bestFlowService.LoadVenues(["ob1.json", "ob2.json"]);

// Logic
var actions = bestFlowService.CalculateBestFlow(quantity, ourOrderType);

Console.WriteLine($"Perform the following " +
    $"actions for best flow of {ourOrderType} {quantity} BTC");
foreach (var action in actions)
{
    Console.WriteLine(action);
}