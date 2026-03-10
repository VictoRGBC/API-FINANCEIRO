using FinancialManager.Application.DTOs;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialManager.Application.Services;

public class CategoryAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryAppService> _logger;

    public CategoryAppService(IUnitOfWork unitOfWork, ILogger<CategoryAppService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
    {
        _logger.LogInformation("Criando categoria {Name} para usuário {UserId}", request.Name, request.UserId);

        var category = new Category(request.Name, request.UserId, request.Icon, request.ParentCategoryId);

        await _unitOfWork.CategoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Categoria criada com sucesso. ID: {CategoryId}", category.Id);

        return new CategoryResponse(
            category.Id,
            category.Name,
            category.Icon,
            category.UserId,
            category.ParentCategoryId,
            category.CreatedAt
        );
    }

    public async Task<IEnumerable<CategoryResponse>> GetByUserIdAsync(Guid userId)
    {
        _logger.LogInformation("Buscando categorias do usuário {UserId}", userId);

        var categories = await _unitOfWork.CategoryRepository.GetByUserIdAsync(userId);

        return categories.Select(c => new CategoryResponse(
            c.Id,
            c.Name,
            c.Icon,
            c.UserId,
            c.ParentCategoryId,
            c.CreatedAt
        ));
    }
}
