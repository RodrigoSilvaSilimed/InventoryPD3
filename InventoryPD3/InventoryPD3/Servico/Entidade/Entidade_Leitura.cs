using System;
using System.Collections.Generic;
using System.Text;
using SQLite; //para colocar os dados das tabelas que serão criadas no _conexao.CreateTable<>(Vagas);


namespace InventoryPD3.Servico.Entidade
{
    [Table("Leitura")]
    public class Entidade_Leitura
    {
        //[PrimaryKey, AutoIncrement]//notation
        [PrimaryKey]//notation
        public string CodigoBarras { get; set; }
        //public int Id { get; set; }
        public string Cliente { get; set; }
        public string Data { get; set; }        
        public string TimestampLeitura { get; set; }
        public string GPSLatitude { get; set; }
        public string GPSLongitude { get; set; }
        public string GPSTimestamp { get; set; }
        public string GPSAccuracy { get; set; }
        public string Usuario { get; set; }
        public bool   Synced { get; set; }
        public string CaminhoImg { get; set; }   
        public string TimestampFoto { get; set; }
        public string urlImg { get; set; }
    }
}
