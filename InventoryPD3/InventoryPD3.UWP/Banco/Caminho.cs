using Banco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using InventoryPD3.UWP.Banco;
using Xamarin.Forms;

[assembly: Dependency(typeof(Caminho))]
namespace InventoryPD3.UWP.Banco
{
    class Caminho: ICaminho
    {
        public string ObterCaminho(string NomeArquivoBanco)
        {
            var caminhoDaPasta = ApplicationData.Current.LocalFolder.Path;
            string caminhoBanco = Path.Combine(caminhoDaPasta, NomeArquivoBanco);

            return caminhoBanco;
        }
    }
}
