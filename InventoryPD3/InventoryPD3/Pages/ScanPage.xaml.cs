
using InventoryPD3.Servico.Entidade;
using InventoryPD3.Servico.DAL;

using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using InventoryPD3;
using Firebase.Database;
using Firebase.Database.Query;

namespace Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanPage : ContentPage
	{
        //variáveis globais
        private string Cliente;
        private string Usuario;
       
        private Control.Control Controle;

        public ScanPage ()
		{
			InitializeComponent ();
            AtualizaInventario();
            ConsultarLeituras();
            TimerGPSWifi();
            Controle = new Control.Control();
            //Entidade_Usuario
            Cliente = "999999";
            Usuario = "rsilva@silimed.com.br";
            
        }

        private void LerBarcode(object sender, EventArgs arg)
        {
            Entidade_Leitura leitura = new Entidade_Leitura();
            leitura.Cliente = Cliente;
            leitura.Usuario = Usuario;
            Scan(leitura);
            /*
             * Como os métodos de ler códigos de barras e tirar fotos são assíncronos, a chamada da foto está dentro do método de leitura de código de barras
             * para que os doi métodos não sejam chamados ao mesmo tempo.
             * O método Scan recebe um objeto do tipo Leitura e acrescenta a ele informações de códigos de barras e GPS e o encaminha para o método da Câmera.
             * Scan-->TakePhoto-->
             * 
             * */
        }

        private void GPSStatus(object sender, EventArgs arg)
        {
            /*
             if (InventoryPD3.App._position==null)
             {
                 DisplayAlert("", "Aguardando sinal GPS.", "OK");
             }
             else
             {
                 if (InventoryPD3.App._position != null)
                 {
                     DisplayAlert("Status do GPS", "Latitude: " + string.Format("{0:0.000}", App._position.Latitude) + " Longitude: " + string.Format("{0:0.000}", App._position.Longitude), "OK");
                 }
             }
             */
            var action = DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");

        }

        public async void Scan(Entidade_Leitura leitura)
        {
            try
            {
                string resultado = "";
                //https://julianocustodio.com/2017/11/03/scanner/
                var scanpage = new ZXingScannerPage();

                scanpage.OnScanResult += (result) =>
                {
                    // Parar de escanear
                    scanpage.IsScanning = false;
                    //Poderia fazer qualquer outra ação
                    resultado = result.Text;
                    // Alert com o código escaneado
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        if ((Controle.ValidaBarcodeSN(result.Text)))
                        {
                            if (InventoryPD3.App._position != null)
                            {
                                leitura.CodigoBarras = result.Text;
                                leitura.TimestampLeitura = DateTime.Now.ToString();
                                leitura.GPSAccuracy = InventoryPD3.App._position.Accuracy.ToString();
                                leitura.GPSLatitude = InventoryPD3.App._position.Latitude.ToString();
                                leitura.GPSLongitude = InventoryPD3.App._position.Longitude.ToString();
                                leitura.GPSTimestamp = InventoryPD3.App._position.Timestamp.ToString();
                                leitura.Synced = false;
                                leitura.CaminhoImg = "N/A";
                                leitura.urlImg = "N/A";
                                leitura.Data = "N/A";
                                leitura.TimestampFoto = DateTime.Now.ToString();
                                //TakeCam(leitura); comentado pois desde 25/01 decidiu-se que não vamos mais armazenar a imagem do produto
                                DisplayAlert("Código Lido!", "O SN " + result.Text + " foi lido com sucesso!", "OK");
                                SendToSQLite(leitura);
                                //SendToFirebase(leitura);
                            }
                            else
                            {
                                DisplayAlert("Status GPS", "Não há sinal informações de localização", "OK");
                            }

                        }
                        else
                        {
                            Navigation.PopAsync();
                            DisplayAlert("Código inválido!", "Esse não é o código de barras do número de série do produto.", "OK");
                            Scan(leitura);
                        }

                    });
                };

                await Navigation.PushAsync(scanpage);
            }
            catch (Exception e)
            {

            }

        }

        public async void TakeCam(Entidade_Leitura leitura)
        {
            //https://github.com/jamesmontemagno/MediaPlugin

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            
            /*   if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
               {
                   await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                   return;
               }
              */
            //Checar e pedir permissão para câmera e storage
            if (InventoryPD3.App._cameraPermissionStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    //Directory = "Pictures",
                    Name = $"{leitura.CodigoBarras}.jpg",
                    CompressionQuality = 92, //0 é o mais comprimido. 92 é o recomendado
                    CustomPhotoSize = 50,//50% do original
                    //PhotoSize = PhotoSize.MaxWidthHeight,
                    PhotoSize = PhotoSize.Medium,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear
                    //Name = "test.jpg"
                });

                if (file == null)
                    return;

                //When your user takes a photo it will still store temporary data, but also if needed make a copy to the 
                //public gallery (based on platform). In the MediaFile you will now see a AlbumPath that you can query as well.

                //Get the public album path
                var aPpath = file.AlbumPath;

                //Get private path
                var path = file.Path;

                leitura.CaminhoImg = path;
                
                await DisplayAlert("Localização do Arquivo", file.Path, "OK");

                
                
                
                //leitura.urlImg = downloadUrl;
                //lb_Resultado.Text = downloadUrl.ToString();
                //SendToFirebase(leitura);
            }
            else
            {
                await DisplayAlert("Permissão Negada", "Não é possível tirar fotos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }
        }        

        private void SendToSQLite(Entidade_Leitura leitura)
        {
            //Salvar informações no Banco
            DAL_Database database = new DAL_Database();
            var _leitura = database.ObterVagaPorCodigoBarras(leitura.CodigoBarras);

            if (_leitura==null)
            {
                database.Cadastro(leitura);
            }
            else
            {
                DisplayAlert("Alerta!", "Você já leu esse Número de Série! As informações serão atualizadas!", "OK");
                database.Atualizacao(leitura);
            }
            ConsultarLeituras();


        }

        private void TimerGPSWifi()
        {
            AtualizaWiFiStatus();
            AtualizaGPSStatus();
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                // Do something
                AtualizaWiFiStatus();
                AtualizaGPSStatus();
                
                return true; // True = Repeat again, False = Stop the timer
            });
        }
        
        private void AtualizaWiFiStatus()
        {
            try
            {
                if (InventoryPD3.App._statusWifi)
                {
                    // your logic... 
                    img_WifiStatus.Source = "wifiOK.png";
                   
                }
                else
                {
                    // write your code if there is no Internet available
                    img_WifiStatus.Source = "wifiNOk.png";
                }
            }
            catch (Exception e)
            {
                
            }
        }

        private void AtualizaGPSStatus()
        {            
            try
            {                
                if (InventoryPD3.App._position != null)
                {
                    img_GPSStatus.Source = "GPSEncontrado.png";
                }
                else
                {
                    img_GPSStatus.Source = "GPSBuscando.png";
                }               

            }
            catch (Exception e)
            {

            }
        }

        private void AtualizaInventario()
        {
            lb_Inventario.Text = "Inventário " + InventoryPD3.App._inventario;            
        }

        private void ConsultarLeituras()
        {
            DAL_Database database = new DAL_Database();
            var Lista = database.Consultar();           

            //Lista = database.Pesquisa()
            lb_Contagem.Text = Lista.Count().ToString()+ " Unidades";
        }

        public async void TesteFirebase()
        {
            var firebase = new FirebaseClient("https://inventorypd3.firebaseio.com/");
            Dinosaur teste = new Dinosaur();
            teste.nome = "Rodrigo";
            teste.Height = 1.80;

            var dino = await firebase
                  .Child("inventorypd3")
                  .PostAsync(teste);

        }

        
    }
}