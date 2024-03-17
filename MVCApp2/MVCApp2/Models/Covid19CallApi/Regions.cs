using Newtonsoft.Json;

namespace MVCApp2.Models.Covid19CallApi
{
    public class Regions
    {
        public string iso { get; set; } = string.Empty;

        public string name { get; set; } = string.Empty;

        public string province { get; set; } = string.Empty;

        public string lat { get; set; } = string.Empty;

        [JsonProperty("long")]
        public string longitude { get; set; } = string.Empty;

        //public Cities cities { get; set; } = string.Empty;
    }
}
