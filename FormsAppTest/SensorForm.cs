using Microsoft.AspNetCore.SignalR.Client;
using SignalRTest.Shared;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAppTest
{
    public partial class SensorForm : Form
    {
        // Solution (run both projects)
        // static string _HubUri = "http://localhost:8016";
        // Publishen in IIS
        readonly string _HubUri = "http://localhost/SignalrTest";
        readonly string _HubPath = "/sensor";

        HubConnection _Connection;

        public SensorForm()
        {
            InitializeComponent();
            Show();

            FormClosing += async (s, e) => await ExitAsync();

            Task.Run(() => MainAsync());
        }

        async Task MainAsync()
        {
            _Connection = new HubConnectionBuilder()
                 .WithUrl(_HubUri + _HubPath)
                 .Build();

            await _Connection.StartAsync();

            // to subscribe, map exactly the hub's function
            _Connection.On<string, Measurement>("Broadcast", (sender, measurement) => {
                labelTime.Let(x => x.Text = measurement.Timestamp.ToString("HH:mm:ss"));
                labelValue.Let(x => x.Text = measurement.Value.ToString("N6"));
            });
        }

        async Task ExitAsync()
        {
            await Task.Delay(200);

            if (_Connection != null) {
                await _Connection.DisposeAsync();

                Trace.WriteLine("Coonnection is closed");
            }
        }
    }

}
