using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileAppFormsTabbed1.Services;
using MobileAppFormsTabbed1.Views;

namespace MobileAppFormsTabbed1
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
