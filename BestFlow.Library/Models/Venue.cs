namespace BestFlow.Library.Models;

public class Venue
{
    public string Name { get; set; }

    // Locking
    public Order? BestBid => _bids.Count != 0 ? _bids.Last() : null;
    // Locking
    public Order? BestAsk => _asks.Count != 0 ? _asks.Last() : null;
    public bool HasRemainingBtcBalance => _btcBalance > 0;
    public bool HasRemainingEurBalance => _eurBalance > 0;

    // Sorted
    private List<Order> _bids { get; set; }
    // Sorted
    private List<Order> _asks { get; set; }
    private decimal _btcBalance { get; set; } = 0m;
    public decimal _eurBalance { get; set; } = 0m;

    public Venue(IEnumerable<Order> bids, IEnumerable<Order> asks,
        string name, decimal btcBalance, decimal eurBalance)
    {
        Name = name;
        _btcBalance = btcBalance;
        _eurBalance = eurBalance;
        // Use this order for O(1) operations
        _bids = bids
            .OrderBy(o => o.Price)
            .ToList();
        _asks = asks
            .OrderByDescending(o => o.Price)
            .ToList();
    }

    /// <summary>
    /// Returns quantity matched in this trade. Would need locking.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public decimal PerformTrade(OrderType type, decimal amount)
    {
        // We are selling BTC
        if (type == OrderType.Sell)
        {
            var tradedAmount = Math.Min(amount, Math.Min(BestBid.Amount, _btcBalance));

            if (tradedAmount == BestBid.Amount)
            {
                _bids.RemoveAt(_bids.Count - 1);
            }
            else
            {
                _bids[_bids.Count - 1].Amount -= tradedAmount;
            }

            _btcBalance -= tradedAmount;

            return tradedAmount;
        }
        else // We are buying BTC
        {
            var price = BestAsk.Price;
            var tradedAmount = Math.Min(amount,
                Math.Min(BestAsk.Amount, _eurBalance / BestAsk.Price));

            if (tradedAmount == BestAsk.Amount)
            {
                _asks.RemoveAt(_asks.Count - 1);
            }
            else
            {
                _asks[_asks.Count - 1].Amount -= tradedAmount;
            }

            _eurBalance -= tradedAmount * price;

            return tradedAmount;
        }
    }
}
