using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Geolocator;
using InventoryPD3.Servico.Entidade;

namespace Servico.BLL
{
    public class BLL_InventoryPD3
    {
        Entidade_GPS posicao = new Entidade_GPS();

        private async void GetPosition()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
          
            posicao.acuracidade = position.Accuracy.ToString();
            posicao.altitude = position.Altitude.ToString();
            posicao.latitude = string.Format("{0:0.0000000}", position.Latitude);
            posicao.longitude = string.Format("{0:0.0000000}", position.Longitude);
            posicao.norte = position.Heading.ToString();
            posicao.velocidade = position.Speed.ToString();
            posicao.timestamp = position.Timestamp.ToString();

        }

    }
}
