using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppClassicChat
{
    public partial class MainPage : ContentPage
    {
        HubConnection _h;
        bool _IsConnected;
        string _user;

        public MainPage()
        {
            InitializeComponent();
            // default
            _user = "Android Client";

            buttonConnect.Clicked += async (s, e) => await ConnectAsync();
            buttonSend.Clicked += async (s, e) => await SendAsync();
        }

        private async Task ConnectAsync() {
            if (_IsConnected) {
                Prompt("This client is connected.");
                return;
            }
            Prompt("Connecting...");
            try {
                _h = new HubConnectionBuilder()
                    .WithUrl(entryUrl.Text)
                    .Build();

                // events
                _h.On<string, string>("ReceiveMessage", (user, message) => {
                    Prompt($"{user}: {message}");
                });

                //_h.On<int>("ConnectedClients", (clientNumber) => {
                //    if (_user == null) {
                //        _user = $"AndroidClient_{clientNumber}";
                //    }
                //});

                await _h.StartAsync();
                // subscribe
                // await _h.SendAsync("Subscribe");

                _IsConnected = true;

                Prompt("Connected.");
            }
            catch (Exception exception) {
                Prompt(exception.Message);
                _IsConnected = false;
                return;
            }
        }

        private async Task SendAsync()
        {
            if (_IsConnected) {
                await _h.SendAsync("SendMessage", _user, entryMessage.Text);
            } else {
                Prompt("This client is not connected.");
            }
        }

        private void Prompt(string text)
        {
            labelPrompt.Text = text;
        }
    }
}

