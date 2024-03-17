namespace MVCApp2.Models.Covid19CallApi
{
    public class Reports
    {

        public string date { get; set; } = string.Empty;
        
        public int confirmed { get; set; }

        public int deaths { get; set; }

        public int recovered { get; set; }

        public int confirmed_diff { get; set; }

        public int deaths_diff { get; set; }

        public int recovered_diff { get; set; }

        public string last_update { get; set; } = string.Empty;

        public int active { get; set; }

        public int active_diff { get; set; }

        public float fatality_rate { get; set; }

        public Regions region { get; set; } = new Regions();
    }
}
