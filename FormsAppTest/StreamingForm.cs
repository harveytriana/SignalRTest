using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAppTest
{
    public partial class StreamingForm : Form
    {
        readonly string[] _url = {
            "http://localhost/SignalrTest/streamHub",
            "https://localhost:8016/streamHub",
        };
        readonly int _urlIndex = 0;

        readonly StreamingClient _ss;

        public StreamingForm()
        {
            InitializeComponent();

            _ss = new StreamingClient(_url[_urlIndex]);
            _ss.Prompt += (s) => Prompt(s);

            buttonConnect.Click += (s, e) => Connect();
            buttonServerToClient.Click += (s, e) => ServerToClient();
            buttonClientToServer.Click += (s, e) => ClientToServer();
            FormClosing += (s, e) => _ss.Dispose();

            Prompt(_url[_urlIndex]);
        }

        private void Connect()
        {
            buttonConnect.Enabled = false;
            buttonServerToClient.Enabled = true;
            buttonClientToServer.Enabled = true;

            Task.Run(async () => {
                await _ss.ConnectAsync();
            });
        }

        private void ServerToClient()
        {
            buttonServerToClient.Enabled = false;
            buttonClientToServer.Enabled = false;

            Task.Run(async () => {
                await _ss.ReadStream();
            });
        }

        private void ClientToServer()
        {
            buttonServerToClient.Enabled = false;
            buttonClientToServer.Enabled = false;

            Task.Run(async () => {
                await _ss.SendStreamBasicDemotration();
                // await _ss.SendStream();
            });
        }

        private void Prompt(string text)
        {
            labelPrompt.Let(x => x.Text = text);
        }
    }
}
