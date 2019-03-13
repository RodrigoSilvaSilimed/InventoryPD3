﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms; //Para usar o ExportRenderer para ter uma renderização customizada
using Xamarin.Forms.Platform.iOS; //para minha página de login herdar de pagerenderer
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;
using InventoryPD3.iOS;
using InventoryPD3.Servico.Entidade;
using Pages; //para pegar o typeoff LoginFacebookPage

[assembly: ExportRenderer(typeof(LoginFacebookPage), typeof(LoginFacebookRenderer))] //para isso usei oXamarin.Forms

namespace InventoryPD3.iOS
{
    class LoginFacebookRenderer : PageRenderer
    {
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //Usando o OAuth


            var OauthFacebook = new OAuth2Authenticator(
            clientId: "776007559440419", //id do aplicativo do https://developers.facebook.com/apps/776007559440419/dashboard/  
            scope: "email",//escopo vazio tem informações básicas de login mas posso fazer uma lista separada por vírgulas "public_profile,email,"
            authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"), // These values do not need changing  
            //authorizeUrl: new Uri("https://www.facebook.com/v3.2/dialog/oauth"), // These values do not need changing  
            redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html")// These values do not need changing  
            );
            
            //API do Xamarin.iOS
            OauthFacebook.Completed += async (sender, args) =>
            {
                DismissViewController(true, null);

                if (args.IsAuthenticated)
                {
                    //acesso aos dados - Token de acesso

                    var token = args.Account.Properties["access_token"].ToString();

                    var requisicao = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=name,email"), null, args.Account); // na uri uso as opções de https://developers.facebook.com/docs/graph-api/overview
                    var resposta = await requisicao.GetResponseAsync();

                    //dynamic obj = JsonConvert.DeserializeObject<JObject>(resposta.GetResponseText()); //tipo dynamic para ter liberdade para acessar a informação
                    var obj = JsonConvert.DeserializeObject<Entidade_Login>(resposta.GetResponseText().ToString()); //tipo dynamic para ter liberdade para acessar a informação

                    App.NavegarParaInciar(obj);

                }

            };

            //analofo ao viewcontroller do Android
            PresentViewController(OauthFacebook.GetUI(), true, null);
        }
    }
}