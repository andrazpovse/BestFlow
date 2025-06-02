namespace BestFlow.Library.Models;

public class OrderBookData
{
    public Order Order { get; set; }
}

public class OrderBook
{
    public DateTime AcqTime { get; set; }
    public List<OrderBookData> Bids { get; set; }
    public List<OrderBookData> Asks { get; set; }
}
