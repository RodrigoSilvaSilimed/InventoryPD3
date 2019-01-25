﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace InventoryPD3.Servico.Entidade
{
    [Table("Leitura")]
    public class Entidade_Leitura
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Data { get; set; }
        public string CodigoBarras { get; set; }
        public string TimestampLeitura { get; set; }
        public string GPSLatitude { get; set; }
        public string GPSLongitude { get; set; }
        public string GPSTimestamp { get; set; }
        public string GPSAccuracy { get; set; }
        public string Usuario { get; set; }
        public string CaminhoImg { get; set; }   
        public string TimestampFoto { get; set; }
        public string urlImg { get; set; }
    }
}
