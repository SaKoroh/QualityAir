using CityAir.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CityAir.UI.Features.CityAirQuality
{
    public class GetCityAirViewModel
    {
        public List<SelectListItem> SortBy { get; set; }
        public List<SelectListItem> OrderBy { get; set; }
        public GetCityAirQueryParam QueryParam { get; set; }
        public List<GetCityResult> Items { get; set; }
    }
}
