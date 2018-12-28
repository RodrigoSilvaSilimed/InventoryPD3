using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryPD3.Servico.Entidade
{
    public class Entidade_GPS
    {
        /// <summary>
        /// Tipo GPS
        /// </summary>
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string altitude { get; set; }
        public string velocidade { get; set; }
        public string acuracidade { get; set; }
        public string norte { get; set; }
        public string timestamp { get; set; }
    }
}
