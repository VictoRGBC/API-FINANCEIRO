using FinancialManager.Application.DTOs;
using FinancialManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryAppService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(CategoryAppService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova categoria
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        _logger.LogInformation("Requisição para criar categoria: {Name}", request.Name);
        var response = await _categoryService.CreateAsync(request);
        return CreatedAtAction(nameof(GetByUserId), new { userId = response.UserId }, response);
    }

    /// <summary>
    /// Busca todas as categorias de um usuário
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var categories = await _categoryService.GetByUserIdAsync(userId);
        return Ok(categories);
    }
}
