using System.Text.Json.Serialization;

namespace BestFlow.Library.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderType
{
    Buy,
    Sell
}

public class Order
{
    public string Id { get; set; }
    public DateTime Time { get; set; }
    public OrderType Type { get; set; }
    public string Kind { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }

    // BBA compare
    public int CompareTo(Order other)
    {
        if (Type != other.Type)
        {
            throw new InvalidOperationException("Orders must have same sides");
        }

        if (Type == OrderType.Buy)
        {
            // We want highest price for Buy orders
            return other.Price.CompareTo(Price);

        }
        else // Sell
        {
            // We want lowest price for Sell Orders
            return Price.CompareTo(other.Price);

        }
    }
}
