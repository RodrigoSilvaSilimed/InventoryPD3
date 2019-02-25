using Plugin.Geolocator;
using Plugin.Permissions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Connectivity;
using InventoryPD3.Servico.DAL;
using InventoryPD3.Servico.Entidade;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace InventoryPD3
{
    public partial class App : Application
    {
        public static Plugin.Permissions.Abstractions.PermissionStatus _cameraPermissionStatus { get; set; }
        public static Plugin.Permissions.Abstractions.PermissionStatus _storagePermissionStatus { get; set; }
        public static Plugin.Permissions.Abstractions.PermissionStatus _GPSPermissionStatus { get; set; }
        public static bool _statusWifi { get; set; } = false;
        public static Plugin.Geolocator.Abstractions.Position _position { get; set; } //última posição obtida
        public static string _inventario { get; set; } = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00");

        //GPS
        private const int TempoAtualizacaoGPSWifiInicial = 2;//Intervalo inicial, mais curto para inicar logo o uso do App
        private const int TempoAtualizacaoGPSWifiFinal = 60;//Intervalo mais longo para economizar bateria
        private const int TempoTimoutGPS = 10;//Tempo de timout do GPS
        private const int PrecisaoDesejadaGPS = 50;//Precição deseja em metros
        private const int TempoSincronizacaoBanco = 60;



        private int _TempoTimer { get; set; } = TempoAtualizacaoGPSWifiInicial; //tempo em segundos em que o timer procura informações GPS.
        public App()
        {
            InitializeComponent();

            ChecarPermissões();
            TimerGPSWifi();
            TimerSync();

            // MainPage = new MainPage() https://forum.casadocodigo.com.br/t/resolvido-xamarin-forms-erro-com-pushasync/468/9
            //MainPage = new NavigationPage(new MainPage());
            //MainPage = (new Master.Menu()); // ultimo que usei em 06/02
            //MainPage = (new Pages.LoginPage());
            App.Current.MainPage = new Pages.LoginFacebookPage();
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
        private async void ChecarPermissões()
        {
            try
            {
                _GPSPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);
                _cameraPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
                _storagePermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            }
            catch (Exception e)
            {

            }

        }
        private void TimerGPSWifi()
        {
            try
            {
                TakeGPS();
                Device.StartTimer(TimeSpan.FromSeconds(_TempoTimer), () =>
                {
                    // Do something
                    TakeGPS();
                    AtualizaStatusWiFi();
                    return true; // True = Repeat again, False = Stop the timer
                });
            }
            catch (Exception e)
            {

            }
        }
        private void TimerSync()
        {
            Device.StartTimer(TimeSpan.FromSeconds(TempoSincronizacaoBanco), () =>
            {
                // Do something
                EnviaLeiturasWebDB();
                return true; // True = Repeat again, False = Stop the timer
            });
            
        }

        public async void TakeGPS()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = PrecisaoDesejadaGPS;
                _position = await locator.GetPositionAsync(TimeSpan.FromSeconds(TempoTimoutGPS));
                if ((_position != null) && (_TempoTimer == TempoAtualizacaoGPSWifiInicial)) //Se tiver posição GPS e se o timer estiver setado para intervalo mais curto (inicial) setar para intervalo maior para não gastar bateria do celular com GPS
                {
                    _TempoTimer = TempoAtualizacaoGPSWifiFinal;//Aumentar o intervalo
                }
            }
            catch (Exception e)
            {

            }
        }
        public void AtualizaStatusWiFi()
        {
            try
            {
                _statusWifi = (CrossConnectivity.Current.IsConnected);
            }
            catch (Exception e)
            {

            }
        }

        public async void EnviaLeiturasWebDB()
        {
            try
            {
                if (_statusWifi)
                {
                    DAL_Database database = new DAL_Database();
                    var Lista = database.ConsultarLeiturasParaSincronizar();
                    if (Lista.Count > 0)
                    {
                        DAL_Firebase firebase = new DAL_Firebase();

                        foreach (Entidade_Leitura leitura in Lista)
                        {
                            //SendToFirebase(leitura);
                            await firebase.SendToFirebase(leitura);
                            var _leiturasFirebase = await firebase.BuscarLeitura(leitura);
                            if (_leiturasFirebase.Count > 0)
                            {
                                leitura.Synced = true;
                                database.Atualizacao(leitura);
                            }
                            /*foreach (var Lleituras in _leiturasFirebase)
                            {
                                //leitura.Synced = true;

                                //Console.WriteLine($"{Lleituras.Key} is {Lleituras.Object.CodigoBarras} m high.");
                            }*/

                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }        
    }
}
