using Bl.BLApi;
using Bl.BLServices;
using Dal.Api;
using Microsoft.Extensions.DependencyInjection;

namespace Bl
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<BlEichudInterface, BlEichudService>();
            services.AddScoped<BlPositionInterface, BlPositionService>();
            services.AddScoped<BlProjectInterface, BlProjectService>();
            services.AddScoped<BlSubProjectInterface, BlSubProjectService>();
            services.AddScoped<BlVolunteerDomainInterface, BlVolunteerDomainService>();
            services.AddScoped<BlVolunteeringInterface, BlVolunteeringService>();
            services.AddScoped<BlVolunteerInterface, BLVolunteerService>();

            return services;
        }
    }
}
