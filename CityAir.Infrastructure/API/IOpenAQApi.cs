using CityAir.Infrastructure.Models;
using Refit;

namespace CityAir.Infrastructure.API
{
    public interface IOpenAQApi
    {
        [Get("/v2/cities")]
        Task<GetCityResponse> GetCities(QueryParam queryParam);
    }
}
