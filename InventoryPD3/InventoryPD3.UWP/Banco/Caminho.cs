using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InventoryPD3.Servico.DAL;
using Xamarin.Forms;
using System.IO;
using InventoryPD3.UWP.Banco;
using Windows.Storage;

[assembly: Dependency(typeof(Caminho))]
namespace InventoryPD3.UWP.Banco
{
    class Caminho : ICaminho
    {
        public string GetCaminho(string NomeArquivoBanco)
        {
            var caminhoDaPasta = ApplicationData.Current.LocalFolder.Path;

            string caminhoBanco = Path.Combine(caminhoDaPasta, NomeArquivoBanco);

            return caminhoBanco;

        }
    }
}
