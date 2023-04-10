namespace CityAir.UI.Features.CityAirQuality
{
    public interface ICityAirViewModelFactory
    {
        Task<GetCityAirViewModel> Create(GetCityAirQueryParam queryParam, string currentUrl);
    }
}
