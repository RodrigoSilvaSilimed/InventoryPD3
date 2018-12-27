using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using InventoryPD3.Servico.Entidade;
using InventoryPD3.Servico.BLL;
using ZXing.Net.Mobile.Forms;



namespace InventoryPD3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            btn_Buscar_CEP.Clicked += BuscarCEP; //+= concatenar e atribuir a um método que eu quiser
            btn_ScanBarcode.Clicked += Scanner;

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
            catch(Exception e)
            {
                DisplayAlert("Erro Crítico", e.Message, "OK");
            }
        }


        public void Scanner(object sender, EventArgs args)
        {
            Scan();
        }

        public async void Scan()
        {
            var scanpage = new ZXingScannerPage();

            scanpage.OnScanResult += (result) => {
                // Parar de escanear
                scanpage.IsScanning = false;

                // Alert com o código escaneado
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    DisplayAlert("Código escaneado", result.Text, "OK");
                });
            };


            await Navigation.PushAsync(scanpage);


        }
    }
}
