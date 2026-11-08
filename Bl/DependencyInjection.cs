using Bl.Abstractions;
using Bl.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bl
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IEichudService, EichudService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ISubProjectService, SubProjectService>();
            services.AddScoped<IVolunteerDomainService, VolunteerDomainService>();
            services.AddScoped<IVolunteeringService, VolunteeringService>();
            services.AddScoped<IVolunteerService, VolunteerService>();
            services.AddScoped<IStatsService, StatsService>();

            return services;
        }
    }
}
