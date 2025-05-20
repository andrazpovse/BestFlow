namespace BestFlow.Models;

public struct Action
{
    public string VenueName { get; set; }
    public float Quantity { get; set; }
    public float Price { get; set; }

    public override string ToString()
    {
        return $"{Quantity:F4} @ {Price:F4} on {VenueName}";
    }
}
