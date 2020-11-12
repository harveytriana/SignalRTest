using System.Windows.Forms;

namespace FormsAppTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            buttonSensor.Click += (s, e) => new SensorForm().Show();
            buttonWeatherReport.Click += (s, e) => new WeatherReportForm().Show();
            buttonStreamingExample.Click += (s, e) => new StreamingForm().Show();
        }
    }
}
