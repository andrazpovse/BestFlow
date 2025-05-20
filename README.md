# Best Flow
Calculate best actions to take in order to Buy or Sell specified amount of BTC.
The code works on multiple venues, each venue has its own balance in EUR and BTC.

We construct a list of venues, where each venue tracks its own best bid/ask. We then iterate
over venues, to find the best bid/ask across all venues and make the trade.

# Test the API
Easiest way to test the API is through Docker container.
`docker build . -t bestflow`
`docker run -p 5555:80 bestflow`
`curl http://localhost:5555/api/v1/BestFlow?quantity=1&orderType=Buy`s
