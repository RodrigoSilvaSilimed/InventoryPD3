using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace InventoryPD3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // MainPage = new MainPage() https://forum.casadocodigo.com.br/t/resolvido-xamarin-forms-erro-com-pushasync/468/9
            //MainPage = new NavigationPage(new MainPage());
            MainPage = (new Master.Menu());
        }

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
