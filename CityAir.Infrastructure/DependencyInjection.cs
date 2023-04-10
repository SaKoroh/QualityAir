using CityAir.Infrastructure.API;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CityAir.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddRefitClient<IOpenAQApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.openaq.org"));

            return services;
        }
    }
}
