using System;
using System.Collections.Generic;
using System.Text;
using SQLite; //para colocar os dados das tabelas que serão criadas no _conexao.CreateTable<>(Vagas);

namespace InventoryPD3.Servico.Entidade
{
    [Table("Leitura_Sync")]
    class Entidade_Leitura_Sync : Entidade.Entidade_Leitura
    {
        //Classe criada para alimentar a tabela de Leituras à enviar para o Banco na Nuvem
    }
}
