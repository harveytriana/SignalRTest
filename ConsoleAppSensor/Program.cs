// original
// Update to .NEY Core 3.1 bt Luis Harvey Triana Vega
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
        // Solution  
        static string _HubUri = "http://localhost:8016";
        // IIS
        // static string _HubUri = "http://localhost/SignalrTest";
        static readonly string _HubPath = "/sensor";

        static void Main()
        {
            Console.WriteLine("Press Enter key when server is ready");
            Console.ReadKey();
            Console.Clear();

            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();

            // start
            logger.LogInformation("Start Thread");

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            Task.Run(() => MainAsync(cancellationToken, logger).Wait());

            Console.WriteLine("\nPress Enter to Exit ...");
            Console.ReadKey();

            cancellationTokenSource.Cancel();
        }

        async static Task MainAsync(CancellationToken cancellationToken, ILogger logger)
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

                // Log informations:
                logger.LogInformation($"Broadcasting: {measurement}");

                // Finally send the value:
                await hubConnection.SendAsync("Broadcast", "Sensor", measurement);
            }
        }
    }
}
