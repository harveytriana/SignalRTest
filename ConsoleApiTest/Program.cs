using SignalRTest.Shared;
using System;
using System.Threading.Tasks;

namespace ConsoleApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.ReadKey();

            OnGet().Wait();
        }

        static async Task OnGet()
        {
            var apiRoot = "http://localhost:8016";
            //var apiRoot = "http://localhost/signalrtest";

            using var rest = new RestClient(apiRoot);

            var data = await rest.GetAll<WeatherForecast>("/WeatherForecast/Report");

            if (data != null) {
                foreach (var i in data) {
                    Console.WriteLine(i);
                }
            } else { Console.WriteLine("Data is null."); } 
        }
    }
}
