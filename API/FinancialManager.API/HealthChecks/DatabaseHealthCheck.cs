using FinancialManager.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FinancialManager.API.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly FinancialManagerDbContext _dbContext;

    public DatabaseHealthCheck(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Database.CanConnectAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database connection is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Database connection failed",
                exception: ex);
        }
    }
}
