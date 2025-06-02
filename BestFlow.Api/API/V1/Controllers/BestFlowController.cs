using BestFlow.Library.Models;
using BestFlow.Library.Services;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
namespace BestFlow.Api.API.V1.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{v:apiVersion}")]
public class BestFlowController : ControllerBase
{
    private readonly BestFlowService _bestFlowService;
    private readonly ILogger _logger;
    public BestFlowController(BestFlowService bestFlowService, ILogger logger)
    {
        _bestFlowService = bestFlowService;
        _logger = logger;
    }
    [HttpGet]
    [Route("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BestFlow(decimal quantity,
        OrderType orderType,
        CancellationToken cancellationToken)
    {
        try
        {
            var actions = _bestFlowService.CalculateBestFlow(quantity, orderType);

            return Ok(actions);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "BestFlow call failed.");
            return StatusCode(500);
        }
    }
}