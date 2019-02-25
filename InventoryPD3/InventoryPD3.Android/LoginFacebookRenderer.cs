using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms; //Para usar o ExportRenderer para ter uma renderização customizada
using Xamarin.Forms.Platform.Android; //para minha página de login herdar de pagerenderer
using Newtonsoft.Json;
using Xamarin.Auth;
using InventoryPD3.Droid;
using Pages; //para pegar o typeoff LoginFacebookPage

[assembly: ExportRenderer(typeof(LoginFacebookPage), typeof(LoginFacebookRenderer))] //para isso usei oXamarin.Forms
namespace InventoryPD3.Droid
{
    
    public class LoginFacebookRenderer : PageRenderer
    {
        public LoginFacebookRenderer()
        {
            //Usando o OAuth
            var OauthFacebook = new OAuth2Authenticator(
            clientId: "776007559440419", //id do aplicativo do https://developers.facebook.com/apps/776007559440419/dashboard/  
            scope: "email",//escopo vazio tem informações básicas de login mas posso fazer uma lista separada por vírgulas "public_profile,email,"
            authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"), // These values do not need changing  
            redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html")// These values do not need changing  
            );

            //API do Xamarin.Android
            OauthFacebook.Completed += async (sender, args) =>
            {
                if (args.IsAuthenticated)
                {
                    //acesso aos dados - Token de acesso

                    string token = args.Account.Properties["access_token"].ToString();

                    var requisicao = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=name,email"), null, args.Account); // na uri uso as opções de https://developers.facebook.com/docs/graph-api/overview
                    var resposta = await requisicao.GetResponseAsync();

                    dynamic obj = JsonConvert.DeserializeObject(resposta.GetResponseText()); //tipo dynamic para ter liberdade para acessar a informação
                    var Nome = obj.name.ToString();
                    var email = obj.email.ToString();
                }
                
            };
            var activity = this.Context as Activity; //activity são telas do Android
            activity.StartActivity(OauthFacebook.GetUI(activity)); //Chamar a activity, nesse caso toda a tela de login
        }


    }
}