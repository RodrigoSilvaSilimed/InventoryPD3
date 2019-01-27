using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InventoryPD3.Servico.DAL;
using InventoryPD3.Servico.Entidade;

namespace Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LeiturasPage : ContentPage
	{
        List<Entidade_Leitura> Lista { get; set; }

        public LeiturasPage ()
		{
			InitializeComponent ();
            ConsultarLeituras();
        }

        private void ConsultarLeituras()
        {
            DAL_Database database = new DAL_Database();
            Lista = database.Consultar();

            ListaLeituras.ItemsSource = Lista;
            lblCount.Text = Lista.Count.ToString();

            //Lista = database.Pesquisa()
            lblCountSync.Text = Lista.Count(a => a.Synced == false).ToString();
        }

        private void PesquisarAction(object sender, TextChangedEventArgs args)
        {
            ListaLeituras.ItemsSource = Lista.Where(a => a.CodigoBarras.Contains(args.NewTextValue)).ToList(); // args.NewTextValue captura a última informação digitada. onde a é igual a.nomevaga
        }

        private async void ExcluirAction(object sender, EventArgs args)
        {
            bool x = await DisplayAlert("Atenção!", "Tem certeza que deseja excluir o registro?", "Sim", "Não");

            if (x)
            {
                Label lblExcluir = (Label)sender; //Instanicei uma label para receber a label da interface (XAML). Desejo com isso pegar o Binding do Command Parameter que está dentro do GetureRecognizer da Label
                                                  //Vagas vaga = ((TapGestureRecognizer)lblDetalhe.GestureRecognizers[0]).CommandParameter as Vagas;//Gesturerecognizer é uma lista. Vou acessar o primeiro elemento da lista que é o tap. Faço uma conversão

                TapGestureRecognizer tapGest = (TapGestureRecognizer)lblExcluir.GestureRecognizers[0]; //0 é o campo da vaga
                Entidade_Leitura leitura;
                leitura = tapGest.CommandParameter as Entidade_Leitura;

                DAL_Database database = new DAL_Database();
                database.Exclusao(leitura);
                ConsultarLeituras();
            }
        }
    }
}