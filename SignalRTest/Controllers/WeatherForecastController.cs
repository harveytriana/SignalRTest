using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SignalRTest.Shared;

namespace SignalRTest.Controllers
{
    // url are: ApiRoot/*controller/[something]
    // where *controler is the ClassNameWithout 'Controller' sufix
    // 
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // inject logger:
        //private readonly ILogger<WeatherForecastController> _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        // GET: weatherforecast/ 
        [HttpGet]
        public string Get()
        {
            return "REST Laboratory.";
        }

        // GET: weatherforecast/report
        [HttpGet("Report")]
        public IEnumerable<WeatherForecast> Report()
        {
            var random = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = random.Next(-20, 55),
                Summary = Summaries[random.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // GET: weatherforecast/getf/123.2 
        [HttpGet("GetF/{degreesCelsius}")]
        public double GetF(double degreesCelsius)
        {
            return degreesCelsius * 1.8 + 32.0;
        }

        // GET: weatherforecast/getc/123.2
        [HttpGet("GetC/{degreesFahrenheit}")]
        public double GetC(double degreesFahrenheit)
        {
            return (degreesFahrenheit - 32.0) / 1.8;
        }
    }
}
