using BestFlow.Library.Models;
using Xunit;

namespace Tests
{
    public class VenueTests
    {
        [Fact]
        public void TestPerformTrade()
        {
            var bid = new Order
            {
                Amount = 1,
                Kind = "Limit",
                Price = 50,
                Type = OrderType.Buy
            };

            var ask = new Order
            {
                Amount = 1,
                Kind = "Limit",
                Price = 100,
                Type = OrderType.Sell
            };

            var venue = new Venue([bid], [ask], "Venue1", 1, 50);

            Assert.Equal(100, venue.BestAsk.Price);
            Assert.Equal(50, venue.BestBid.Price);

            var tradedAmount = venue.PerformTrade(OrderType.Sell, 1);
            Assert.Equal(1, tradedAmount);

            // Only 0.5 matched due to lack of balance
            tradedAmount = venue.PerformTrade(OrderType.Buy, 1);
            Assert.Equal(0.5m, tradedAmount);
        }
    }
}