using Newtonsoft.Json;

namespace MVCApp2.Models.Covid19CallApi
{
    public class Top10Regions
    {
        public string Region { get; set; } = string.Empty;

        public int Cases { get; set; }

        public int Deaths { get; set; }

    }
}
