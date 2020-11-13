using SignalRTest.Shared;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAppTest
{
    public partial class StreamingForm : Form
    {
        readonly StreamingHandler _streamingHandler;
        readonly string _url = Constants.IISSITE + "/streamHub";

        public StreamingForm()
        {
            InitializeComponent();

            _streamingHandler = new StreamingHandler(_url);
            _streamingHandler.Prompt += (s) => Prompt(s);

            buttonConnect.Click += async (s, e) => await Connect();
            buttonServerToClient.Click += async (s, e) => await ServerToClient();
            buttonClientToServer.Click += async (s, e) => await ClientToServer();
            FormClosing += (s, e) => _streamingHandler.Dispose();

            Prompt(_url);
        }

        async Task Connect()
        {
            buttonConnect.Enabled = false;

            if (await _streamingHandler.ConnectAsync()) {
                buttonClientToServer.Let(x => x.Enabled = true);
                buttonServerToClient.Let(x => x.Enabled = true);
            }
        }

        async Task ServerToClient()
        {
            buttonServerToClient.Enabled = false;
            buttonClientToServer.Enabled = false;

            // TWO APPROACHS
            // await _streamingHandler.ReadStreamChannel();
            await _streamingHandler.ReadStream();
        }

        async Task ClientToServer()
        {
            buttonServerToClient.Enabled = false;
            buttonClientToServer.Enabled = false;

            // TWO APPROACHS
            // await _streamingHandler.SendStreamChannel();
            await _streamingHandler.SendStreamEnumerable();

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
