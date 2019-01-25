using System;
using System.Collections.Generic;
using System.Text;
using SQLite.Net.Attributes;

namespace Servico.Entidade
{
    public class Inventario
    {
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
        //return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}",Cliente,Data,CodigoBarras,TimestampLeitura,GPSLatitude,GPSLongitude,GPSTimestamp,GPSAccuracy,Usuario,CaminhoImg,TimestampFoto,urlImg 

    }
}
