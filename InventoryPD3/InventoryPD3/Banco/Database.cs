using InventoryPD3.Servico.Entidade;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Banco
{
    class Database
    {
        private SQLiteConnection _conexao;

        public Database()
        {
            var dep = DependencyService.Get<ICaminho>();
            string caminho = dep.ObterCaminho("database.sqlite");

            _conexao = new SQLiteConnection(caminho);
            _conexao.CreateTable<Entidade_Leitura>();

        }

        public List<Entidade_Leitura> consultar()
        {
            return _conexao.Table<Entidade_Leitura>().ToList();

        }

        public Entidade_Leitura Pesquisar(string palavra)
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => a.CodigoBarras.Contains(palavra)).FirstOrDefault();

        }

        public Entidade_Leitura ObterVagaPorId(int id)
        {
            return _conexao.Table<Entidade_Leitura>().Where(a => a.Id == id).FirstOrDefault();

        }

        public void Cadastro(Entidade_Leitura leitura)
        {
            _conexao.Insert(leitura);
        }

        public void Atualização(Entidade_Leitura leitura)
        {
            _conexao.Update(leitura);
        }

        public void Exclusao(Entidade_Leitura leitura)
        {
            _conexao.Delete(leitura);
        }
    }
}
