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
using InventoryPD3.Servico.Entidade;


namespace Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : MasterDetailPage
    {
        

        public Menu()
        {
            InitializeComponent();   

        }

        public Menu(Entidade_Login Login)
        {
            InitializeComponent();
            NomeUsuario.Text = Login.name;

        }
        public Menu(InventoryPD3.Servico.BLL.User Login)
        {
            InventoryPD3.App._Usuario = Login;

            InitializeComponent();
            

            NomeUsuario.Text = Login.GivenName +" "+ Login.FamilyName;
            email.Text = Login.Email;
            img_User.Source = Login.Picture;
            _cliente.Text = "Cliente " + Login.Cliente;

            //var webImage = new Image { Source = ImageSource.FromUri(new Uri("https://xamarin.com/content/images/pages/forms/example-app.png")) };

            //webImage.Source = "https://xamarin.com/content/images/pages/forms/example-app.png";

            //image.Source = new UriImageSource { CachingEnabled = false, Uri="http://server.com/image" };
            /*webImage.Source = new UriImageSource
            {
                Uri = new Uri("https://xamarin.com/content/images/pages/forms/example-app.png"),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0) //To set a specific cache period (for example, 5 days) i
            };*/

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

        /*
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
        */

        async private void GoSair(object sender, EventArgs args)
        {
            //InventoryPD3.App.Current.Quit();
            
            var answer = await DisplayAlert("Aviso!", "Você realmente deseja sair?", "Sim", "Não");            
            if (answer==true)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
            
            /*
            var action = await DisplayActionSheet"ActionSheet: SavePhoto?", "Cancel", "Delete", "Photo Roll", "Email");
            if (action == "Sim")
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
            */
            IsPresented = false; //para esconder o menu
        }

        

    }
}