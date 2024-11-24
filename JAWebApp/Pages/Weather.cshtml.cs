using JAWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;


namespace JAWebApp.Pages
{
    public class WeatherModel : PageModel
    {
        private readonly ILogger<WeatherModel> _logger;

        public WeatherModel(ILogger<WeatherModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        async static Task<HttpResponseMessage> GetResponseString(string Latitude, string Longitude)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "my-user-agent-name");
            var response = await httpClient.GetAsync($"https://jahttpfunctionapp.azurewebsites.net/api/Function1?code=-VEE_oXc5Y1Zx5OlSzy7lv6-gOO_8Za0G8Il0w51erZoAzFuTypQWw%3D%3D&lat={Latitude}&lon={Longitude}");
            return response;

        }
        public string ActionResultMessageText { get; set; }
        public string ActionResultAlertMessageText { get; set; }
        public async void OnPost()
        {
            var latitudeVar = Request.Form["latitude"];
            var longitudeVar = Request.Form["longitude"];

            var temperature = float.Parse(GetResponseString(latitudeVar, longitudeVar).Result.Content.ReadAsStringAsync().Result);

            var jackets = temperature > 15 ? 0 : Math.Floor((15 - temperature) / 10);
            this.ActionResultMessageText = string.Empty;
            this.ActionResultAlertMessageText = jackets == 0 ?
                $"It is {temperature} degrees. You do not need a jacket" :
                jackets == 1 ? $"It is {temperature} degrees. You need a jacket" : $"It is {temperature} degrees. You need {jackets} jackets";
        }
    }
}