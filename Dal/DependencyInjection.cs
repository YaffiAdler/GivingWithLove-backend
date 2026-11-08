using Dal.Abstractions;
using Dal.Models;
using Dal.Queries;
using Dal.Repositories;
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

            services.AddScoped<IEichudRepository, EichudRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ISubProjectRepository, SubProjectRepository>();
            services.AddScoped<IVolunteerDomainRepository, VolunteerDomainRepository>();
            services.AddScoped<IVolunteeringRepository, VolunteeringRepository>();
            services.AddScoped<IVolunteerRepository, VolunteerRepository>();
            services.AddScoped<IStatsQueries, StatsQueries>();

            return services;
        }
    }
}
