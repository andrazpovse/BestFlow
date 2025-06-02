namespace BestFlow.Library.Models;

public struct Action
{
    public string VenueName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"{Quantity:F4} @ {Price:F4} on {VenueName}";
    }
}
