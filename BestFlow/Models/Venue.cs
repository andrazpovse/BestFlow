namespace BestFlow.Models;

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
    private float _btcBalance { get; set; } = 0;
    public float _eurBalance { get; set; } = 0;

    public Venue(IEnumerable<Order> bids, IEnumerable<Order> asks,
        string name, float btcBalance, float eurBalance)
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
    public float PerformTrade(OrderType type, float amount)
    {
        // We are selling BTC
        if (type == OrderType.Sell)
        {
            var tradedAmount = Math.Min(amount, Math.Min(BestBid.Amount, _btcBalance));
            
            if (IsEqual(tradedAmount, BestBid.Amount))
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

            if (IsEqual(tradedAmount, BestAsk.Amount))
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

    private bool IsEqual(float a, float b)
    {
        return Math.Abs(a - b) < 0.0000000001f;
    }
}
