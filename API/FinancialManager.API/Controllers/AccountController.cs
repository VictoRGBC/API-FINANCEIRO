using FinancialManager.Application.DTOs;
using FinancialManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountAppService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(AccountAppService accountService, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova conta
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        _logger.LogInformation("Requisição para criar conta: {Name}", request.Name);
        var response = await _accountService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Busca uma conta por ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var account = await _accountService.GetByIdAsync(id);
        if (account == null)
            return NotFound(new { message = $"Conta com ID '{id}' não encontrada." });

        return Ok(account);
    }

    /// <summary>
    /// Busca todas as contas de um usuário
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var accounts = await _accountService.GetByUserIdAsync(userId);
        return Ok(accounts);
    }
}
