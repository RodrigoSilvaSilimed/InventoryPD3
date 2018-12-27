using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.CurrentActivity;

namespace InventoryPD3.Droid
{
    [Activity(Label = "InventoryPD3", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected  override void OnCreate(Bundle savedInstanceState) //precisei transdformar o método em async por causa do await da câmera
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            //await CrossMedia.Current.Initialize();//Adicionei por causa da Câmera
            CrossCurrentActivity.Current.Init(this, savedInstanceState); // Sem essa linha o app não pede permissão erro: "Plugin.Media.Abstractions.MediaPermissionException: Camera permission(s) are required."

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

       public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults) //https://julianocustodio.com/2017/11/03/scanner/
        {
            //ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);// permissão da Câmera
          
        }

       

    }
}