using CityAir.Infrastructure.Models;

namespace CityAir.UI.Features.CityAirQuality
{
    public class GetCityAirQueryParam
    {
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 1;
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string CountryId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string OrderBy { get; set; }
        public string Entity { get; set; }


        public static implicit operator QueryParam(GetCityAirQueryParam cityAirQueryParam)
        {
            return new QueryParam
            {
                City = SplitStringToArray(cityAirQueryParam.City),
                OrderBy = cityAirQueryParam.OrderBy,
                CountryId = cityAirQueryParam.CountryId,
                Country = SplitStringToArray(cityAirQueryParam.Country),
                Entity = cityAirQueryParam.Entity,
                Limit = cityAirQueryParam.Limit,
                Page = cityAirQueryParam.Page,
                Offset = cityAirQueryParam.Offset,
                Sort = cityAirQueryParam.Sort
            };
        }

        private static string[] SplitStringToArray(string content)
        {
            return string.IsNullOrWhiteSpace(content) ?
                Array.Empty<string>() : content.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
    }
}
