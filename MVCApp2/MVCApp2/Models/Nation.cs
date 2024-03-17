using Newtonsoft.Json;

namespace MVCApp2.Models
{
    public class Nation
    {
        [JsonProperty("ID Nation")]
        public string Id_Nation { get; set; } = null!;

        [JsonProperty("Nation")]
        public string Nation_Name { get; set; } = null!;

        [JsonProperty("ID Year")]
        public int Id_Year { get; set; }

        public string Year { get; set; } = null!;

        public int Population { get; set; }

        [JsonProperty("Slug Nation")]
        public string Slug_Nation { get; set; } = null!;
    }
}
