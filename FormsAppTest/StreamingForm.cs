using SignalRTest.Shared;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAppTest
{
    public partial class StreamingForm : Form
    {
        readonly StreamingClient _ss;
        readonly string _url = Constants.IISSITE + "/streamHub";

        public StreamingForm()
        {
            InitializeComponent();

            _ss = new StreamingClient(_url);
            _ss.Prompt += (s) => Prompt(s);

            buttonConnect.Click += (s, e) => Connect();
            buttonServerToClient.Click += (s, e) => ServerToClient();
            buttonClientToServer.Click += (s, e) => ClientToServer();
            FormClosing += (s, e) => _ss.Dispose();

            Prompt(_url);
        }

        private void Connect()
        {
            buttonConnect.Enabled = false;

            Task.Run(async () => {
                if (await _ss.ConnectAsync()) {
                    buttonClientToServer.Let(x => x.Enabled = true);
                    buttonServerToClient.Let(x => x.Enabled = true);
                }
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
                // ChannelReader: UploadStream
                // IAsyncEnumerable: UploadStream2
                // 
                await _ss.SendStream();

                // another sample
                // await _ss.SendStreamBasicDemotration();
            });
        }

        private void Prompt(string text)
        {
            labelPrompt.Let(x => x.Text = text);

            if (text == "Complete") {
                buttonClientToServer.Let(x => x.Enabled = true);
                buttonServerToClient.Let(x => x.Enabled = true);
            }
        }
    }
}
