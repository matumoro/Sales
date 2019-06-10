using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sales
{
    using System;
    using Views;
    public partial class App : Application
    {
        public App()
        {
           InitializeComponent();

           MainPage = new  NavigationPage(new ProductsPage());
        }
/*
        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }
*/
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
