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

using InventoryPD3.Servico.BLL;
using Android.Content.PM;

namespace InventoryPD3.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [
        IntentFilter
        (
            new[] { Intent.ActionView },
            Categories = new[]
            {
                Intent.CategoryDefault, Intent.CategoryBrowsable
            },
            DataSchemes = new[]
            {
                "com.googleusercontent.apps.718300855440-sn3fkaja1a96d03gp0vmcprt6jlmhkoo"
            },
            //DataSchemes = new[] { "com.googleusercontent.apps.723962257721-ql0tki3si3s22l1lsovimivkmnrfm6rr" },
            DataPaths = new[]
            {
                "/oauth2redirect"
            }
          )
        ]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Convert Android.Net.Url to Uri
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            AuthenticationState.Authenticator.OnPageLoading(uri);

            Finish();
        }
    }
}