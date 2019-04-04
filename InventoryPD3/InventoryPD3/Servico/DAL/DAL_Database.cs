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
            //var i = _conexao.DropTable<Entidade_Leitura>();//apagar a tabela. Uso quando a
            _conexao.CreateTable<Entidade_Leitura>();
           
        
            //Query("delete Entidade_Leitura ");
        }       

        public List<Entidade_Leitura> Consultar(string inventario, string cliente)
        {
            //return _conexao.Table<Entidade_Leitura>().ToList();
            return _conexao.Table<Entidade_Leitura>().Where(a => (a.Data == inventario && a.Cliente==cliente)).ToList();
        }

        public List<Entidade_Leitura> ConsultarLeiturasParaSincronizar()
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => a.Synced == false).ToList();
        }

        public List<Entidade_Leitura> Pesquisa(string CodigoBarras, string inventario, string cliente)
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => (a.Data == inventario && a.Cliente == cliente && a.Barcode == CodigoBarras)).ToList();
        }

        public Entidade_Leitura ObterVagaPorCodigoBarras(string CodigoBarras)
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => (a.Barcode == CodigoBarras)).FirstOrDefault();
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

        public void Query(string query)
        {
            
            var returno = _conexao.Query<Entidade_Leitura>(query);
        }

        public void ApagarTabelaLeitura()
        {
            _conexao.DropTable<Entidade_Leitura>();
            _conexao.CreateTable<Entidade_Leitura>();
        }
        public void ApagarLeituraCliente(string cliente)
        {
            var lista = _conexao.Table<Entidade_Leitura>().Where(a => (a.Cliente == cliente)).ToList();
            lista.ForEach(delegate (Entidade_Leitura Leitura)
            {
                _conexao.Delete(Leitura);

            });
            
        }
    }
}
