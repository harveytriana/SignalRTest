using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppClassicChat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            Prompt("OnStart");
        }

        protected override void OnSleep()
        {
            Prompt("OnSleep");
        }

        protected override void OnResume()
        {
            Prompt("OnResume");
        }

        private void Prompt(string text)
        {
            Console.WriteLine($"** {text}");
        }
    }
}
