using Refit;

namespace CityAir.Infrastructure.Models
{
    public class QueryParam
    {
        [AliasAs("limit")]
        public int Limit { get; set; }


        [AliasAs("page")]
        public int Page { get; set; }

        [AliasAs("offset")]
        public int Offset { get; set; }

        [AliasAs("sort")]
        public string Sort { get; set; }

        [AliasAs("country_id")]
        public string CountryId { get; set; }

        [AliasAs("country")]
        [Query(CollectionFormat.Multi)]
        public string[] Country { get; set; }

        [AliasAs("city")]
        [Query(CollectionFormat.Multi)]
        public string[] City { get; set; }

        [AliasAs("order_by")]
        public string OrderBy { get; set; }

        [AliasAs("entity")]
        public string Entity { get; set; }
    }
}
