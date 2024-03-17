using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using MVCApp2.Models;

namespace MVCApp2.Controllers
{
    [Route("[Controller]")]
    public class NationController : Controller
    {
        [HttpGet]
        [Route("/Nation/Index")]
        public async Task<ActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "https://datausa.io/api/data?drilldowns=Nation&measures=Population";

                    //string apiUrl = "https://datausa.io/api/data?drilldowns=Nation&measures=Population&year=latest";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        NationObject nationObject = JsonConvert.DeserializeObject<NationObject>(responseBody)!;

                        List<Nation> nationList = nationObject.Data;

                        //Nation item = (from nation in nationList where nation.Year == "2016" select nation).FirstOrDefault()!;

                        return View(nationList);
                    }
                    else
                    {
                        return Content($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    return Content($"Error: {ex.Message}");
                }

            }
        }
    }
}
