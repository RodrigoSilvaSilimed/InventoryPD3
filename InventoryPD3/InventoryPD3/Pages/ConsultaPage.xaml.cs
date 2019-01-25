using Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConsultaPage : ContentPage
	{
		public ConsultaPage ()
		{
			InitializeComponent();

            Database database = new Database();

            ListaLeitura.ItemsSource = database.consultar();

            var Lista = database.consultar();



        }
	}
}