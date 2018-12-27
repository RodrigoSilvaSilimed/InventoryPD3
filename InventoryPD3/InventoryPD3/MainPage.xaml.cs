using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using InventoryPD3.Servico.Entidade;
using InventoryPD3.Servico.BLL;
using ZXing.Net.Mobile.Forms;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace InventoryPD3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            btn_Buscar_CEP.Clicked += BuscarCEP; //+= concatenar e atribuir a um método que eu quiser
            btn_ScanBarcode.Clicked += Scanner;
            btn_Foto.Clicked += Camera;

        }

        private void BuscarCEP(object sender, EventArgs args) //para ser um event handler preciso de dois parâmetros, um é um objeto e o outro Event args
        {
            //TODO lógica 

            //TODO validações

            //TODO Busca na internet

            //TODO apresenta os dados
            string cep = ttb_CEP.Text.Trim();
            Entidade_CEP end = BLL_CEP.BuscaCEP(cep);
            try
            {
                if (end != null)
                {
                    lb_Resultado.Text = string.Format("Endereço: {0}, {1}, {2} - {3}, CEP: {4}", end.logradouro, end.bairro, end.localidade, end.uf, end.cep);
                }
                else
                {
                    DisplayAlert("Alerta", "Cep Inválido", "OK");
                }

            }
            catch (Exception e)
            {
                DisplayAlert("Erro Crítico", e.Message, "OK");
            }
        }


        public void Scanner(object sender, EventArgs args)
        {
            Scan();

        }

        public void Camera(object sender, EventArgs args)
        {
            TakeCam2();

        }

        public async void Scan()
        {
            //https://julianocustodio.com/2017/11/03/scanner/
            var scanpage = new ZXingScannerPage();

            scanpage.OnScanResult += (result) =>
            {
                // Parar de escanear
                scanpage.IsScanning = false;

                // Alert com o código escaneado
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    DisplayAlert("Código escaneado", result.Text, "OK");
                });
            };


            await Navigation.PushAsync(scanpage);


        }

        public async void TakeCam()
        {
            


            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)

                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        }
        public async void TakeCam2()
        {

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            var GPSStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);
            

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }
                
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

                    Directory = "Pictures",
                    Name = "test.jpg"
                });


                if (file == null)
                    return;

                await DisplayAlert("Localização do Arquivo", file.Path, "OK");

                PhotoImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            else
            {
                await DisplayAlert("Permissão Negada", "Não é possível tirar fotos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }
        }
    }
}
