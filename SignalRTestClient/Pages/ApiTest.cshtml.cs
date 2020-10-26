using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRTest.Shared;

namespace SignalRTestClient.Pages
{
    public class ApiTestModel : PageModel
    {
        public List<WeatherForecast> Data { get; private set; }
        public string ApiRoot { get; private set; }

        public async Task OnGet()
        {
            //ApiRoot = "http://localhost:8016";
            ApiRoot = "http://localhost/signalrtest";

            using var rest = new RestClient(ApiRoot);

            Data = await rest.GetAll<WeatherForecast>("/WeatherForecast/Report/");

        }
    }
}
