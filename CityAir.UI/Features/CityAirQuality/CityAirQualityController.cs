using Microsoft.AspNetCore.Mvc;
using Razor.Templating.Core;

namespace CityAir.UI.Features.CityAirQuality
{
    public class CityAirQualityController : Controller
    {
        private readonly ICityAirViewModelFactory _viewModelFactory;

        public CityAirQualityController(ICityAirViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }


        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Index(GetCityAirQueryParam queryParam)
        {
            var viewModel = await _viewModelFactory.Create(queryParam);

            return View("/Features/CityAirQuality/Index.cshtml", viewModel);
        }

        [Route("/loadmore")]
        [HttpPost]
        public async Task<IActionResult> LoadMore([FromBody] GetCityAirQueryParam queryParam)
        {
            var viewModel = await _viewModelFactory.Create(queryParam);
            var html = await RazorTemplateEngine.RenderAsync("/Features/CityAirQuality/CitiesPartial.cshtml", viewModel);

            return Json(new { data = html });
        }
    }
}