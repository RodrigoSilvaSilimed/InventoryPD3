using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Permissions;
using Plugin.Geolocator;


using Plugin.Connectivity;


namespace Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : MasterDetailPage
    {

        private bool statusWifi = false;
        private bool statusGPS = false; //stats do GPS. Encontrado = true
        private Plugin.Geolocator.Abstractions.Position position; //última posição obtida
        private Control.Control Controle;
        public Menu()
        {
            InitializeComponent();
            TimerGPSWifi();
        }

        private void GoPaginaScan(object sender, EventArgs args)
        {
            Detail = new Pages.ScanPage();
            IsPresented = false; //para esconder o menu
        }
        private void GoPaginaStatus(object sender, EventArgs args)
        {
            Detail = (new Pages.StatusPage());
            IsPresented = false; //para esconder o menu
        }
        private void GoConsulta(object sender,EventArgs args)
        {
            Detail = (new Pages.ConsultaPage());
            IsPresented = false; //para esconder o menu
        }
        private void GoPaginaAjuda(object sender, EventArgs args)
        {
            Detail = (new Pages.AjudaPage());           
            IsPresented = false; //para esconder o menu
        }
        private void GoPaginaSobre(object sender, EventArgs args)
        {
            Detail = (new Pages.SobrePage());
            IsPresented = false; //para esconder o menu
        }
        private void Sair(object sender, EventArgs args)
        {
            //InventoryPD3.App.Current.Quit();
         
            IsPresented = false; //para esconder o menu
        }

        private void TimerGPSWifi()
        {
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                // Do something
                TakeGPS();
                WiFiStatus();

                return true; // True = Repeat again, False = Stop the timer
            });
        }

        public void WiFiStatus()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    // your logic... 
                    img_WifiStatus.Source = "wifiOK.png";
                    statusWifi = true;

                }
                else
                {
                    // write your code if there is no Internet available
                    img_WifiStatus.Source = "wifiNOk.png";
                    statusWifi = false;
                }
            }
            catch (Exception e)
            {

            }
        }

        public async void TakeGPS()
        {
            var GPSStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);
            try
            {

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                if (position != null)
                {
                    img_GPSStatus.Source = "GPSEncontrado.png";
                    statusGPS = true;
                }
                else
                {
                    img_GPSStatus.Source = "GPSBuscando.png";
                    statusGPS = false;
                }

            }
            catch (Exception e)
            {

            }
        }
    }
}