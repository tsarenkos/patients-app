using Microsoft.Extensions.Diagnostics.HealthChecks;
using PatientsProject.Infrastructure;

namespace PatientsProject.Api
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseHealthCheck(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                bool canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
                return canConnect ? HealthCheckResult.Healthy("Database is reachable")
                                  : HealthCheckResult.Unhealthy("Database connection failed");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database connection threw an exception", ex);
            }
        }
    }
}
