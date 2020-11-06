using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClientChatTest
{
    class Program
    {
        static HubConnection _connection;

        // Solution  
        // static string _HubUri = "http://localhost:8016";
        // IIS
        static string _HubUri = "http://localhost/SignalrTest";

        static readonly string _HubPath = "/chatHub";

        static void Main()
        {
            Prompt("SignalR Clasic Chat Sample\n", ConsoleColor.Green);

            Console.WriteLine("Press Enter key when server is ready");
            Console.ReadKey();
            Console.Clear();

            if (StartConnectionAsync().Result) {
                // subscribe remote to events
                _connection.On<string, string>("ReceiveMessage", (user, message) => {
                    Prompt($"{user}: {message}", ConsoleColor.DarkYellow);
                });
                // wait
                Prompt("Press any key to close connection\n", ConsoleColor.DarkGray);
                Console.ReadKey();

                DisposeAsync().Wait();

                Thread.Sleep(2000);
            }
        }

        public static async Task<bool> StartConnectionAsync()
        {
            try {
                _connection = new HubConnectionBuilder()
                     .WithUrl(_HubUri + _HubPath)
                     .Build();

                await _connection.StartAsync();
                await _connection.SendAsync("SendMessage", "Console Client", "Hello");

                Prompt($"\nHub is Started. Waiting Signals.", ConsoleColor.Yellow);

                return true;
            }
            catch (Exception exception) {
                Prompt($"Exception:\n{exception.Message}");
            }
            return false;
        }

        public static async Task DisposeAsync()
        {
            Prompt("Unsubscribed");
            await _connection.DisposeAsync();
        }

        public static void Prompt(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
