using Microsoft.AspNetCore.Mvc;
using MVCApp2.Models.Covid19CallApi;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System;
using System.Text;
//using System.Xml.Serialization;

namespace MVCApp2.Controllers
{
    public class RegionsController : Controller
    {
        [HttpGet]
        [Route("Regions/RegionsIndex")]
        public async Task<ActionResult> RegionsIndex(Regions selectedRegion)
        {
            IEnumerable<Top10Regions> reportsList = await getTop10RegionsWithCases();
            ViewData["reportsListViewData"] = reportsList;

            ViewData["provincesListViewData"] = new List<Top10Provinces>();
            if (selectedRegion.name != "")
            {
                IEnumerable<Top10Provinces> provincesList = await getTop10ProvincesWithCases(selectedRegion);
                ViewData["provincesListViewData"] = provincesList;
                //ViewData["regionName"] = selectedRegion.name;
            }

            IEnumerable<string> regionNames = await getRegionNames();
            ViewData["regionNamesViewData"] = regionNames;

            return View(selectedRegion);
        }

        [HttpPost]
        [Route("Regions/RegionsIndex")]
        public async Task<ActionResult> RegionsIndex(Regions selectedRegion, int param)
        {
            IEnumerable<Top10Regions> reportsList = await getTop10RegionsWithCases();
            ViewData["reportsListViewData"] = reportsList;

            if (selectedRegion.name != "")
            {
                IEnumerable<Top10Provinces> provincesList = await getTop10ProvincesWithCases(selectedRegion);
                ViewData["provincesListViewData"] = provincesList;
            }

            IEnumerable<string> regionNames = await getRegionNames();
            ViewData["regionNamesViewData"] = regionNames;

            return RedirectToAction("RegionsIndex", selectedRegion);
        }


        private async Task<IEnumerable<Reports>> getReports()
        {
            string apiUrl = "https://covid-19-statistics.p.rapidapi.com/reports";
            IEnumerable<Reports> reportsList = [];

            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "26d5281c7fmsh99580840d40e2c7p130e9djsn03159cab280f");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "covid-19-statistics.p.rapidapi.com");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                string responseBody = await response.Content.ReadAsStringAsync();

                reportsList = JsonConvert.DeserializeObject<ReportsData>(responseBody)!.data;

                //ReportsData reportsData = JsonConvert.DeserializeObject<ReportsData>(responseBody)!;
                //reportsList = reportsData.data;
            }

            return reportsList;
        }

        private async Task<IEnumerable<Top10Regions>> getTop10RegionsWithCases()
        {
            IEnumerable<Top10Regions> reportsList = [];

            reportsList = (await getReports())
                .GroupBy(item => item.region.name)
                .Select(item => new Top10Regions
                {
                    Region = item.First().region.name,
                    Cases = item.Sum(item => item.confirmed),
                    Deaths = item.Sum(item => item.deaths)
                })
                .OrderByDescending(item => item.Cases)
                .Take(10);

                //(from reports in await getReports()
                //group reports by reports.region.name into groupedReports
                //select new Top10Regions
                //{
                //    Region = groupedReports.First().region.name,
                //    Cases = groupedReports.Sum(x => x.confirmed),
                //    Deaths = groupedReports.Sum(x => x.deaths)
                //} into groupedAndSummedReports
                //orderby groupedAndSummedReports.Cases descending
                //select groupedAndSummedReports
                //)
                //.Take(10);

            return reportsList;
        }

        //private async Task<IEnumerable<Reports>> getTop10RegionsWithCases()
        //{
        //    IEnumerable<Reports> reportsList = [];

        //    reportsList = (from reports in await getReports()
        //                   group reports by reports.region.name into groupedReports
        //                   select new Reports
        //                   {
        //                       date = "",
        //                       confirmed = groupedReports.Sum(x => x.confirmed),
        //                       deaths = groupedReports.Sum(x => x.deaths),
        //                       recovered = 0,
        //                       confirmed_diff = 0,
        //                       deaths_diff = 0,
        //                       recovered_diff = 0,
        //                       last_update = "",
        //                       active = 0,
        //                       active_diff = 0,
        //                       fatality_rate = 0,
        //                       region = groupedReports.First().region
        //                   } into groupedAndSummedReports
        //                   orderby groupedAndSummedReports.confirmed descending
        //                   select groupedAndSummedReports
        //                   )
        //                   .Take(10);

        //    return reportsList;
        //}

        private async Task<IEnumerable<Top10Provinces>> getTop10ProvincesWithCases(Regions selectedRegion)
        {
            IEnumerable<Top10Provinces> reportsList = [];

            reportsList = (await getReports())
                .Where(item => item.region.name == selectedRegion.name)
                .OrderByDescending(item => item.confirmed)
                .Take(10)
                .Select(item => new Top10Provinces
                {
                    Province = item.region.province,
                    Cases = item.confirmed,
                    Deaths = item.deaths
                });

            return reportsList;
        }

        //private async Task<IEnumerable<Reports>> getTop10ProvincesWithCases(Regions selectedRegion)
        //{
        //    IEnumerable<Reports> reportsList = [];

        //    reportsList = (from reports in await getReports()
        //                   where reports.region.name == selectedRegion.name
        //                   orderby reports.confirmed descending
        //                   select new Reports
        //                   {
        //                       date = "",
        //                       confirmed = reports.confirmed,
        //                       deaths = reports.deaths,
        //                       recovered = 0,
        //                       confirmed_diff = 0,
        //                       deaths_diff = 0,
        //                       recovered_diff = 0,
        //                       last_update = "",
        //                       active = 0,
        //                       active_diff = 0,
        //                       fatality_rate = 0,
        //                       region = reports.region
        //                   })
        //       .Take(10);

        //    return reportsList;
        //}

        private async Task<IEnumerable<string>> getRegionNames()
        {
            string apiUrl = "https://covid-19-statistics.p.rapidapi.com/regions";
            IEnumerable<string> regionNames = [];

            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "26d5281c7fmsh99580840d40e2c7p130e9djsn03159cab280f");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "covid-19-statistics.p.rapidapi.com");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                string responseBody = await response.Content.ReadAsStringAsync();

                RegionsIsoNameData regionsIsoNameData = JsonConvert.DeserializeObject<RegionsIsoNameData>(responseBody)!;

                IEnumerable<RegionsIsoName> regionsIsoNameList = regionsIsoNameData.data;

                regionNames = regionsIsoNameList.Select(x => x.name);

            }

            return regionNames;
        }

        public async Task<ActionResult> exportTop10RegionsToXml()
        {
            List<Top10Regions> top10Regions = (await getTop10RegionsWithCases()).ToList();

            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(top10Regions.GetType());

            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, top10Regions);
            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(stringWriter.ToString());
            return File(byteArr, "application/xml", "Top10Regions.xml");
        }

        public async Task<ActionResult> exportTop10RegionsToJson()
        {
            List<Top10Regions> top10Regions = (await getTop10RegionsWithCases()).ToList();

            string jsonString = System.Text.Json.JsonSerializer.Serialize(top10Regions);
            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(jsonString);
            return File(byteArr, "application/json", "Top10Regions.json");

        }

        public async Task<ActionResult> exportTop10RegionsToCsv()
        {
            List<Top10Regions> top10Regions = (await getTop10RegionsWithCases()).ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Region,Cases,Deaths");

            foreach (var item in top10Regions)
            {
                stringBuilder.AppendLine($"{item.Region},{item.Cases},{item.Deaths}");
            }

            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
            return File(byteArr, "text/csv", "Top10Regions.csv");
        }

        public async Task<ActionResult> exportTop10ProvincesToXml(Regions selectedRegion)
        {
            List<Top10Provinces> top10Provinces = (await getTop10ProvincesWithCases(selectedRegion)).ToList();

            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(top10Provinces.GetType());

            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, top10Provinces);
            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(stringWriter.ToString());
            return File(byteArr, "application/xml", "Top10Provinces.xml");
        }

        public async Task<ActionResult> exportTop10ProvincesToJson(Regions selectedRegion)
        {
            List<Top10Provinces> top10Provinces = (await getTop10ProvincesWithCases(selectedRegion)).ToList();

            string jsonString = System.Text.Json.JsonSerializer.Serialize(top10Provinces);
            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(jsonString);
            return File(byteArr, "application/json", "Top10Provinces.json");
        }

        public async Task<ActionResult> exportTop10ProvincesToCsv(Regions selectedRegion)
        {
            List<Top10Provinces> top10Provinces = (await getTop10ProvincesWithCases(selectedRegion)).ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Province,Cases,Deaths");

            foreach (var item in top10Provinces)
            {
                stringBuilder.AppendLine($"{item.Province},{item.Cases},{item.Deaths}");
            }

            byte[] byteArr = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
            return File(byteArr, "text/csv", "Top10Provinces.csv");
        }
    }
}
