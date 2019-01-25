using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Banco;
using InventoryPD3.Droid.Banco;
using Xamarin.Forms;

[assembly: Dependency(typeof(Caminho))]
namespace InventoryPD3.Droid.Banco
{
    class Caminho : ICaminho
    {
        public string ObterCaminho(string NomeArquivoBanco)
        {
            var caminhoDaPasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            string caminhoBanco = Path.Combine(caminhoDaPasta, NomeArquivoBanco);

            return caminhoBanco;
        }
    }
}