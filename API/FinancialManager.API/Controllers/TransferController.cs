using FinancialManager.Application.DTOs;
using FinancialManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferController : ControllerBase
{
    private readonly TransferAppService _transferService;
    private readonly ILogger<TransferController> _logger;

    public TransferController(TransferAppService transferService, ILogger<TransferController> logger)
    {
        _transferService = transferService;
        _logger = logger;
    }

    /// <summary>
    /// Realiza uma transferência entre contas
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        _logger.LogInformation(
            "Requisição de transferência de {Amount:C} da conta {SourceId} para {DestinationId}",
            request.Amount, request.SourceAccountId, request.DestinationAccountId);

        await _transferService.ExecuteAsync(request);

        return Ok(new
        {
            message = "Transferência realizada com sucesso!",
            sourceAccountId = request.SourceAccountId,
            destinationAccountId = request.DestinationAccountId,
            amount = request.Amount
        });
    }
}
