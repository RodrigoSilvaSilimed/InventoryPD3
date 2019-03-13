using System;
using System.Collections.Generic;
using System.Text;

namespace  InventoryPD3.Servico.BLL
{
    public class Constants
    {
        public static string AppName = "InventoryPD3";

        // OAuth
        // For Google login, configure at https://console.developers.google.com/
        public static string iOSClientId = "718300855440-6ql247lmjco7fsdmncd100adglhadlqp.apps.googleusercontent.com";
        public static string AndroidClientId = "718300855440-sn3fkaja1a96d03gp0vmcprt6jlmhkoo.apps.googleusercontent.com";

        // These values do not need changing
        //public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string Scope = "https://www.googleapis.com/auth/plus.login https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string iOSRedirectUrl = "com.googleusercontent.apps.718300855440-6ql247lmjco7fsdmncd100adglhadlqp:/oauth2redirect";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.718300855440-sn3fkaja1a96d03gp0vmcprt6jlmhkoo:/oauth2redirect";
    }
}
