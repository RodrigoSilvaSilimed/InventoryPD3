using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using System.IO;
using InventoryPD3.Droid.Banco;
using InventoryPD3.Servico.DAL;

[assembly: Dependency(typeof(Caminho))]

namespace InventoryPD3.Droid.Banco
{
    class Caminho : ICaminho
    {
        public string GetCaminho(string NomeArquivoBanco)
        {
            //no Android costuma ser /data/user/0/br.com.silimed.InventoryPD3/files/database.sqlite
            var caminhoDaPasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string caminhoBanco = Path.Combine(caminhoDaPasta, NomeArquivoBanco);
            return caminhoBanco;
        }
    }
}