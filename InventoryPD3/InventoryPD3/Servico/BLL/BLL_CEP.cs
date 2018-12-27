using System;
using System.Collections.Generic;
using System.Text;
using InventoryPD3.Servico.Entidade;
using System.Net;
using Newtonsoft.Json;


namespace InventoryPD3.Servico.BLL
{
    class BLL_CEP
    {
        private static string EnderecoURL = "http://viacep.com.br/ws/{0}/json/";//{0} parâmetro 0

        public static Entidade_CEP BuscaCEP(string CEP)
        {
            string NovoEnderecoURL = string.Format(EnderecoURL, CEP);

            WebClient WB = new WebClient();

            string Conteudo = WB.DownloadString(NovoEnderecoURL);

            Entidade_CEP end = JsonConvert.DeserializeObject<Entidade_CEP>(Conteudo); //Deserializar a string json em formato Entidade CEP

            if (end.cep == null)
            {
                return null;
            }
            else
            {
                return end;
            }

        }
    }
}
