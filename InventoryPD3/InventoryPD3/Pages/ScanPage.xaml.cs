using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using InventoryPD3.Servico.Entidade;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanPage : ContentPage
	{
        //variáveis globais
        private string Cliente;
        private string Usuario;
        private string Inventario;
        private bool statusWifi=false;
        private bool statusGPS=false; //stats do GPS. Encontrado = true
        private Plugin.Geolocator.Abstractions.Position position; //última posição obtida
        private Control.Control Controle;

        public ScanPage ()
		{
			InitializeComponent ();
            AtualizaInventario();
            TimerGPSWifi();
            Controle = new Control.Control();
            Cliente = "999999";
            Usuario = "rsilva@silimed.com.br";
            Inventario = "190001";
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
       
        public async void Scan(Entidade_Leitura leitura)
        {
            string resultado="";
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
                    
                    if (Controle.ValidaBarcodeSN(result.Text))
                    {
                        leitura.CodigoBarras = result.Text;
                        leitura.TimestampLeitura = DateTime.Now.ToString();
                        leitura.GPSAccuracy = position.Accuracy.ToString();
                        leitura.GPSLatitude = position.Latitude.ToString();
                        leitura.GPSLongitude = position.Longitude.ToString();
                        leitura.GPSTimestamp = position.Timestamp.ToString();
                        TakeCam(leitura);
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
            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
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

                
                //Envio para o FirebaseStorage
                // Get any Stream - it can be FileStream, MemoryStream or any other type of Stream
                //var stream = File.Open(@"C:\Users\you\file.png", FileMode.Open);
                var stream2 = file.GetStream();
                leitura.TimestampLeitura = DateTime.Now.ToString();
                // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
                var task = new FirebaseStorage("inventorypd3.appspot.com")
                       .Child(leitura.Cliente)
                       .Child(DateTime.Now.Year.ToString()+ DateTime.Now.Month.ToString("00"))
                       .Child(leitura.CodigoBarras + ".jpg")
                       .PutAsync(stream2);

                file.Dispose();
                // Track progress of the upload
                //task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                // await the task to wait until upload completes and get the download url
                //task.Progress.ProgressChanged += (s, e) => DisplayAlert("Progresso", e.Percentage.ToString("00"), "Ok");
                var downloadUrl = await task;
                
                leitura.urlImg = downloadUrl;
                //lb_Resultado.Text = downloadUrl.ToString();
                SendToFirebase(leitura);
            }
            else
            {
                await DisplayAlert("Permissão Negada", "Não é possível tirar fotos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }
        }

        public async void SendToFirebase(Entidade_Leitura leitura)
        {
            //Firebase https://rsamorim.azurewebsites.net/2017/11/07/reagindo-a-evento-com-xamarin-forms-e-firebase/
            //doc git https://github.com/step-up-labs/firebase-database-dotnet
            var firebase = new FirebaseClient("https://inventorypd3.firebaseio.com/");

            // add new item to list of data and let the client generate new key for you (done offline)
            /*var dino = await firebase
              .Child("inventorypd3")
              .PostAsync("nome:rodrigo");
              */
            /*var dinos = await firebase
              .Child("dinosaurs")
              .OrderByKey()
              .StartAt("pterodactyl")
              .LimitToFirst(2)
              .OnceAsync<Dinosaur>();

            foreach (var dino in dinos)
            {
                lb_Resultado.Text = lb_Resultado.Text + $"{dino.Key} is {dino.Object.Height}m high.";
            }*/

            //var cliente = 
            await firebase
            .Child(leitura.Cliente)
            .Child(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00"))
            .Child(leitura.CodigoBarras)
            //.PostAsync(leitura);
            .PutAsync(leitura);


            /*Dinosaur meudino = new Dinosaur();

            meudino.nome = "Rodrigo";
            meudino.Height = 1.78;

            
            var dino = await firebase
              .Child("dinosaurs")
              .PostAsync(meudino); //Firebase cria a chave

            //lb_Resultado.Text = lb_Resultado.Text + $"{dino.Key} is {dino.Object.Height}m high.";

            await firebase
              .Child("dinosaurs")
              .Child("t-rex")
              .PutAsync(meudino);*/// Eu crio a chave

            // note that there is another overload for the PostAsync method which delegates the new key generation to the firebase server
            //Console.WriteLine($"Key for the new dinosaur: {dino.Key}");

            // add new item directly to the specified location (this will overwrite whatever data already exists at that location)
            /* await firebase
               .Child("dinosaurs")
               .Child("t-rex")
               .PutAsync("com overwrite");

             // delete given child node
             /*await firebase
               .Child("dinosaurs")
               .Child("t-rex")
               .DeleteAsync();*/

            /*posicao.acuracidade = position.Accuracy.ToString();
            //posicao.altitude = position.Altitude.ToString();
            posicao.latitude = string.Format("{0:0.0000000}", position.Latitude);
            posicao.longitude = string.Format("{0:0.0000000}", position.Longitude);
            posicao.norte = position.Heading.ToString();
            posicao.velocidade = position.Speed.ToString();
            posicao.timestamp = position.Timestamp.ToString();*/

            //lb_Resultado_Scan.Text = ("Posicao GPS: " + position.Latitude.ToString() + ", " + position.Longitude.ToString() + ". Hora GPS: " + position.Timestamp);

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

        private void AtualizaInventario()
        {
            lb_Inventario.Text = "Inventário " + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString("00");
            Inventario = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00");
        }
    }
}