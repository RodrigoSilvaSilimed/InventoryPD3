using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using InventoryPD3.Servico.Entidade;
using Xamarin.Forms;

namespace InventoryPD3.Servico.DAL
{


    public class DAL_Database
    {
        private SQLiteConnection _conexao;

        public DAL_Database ()
        {
            var dep = DependencyService.Get<ICaminho>(); //Uso o dependency service para solicitar a interface o caminho do banco. A interface será implementada de forma particular em cada projeto
            string caminho = dep.GetCaminho("database.sqlite");

            _conexao = new SQLiteConnection(caminho);
            _conexao.CreateTable<Entidade_Leitura>();
        }    

        public List<Entidade_Leitura> Consultar()
        {
            return _conexao.Table<Entidade_Leitura>().ToList();
        }

        public List<Entidade_Leitura> Consultar(string inventario)
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => a.Data == inventario).ToList();
        }

        public List<Entidade_Leitura> ConsultarLeiturasParaSincronizar()
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => a.Synced == false).ToList();
        }

        public List<Entidade_Leitura> Pesquisa(string CodigoBarras)
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => a.CodigoBarras == CodigoBarras).ToList();
        }

        public Entidade_Leitura ObterVagaPorCodigoBarras(string CodigoBarras)
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => a.CodigoBarras == CodigoBarras).FirstOrDefault();
        }

        public void Cadastro(Entidade_Leitura leitura)
        {
            _conexao.Insert(leitura);
        }

        public void Atualizacao(Entidade_Leitura leitura)
        {
            _conexao.Update(leitura);
        }

        public void Exclusao(Entidade_Leitura leitura)
        {
            _conexao.Delete(leitura);
        }
    }
}
