using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using SignalRTest.Shared;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAppTest
{
    public partial class WeatherReportForm : Form
    { // Solution (run both projects)
        // static string _HubUri = "http://localhost:8016";
        // Publishen in IIS
        readonly string _HubUri = "http://localhost/SignalrTest";
        readonly string _HubPath = "/weatherReportHub";

        HubConnection _Connection;

        // business logic
        readonly string[] _countries =
        {
            "Afghanistan",
            "Armenia",
            "Brazil",
            "Colombia",
            "Grenada",
            "Ireland",
            "Seychelles",
            "Swaziland",
            "Zimbabwe"
        };
        readonly Random _random = new Random();

        readonly string _userId = Guid.NewGuid().ToString();

        public WeatherReportForm()
        {
            InitializeComponent();
            Show();

            FormClosing += async (s, e) => await ExitAsync();

            Task.Run(() => MainAsync());

            buttonSend.Click += async (s, e) => await SendAsync();

            buttonClear.Click += (s, e) => {
                listBoxResult.Invoke((Action)(() => listBoxResult.Items.Clear()));
            };

            buttonSend.Enabled = false;
        }

        async Task MainAsync()
        {
            try {
                _Connection = new HubConnectionBuilder()
                     .WithUrl(_HubUri + _HubPath)
                     .AddMessagePackProtocol()
                     .Build();

                await _Connection.StartAsync();

                // to subscribe, map exactly the hub's function
                _Connection.On<WeatherReport>("Receive", (data) => {
                    var s = $"Sender: {data.UserId.Substring(0, 8)} {data.Country}: {data.Temperature:N2} ºF";

                    listBoxResult.AddItemThread(s);
                });

                buttonSend.Let(x => x.Enabled = true);
            }
            catch { }
        }

        async Task ExitAsync()
        {
            await Task.Delay(200);

            if (_Connection != null) {
                await _Connection.DisposeAsync();

                Trace.WriteLine("Coonnection is closed");
            }
        }

        async Task SendAsync()
        {
            try {
                for (int i = 1; i <= 8; i++)
                    await _Connection.InvokeAsync("Send", new WeatherReport
                    {
                        UserId = _userId,
                        Temperature = GetTemperature(),
                        Country = GetCountry()
                    });
            }
            catch { }
        }

        private string GetCountry()
        {
            // support boundaries
            var i = _random.Next(-2, _countries.Length + 1);
            if (i < 0) i = 0;
            if (i > _countries.Length - 1) i = _countries.Length - 1;
            // random choice
            return _countries[i];
        }

        public double GetTemperature()
        {
            return 60.0 + Math.Floor(_random.NextDouble() * 1000.0 + 1.0) / 700.0;
        }
    }
}