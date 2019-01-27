using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using System.IO;
using InventoryPD3.iOS.Banco;
using InventoryPD3.Servico.DAL;

namespace InventoryPD3.iOS.Banco
{
    class Caminho : ICaminho
    {
        public string GetCaminho(string NomeArquivoBanco)
        {
            var caminhoDaPasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            string caminhoDaBiblioteca = Path.Combine(caminhoDaPasta, "..", "Library");//uso o .. para descer um nível e usar a pasta Library

            string caminhoBanco = Path.Combine(caminhoDaBiblioteca, NomeArquivoBanco);

            return caminhoBanco;

        }
    }
}