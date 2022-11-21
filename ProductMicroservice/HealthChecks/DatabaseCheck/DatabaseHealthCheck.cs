using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;

namespace AuthenticationMicroservice.HealthChecks.DatabaseCheck
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;
        [ExcludeFromCodeCoverage]
        public DatabaseHealthCheck(IConfiguration configuration) => _configuration = configuration;

        [ExcludeFromCodeCoverage]
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
                                                        CancellationToken cancellationToken = default)
        {
            var isHealthy = HealthCheckResult.Healthy();

            try
            {
                var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

                connection.Open();

                isHealthy = HealthCheckResult.Healthy("Database connection is OK");
            }
            catch (Exception)
            {
                isHealthy = HealthCheckResult.Unhealthy("Database connection ERROR");
            }

            return Task.FromResult(isHealthy);
        }
    }
}
