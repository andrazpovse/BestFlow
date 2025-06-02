using BestFlow.Library.Models;
using System.Text.Json;
using Action = BestFlow.Library.Models.Action;
using OrderType = BestFlow.Library.Models.OrderType;

namespace BestFlow.Library.Services;

public class BestFlowService
{
    private List<Venue> _venues = [];

    public BestFlowService()
    {

    }

    public void LoadVenues(List<string> files = null, List<Venue> venues = null)
    {
        if (venues != null)
        {
            _venues.AddRange(venues);
        }

        if (files != null)
        {
            var idx = 0;
            var btcQty = 1.0m;
            var eurQty = 5000.0m;
            foreach (var file in files)
            {
                string jsonFilePath = Path.Combine(AppContext.BaseDirectory, "data", file);
                string jsonString = File.ReadAllText(jsonFilePath);
                OrderBook ob = JsonSerializer.Deserialize<OrderBook>(jsonString);
                _venues.Add(new Venue(ob.Bids.Select(o => o.Order), ob.Asks.Select(o => o.Order),
                    $"Venue-{idx}", btcQty, eurQty));
            }
        }
    }

    public List<Action> CalculateBestFlow(decimal totalAmount, OrderType ourOrderType)
    {
        List<Action> bestFlow = new();
        while (totalAmount > 0)
        {
            // Go through all venues and find best offer
            Venue? bestVenue = null;
            foreach (var venue in _venues)
            {
                if (!HasBalance(ourOrderType, venue))
                {
                    continue;
                }

                var candidateOffer = GetBestOffer(ourOrderType, venue);
                if (candidateOffer == null)
                {
                    // Nothing on this side of OB
                    continue;
                }
                if (bestVenue == null)
                {
                    bestVenue = venue;
                }
                else if (candidateOffer.CompareTo(GetBestOffer(ourOrderType, bestVenue)) < 0)
                {
                    bestVenue = venue;
                }
            }

            if (bestVenue == null)
            {
                break;
            }

            var bestOffer = GetBestOffer(ourOrderType, bestVenue);
            if (bestOffer == null)
            {
                // OB is empty
                break;
            }
            var price = bestOffer.Price;
            var venueName = bestVenue.Name;
            var amount = bestVenue.PerformTrade(ourOrderType, totalAmount);
            bestFlow.Add(new Action
            {
                Price = price,
                Quantity = amount,
                VenueName = venueName,
            });
            totalAmount -= amount;
        }

        return bestFlow;
    }

    private Order? GetBestOffer(OrderType type, Venue venue)
    {
        // We are buying, get best asking price
        if (type == OrderType.Buy)
        {
            return venue.BestAsk;
        }

        return venue.BestBid;
    }

    private bool HasBalance(OrderType type, Venue venue)
    {
        if (type == OrderType.Buy)
        {
            return venue.HasRemainingEurBalance;
        }

        return venue.HasRemainingBtcBalance;
    }
}
