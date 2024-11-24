using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Linq;
using System.Xml.Linq;

namespace JAHttpFunctionApp
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        //async static Task<string> GetResponseString()
        async static Task<string> GetResponseString(string latitude, string longitude)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "my-user-agent-name");

            var response = await httpClient.GetAsync($"https://api.met.no/weatherapi/locationforecast/2.0/classic?lat={latitude}&lon={longitude}");
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }
        
        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            Task<string> result = GetResponseString(req.Query["lat"], req.Query["lon"]);
            var finalResult = result.Result;
            var rootElement = XElement.Parse(finalResult);
            var temperature = rootElement.Element("product").Element("time").Element("location").Element("temperature").Attribute("value").Value;

            return new OkObjectResult(temperature);
        }
    }
}
