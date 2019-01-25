using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Banco;
using Foundation;
using UIKit;
using InventoryPD3.iOS.Banco;
using Xamarin.Forms;

[assembly: Dependency(typeof(Caminho))]
namespace InventoryPD3.iOS.Banco
{
    class Caminho: ICaminho
    {
        public string ObterCaminho(string NomeArquivoBanco)
        {

            var caminhoDaPasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string caminhoDaBiblioteca = Path.Combine(caminhoDaPasta, "..", "Library");

            string caminhoBanco = Path.Combine(caminhoDaBiblioteca, NomeArquivoBanco);

            return caminhoBanco;
        }
    }
}