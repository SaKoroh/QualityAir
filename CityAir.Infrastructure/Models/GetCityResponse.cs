using Newtonsoft.Json;

namespace CityAir.Infrastructure.Models
{
    public class GetCityResponse
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("results")]
        public List<GetCityResult> Results { get; set; }
    }

    public class Meta
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("license")]
        public string License { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("found")]
        public int Found { get; set; }
    }

    public class GetCityResult
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("locations")]
        public int Locations { get; set; }

        [JsonProperty("firstUpdated")]
        public DateTime FirstUpdated { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("parameters")]
        public List<string> Parameters { get; set; }
    }


}
