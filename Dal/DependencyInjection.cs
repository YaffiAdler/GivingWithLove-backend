using Dal.Api;
using Dal.Models;
using Dal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dal
{
    public static class DependencyInjection
    {
        private const string ConnectionStringName = "DefaultConnection";

        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString(ConnectionStringName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{ConnectionStringName}' is not configured. " +
                    $"For local development run: dotnet user-secrets set \"ConnectionStrings:{ConnectionStringName}\" \"<connection string>\" --project Web");
            }

            services.AddDbContext<DbManager>(options => options.UseSqlServer(connectionString));

            services.AddScoped<DalEichudInterface, DalEichudService>();
            services.AddScoped<DalPositionInterface, DalPositionService>();
            services.AddScoped<DalProjectInterface, DalProjectService>();
            services.AddScoped<DalSubProjectInterface, DalSubProjectService>();
            services.AddScoped<DalVolunteerDomainInterface, DalVolunteerDomainService>();
            services.AddScoped<DalVolunteeringInterface, DalVolunteeringService>();
            services.AddScoped<DalVolunteerInterface, DalVolunteerService>();

            return services;
        }
    }
}
