using BestFlow.Library.Models;
using BestFlow.Library.Services;
using Xunit;

namespace Tests
{
    public class BestFlowTests : IDisposable
    {
        private readonly BestFlowService _bestFlowService;

        public BestFlowTests()
        {
            _bestFlowService = new BestFlowService();
        }

        public void Dispose()
        {
        }

        [Fact]
        public void TestSellingOverBalance()
        {
            _bestFlowService.LoadVenues(["test_ob1.json", "test_ob2.json"]);
            var actions = _bestFlowService.CalculateBestFlow(5, OrderType.Sell);

            Assert.Equal(2, actions.Sum(a => a.Quantity), precision: 4);
        }

        [Fact]
        public void TestBuyingOverBalance()
        {
            _bestFlowService.LoadVenues(["test_ob1.json", "test_ob2.json"]);
            var actions = _bestFlowService.CalculateBestFlow(5, OrderType.Buy);

            Assert.Equal(3.3727m, actions.Sum(a => a.Quantity), precision: 4);
        }

        [Fact]
        public void TestEmptyOrderBook()
        {
            var venue = new Venue([], [], "Venue", 1, 1000);
            _bestFlowService.LoadVenues(venues: [venue]);
            var actions = _bestFlowService.CalculateBestFlow(5, OrderType.Buy);

            Assert.Empty(actions);
        }

        [Fact]
        public void TestClearOrderBook()
        {
            var bid = GenerateOrder(1, 100, OrderType.Buy);
            var venue = new Venue([bid], [], "Venue", 2, 1000);
            _bestFlowService.LoadVenues(venues: [venue]);
            var actions = _bestFlowService.CalculateBestFlow(2, OrderType.Sell);

            Assert.Single(actions);
            Assert.Equal(1, actions.First().Quantity);
        }

        private Order GenerateOrder(
            decimal amount = 1m,
            decimal price = 100m,
            OrderType type = OrderType.Sell)
        {
            return new Order
            {
                Amount = amount,
                Price = price,
                Type = type
            };
        }
    }
}