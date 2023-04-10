using CityAir.Infrastructure.API;
using CityAir.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace CityAir.UI.Features.CityAirQuality
{
    public class CityAirViewModelFactory : ICityAirViewModelFactory
    {
        private readonly IOpenAQApi _openAQApi;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CityAirViewModelFactory> _logger;

        public CityAirViewModelFactory(IOpenAQApi openAQApi, IMemoryCache memoryCache, ILogger<CityAirViewModelFactory> logger)
        {
            _openAQApi = openAQApi ?? throw new ArgumentNullException(nameof(openAQApi));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<GetCityAirViewModel> Create(GetCityAirQueryParam queryParam)
        {
            var result = InitializeModel(queryParam);
            var cachKey = GetKey(queryParam);
            try
            {
                if (_memoryCache.TryGetValue(cachKey, out GetCityAirViewModel cachedResponse))
                {
                    return cachedResponse;
                }

                var response = await _openAQApi.GetCities(queryParam);

                _logger.LogInformation($"Request processed successful for {cachKey}", response);

                result.Results.AddRange(response.Results);

                _memoryCache.Set(cachKey, result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return result;
            }
        }

        private GetCityAirViewModel InitializeModel(GetCityAirQueryParam queryParam)
        {
            return new GetCityAirViewModel
            {
                QueryParam = queryParam,
                OrderBy = GetOrderBySelectListItems(queryParam.OrderBy),
                SortBy = GetSortBySelectListItems(queryParam.Sort),
                Results = new List<GetCityResult>()
            };
        }

        private List<SelectListItem> GetOrderBySelectListItems(string orderBy)
        {
            var orderByList = new string[] { "city", "country", "firstUpdated", "lastUpdated", "lastUpdated" };
            return GetelectListItems(orderByList, orderBy);
        }

        private List<SelectListItem> GetSortBySelectListItems(string sortBy)
        {
            return GetelectListItems(new string[] { "asc", "desc" }, sortBy);
        }

        private List<SelectListItem> GetelectListItems(string[] items, string selectedText)
        {
            if (string.IsNullOrWhiteSpace(selectedText))
                selectedText = items[0];

            return items.Select(item => new SelectListItem
            {
                Text = item,
                Value = item,
                Selected = item.Equals(selectedText, StringComparison.InvariantCultureIgnoreCase)
            }).ToList();
        }

        private string GetKey(GetCityAirQueryParam queryParam)
        {
            //.OrderBy=city&QueryParam.CountryId=&QueryParam.Entity=&QueryParam.Country=&QueryParam.City=
            return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}", queryParam.Page, queryParam.Offset, 
                queryParam.Limit, queryParam.Sort, 
                queryParam.OrderBy, 
                queryParam.CountryId, 
                queryParam.Entity, 
                queryParam.City, 
                queryParam.Country);
        }
    }
}
