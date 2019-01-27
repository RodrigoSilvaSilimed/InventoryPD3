using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ZXing.Net.Mobile.Forms;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Media.Abstractions;
using Plugin.Geolocator;
using Firebase.Storage;
using Firebase.Database;
using Firebase.Database.Query;


namespace Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : MasterDetailPage
    {
        

        public Menu()
        {
            InitializeComponent();   

        }

        private void GoPaginaScan(object sender, EventArgs args)
        {
            Detail = new NavigationPage(new Pages.ScanPage());
            IsPresented = false; //para esconder o menu GoPaginaLeituras
        }
        private void GoPaginaLeituras(object sender, EventArgs args)
        {
            Detail = new NavigationPage(new Pages.LeiturasPage());            
            IsPresented = false; //para esconder o menu 
        }
        private void GoPaginaStatus(object sender, EventArgs args)
        {
            Detail = new NavigationPage(new Pages.StatusPage());
            IsPresented = false; //para esconder o menu
        }
        private void GoPaginaAjuda(object sender, EventArgs args)
        {
            Detail = new NavigationPage(new Pages.AjudaPage());           
            IsPresented = false; //para esconder o menu
        }
        private void GoPaginaSobre(object sender, EventArgs args)
        {
            Detail = new NavigationPage(new Pages.SobrePage());
            IsPresented = false; //para esconder o menu 
        }
        private void Sair(object sender, EventArgs args)
        {
            InventoryPD3.App.Current.Quit();
            

            IsPresented = false; //para esconder o menu
        }

       
    }
}