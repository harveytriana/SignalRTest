using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppClassicChat
{
    public partial class MainPage : ContentPage
    {
        HubConnection _connection;
        bool _isConnected;
        string _user;

        public MainPage()
        {
            InitializeComponent();
            // default
            _user = "Android Client";

            buttonConnect.Clicked += async (s, e) => await ConnectAsync();
            buttonSend.Clicked += async (s, e) => await SendAsync();
        }

        private async Task ConnectAsync()
        {
            if (_connection != null) {
                Prompt("Connection is active.");
                return;
            }
            Prompt("Connecting...");
            try {
                _connection = new HubConnectionBuilder()
                    .WithUrl(entryUrl.Text)
                    .WithAutomaticReconnect()
                    .Build();

                await _connection.StartAsync();

                if (_connection.State == HubConnectionState.Connected) {
                    // subscribe to hub events
                    _connection.Closed += async (e) => await ConnectionClosed(e);
                    _connection.Reconnected += async (s) => await ConnectionReconnected(s);
                    _connection.Reconnecting += async (e) => await ConnectionReconnectiong(e);

                    // logic events
                    _connection.On<string, string>("ReceiveMessage", (user, message) => {
                        Prompt($"{user}: {message}");
                    });

                    // subscribe to server logic
                    // await _h.SendAsync("Subscribe");

                    //_h.On<int>("ConnectedClients", (clientNumber) => {
                    //    if (_user == null) {
                    //        _user = $"AndroidClient_{clientNumber}";
                    //    }
                    //});

                    Prompt($"State: {_connection.State}", true);
                    _isConnected = true;
                }
            }
            catch (Exception exception) {
                Prompt(exception.Message, true);
                _isConnected = false;
                return;
            }
        }

        private async Task SendAsync()
        {
            if (_isConnected) {
                await _connection.SendAsync("SendMessage", _user, entryMessage.Text);
            } else {
                Prompt("This client is not connected.");
            }
        }

        async Task ConnectionClosed(Exception e)
        {
            Prompt($"Closed. E: {e.Message}", true);
            Console.WriteLine($"State: {_connection.State}");

            await Task.Delay(100);

            // Manually reconnect. It is replaced by .WithAutomaticReconnect()
            //if (_connection.State == HubConnectionState.Disconnected) {
            //    await Task.Delay(new Random().Next(0, 5) * 1000);
            //    await _connection.StartAsync();
            //}
        }

        async Task ConnectionReconnectiong(Exception e)
        {
            Console.WriteLine($"Reconnecting. E: {e.Message}");
            Console.WriteLine($"State: {_connection.State}");

            await Task.Delay(100);
        }

        async Task ConnectionReconnected(string arg)
        {
            Prompt($"Reconnected. {arg}", true);
            Console.WriteLine($"State: {_connection.State}");

            await Task.Delay(200);
        }

        private void Prompt(string text, bool console = false)
        {
            labelPrompt.Text = text;
            if (console) {
                Console.WriteLine($"** {text}");
            }
        }
    }
}

