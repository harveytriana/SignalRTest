using SignalRTest.Shared;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAppTest
{
    public partial class StreamingForm : Form
    {
        readonly StreamingClient _streamingClient;
        readonly string _url = Constants.IISSITE + "/streamHub";

        public StreamingForm()
        {
            InitializeComponent();

            _streamingClient = new StreamingClient(_url);
            _streamingClient.Prompt += (s) => Prompt(s);

            buttonConnect.Click += async (s, e) => await Connect();
            buttonServerToClient.Click += async (s, e) => await ServerToClient();
            buttonClientToServer.Click += async (s, e) => await ClientToServer();
            FormClosing += (s, e) => _streamingClient.Dispose();

            Prompt(_url);
        }

        async Task Connect()
        {
            buttonConnect.Enabled = false;

            if (await _streamingClient.ConnectAsync()) {
                buttonClientToServer.Let(x => x.Enabled = true);
                buttonServerToClient.Let(x => x.Enabled = true);
            }
        }

        async Task ServerToClient()
        {
            buttonServerToClient.Enabled = false;
            buttonClientToServer.Enabled = false;

            await _streamingClient.ReadStream2();
        }

        async Task ClientToServer()
        {
            buttonServerToClient.Enabled = false;
            buttonClientToServer.Enabled = false;

            // TWO APPROACHS
            // await _streamingClient.SendStreamChannel();
            await _streamingClient.SendStreamEnumerable();

            // another sample
            // await _streamingClient.SendStreamBasicDemotration();
        }

        private void Prompt(string text)
        {
            labelPrompt.Let(x => x.Text = text);

            if (text == "Completed") {
                buttonClientToServer.Let(x => x.Enabled = true);
                buttonServerToClient.Let(x => x.Enabled = true);
            }
        }
    }
}
