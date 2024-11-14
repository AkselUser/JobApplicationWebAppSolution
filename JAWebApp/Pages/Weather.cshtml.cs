using JAWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;


namespace JAWebApp.Pages
{
    public class WeatherModel : PageModel
    {
        [BindProperty]
        public CoordinatesModel Coordinates { get; set; }

        private readonly ILogger<WeatherModel> _logger;
        public WeatherModel(ILogger<WeatherModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            // Save Model to Database

            return RedirectToPage("/Index", new {Coordinates.Latitude});
        }
    }
}