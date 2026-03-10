using FinancialManager.Application.DTOs;
using FinancialManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(
        TransactionService transactionService,
        ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova transação (receita ou despesa)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
    {
        _logger.LogInformation("Criando transação para conta {AccountId}", request.AccountId);
        await _transactionService.ExecuteAsync(request);
        return Ok(new { message = "Transação registrada com sucesso!" });
    }
}