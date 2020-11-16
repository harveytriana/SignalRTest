// original
// Update to .NET Core 3.1: Luis Harvey Triana Vega
//
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRTest.Shared;

namespace ConsoleAppSensor
{
    class Program
    {
        // Solution (run both projects)
        // static string _HubUri = "http://localhost:8016";
        // Publishen in IIS
        static readonly string _HubUri = Constants.IISSITE;
        static readonly string _HubPath = "/sensor";

        static Tracer _tracer;

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("SENSOR TEST\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Press Enter key when server is ready");
            Console.ReadKey();
            Console.Clear();

            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();

            _tracer = new Tracer();
            _tracer.Start("Sensor_00", false);

            // start
            logger.LogInformation("Start Thread");

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            Task.Run(() => MainAsync(logger, cancellationToken).Wait());

            Console.WriteLine("\nPress Enter to Exit ...");
            Console.ReadKey();

            // cancel the thread
            cancellationTokenSource.Cancel();
        }

        async static Task MainAsync(ILogger logger, CancellationToken cancellationToken)
        {
            var hubConnection = new HubConnectionBuilder()
                 .WithUrl(_HubUri + _HubPath)
                 .Build();

            await hubConnection.StartAsync();

            // Initialize a new Random Number Generator:
            var rnd = new Random();

            var value = 0.0;

            var measurement = new Measurement();

            while (true) {
                if (cancellationToken.IsCancellationRequested) {
                    await hubConnection.DisposeAsync();
                    return;
                }
                await Task.Delay(250);
                // Generate the value to Broadcast to Clients:
                value = Math.Min(Math.Max(value + (0.1 - rnd.NextDouble() / 5.0), -1), 1);

                // Set the Measurement with a Timestamp assigned:
                measurement.Timestamp = DateTime.UtcNow;
                measurement.Value= value;
                // report
                logger.LogInformation($"Broadcasting: {measurement}");

                _tracer.Log($"{measurement.Timestamp:HH:mm:ss}\t{measurement.Value:0.0000}");

                // Finally send the value:
                await hubConnection.SendAsync("Broadcast", "Sensor", measurement);
            }
        }
    }
}
