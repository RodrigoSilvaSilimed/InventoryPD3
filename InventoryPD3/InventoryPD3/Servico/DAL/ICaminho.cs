using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryPD3.Servico.DAL
{
    public interface ICaminho
    {
        string GetCaminho(string NomeArquivoBanco);
    }
}
