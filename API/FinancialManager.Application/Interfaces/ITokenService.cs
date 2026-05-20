using FinancialManager.Domain.Entities;

namespace FinancialManager.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
