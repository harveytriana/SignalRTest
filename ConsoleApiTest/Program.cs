using SignalRTest.Shared;
using System;
using System.Threading.Tasks;

namespace ConsoleApiTest
{
    class Program
    {
        //static Tracer _tracer = new Tracer();

        static void Main(string[] args)
        {
            Console.WriteLine("Another Tests");

            //_tracer.Start("SignalRTest_Index_{Date}");
            //_tracer.Log("This is a Tracer test.");

            var x = System.Reflection.Assembly.GetExecutingAssembly();

            OnGet().Wait();
        }

        static async Task OnGet()
        {
            //var apiRoot = "http://localhost:8016";
            var apiRoot = Constants.IISSITE;

            if (apiRoot != Constants.IISSITE) {

                Console.WriteLine("\nPress any key when Server is ready.\n");
                Console.ReadKey();
            }

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
