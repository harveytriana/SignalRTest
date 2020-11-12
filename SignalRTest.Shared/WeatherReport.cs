using System;

namespace SignalRTest.Shared
{
    public class WeatherReport
    {
        public string ConnectionId { get; set; }
        public double Temperature { get; set; }

        static readonly Random _random = new Random();

        public static double GetTemperature()
        {
            return 60.0 + Math.Floor(_random.NextDouble() * 1000.0 + 1.0) / 700.0;
        }
    }
}
