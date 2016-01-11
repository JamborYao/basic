using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasicAzureWeb
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        //public string GetAToken()
        //{
        //    try
        //    {
        //        //https://login.chinacloudapi.cn/fe857caa-4b6c-4e82-9133-596159076779/oauth2/authorize
        //        AuthenticationContext authenticationContext = new AuthenticationContext("https://login.chinacloudapi.cn/fe857caa-4b6c-4e82-9133-596159076779/oauth2/authorize");
        //        var result = authenticationContext.AcquireToken("https://graph.chinacloudapi.cn/", "370b1d4c-0a30-4a80-bf7b-3cf15ca9d76f", new Uri("http://huzc1234"));
        //        if (result == null)
        //        {
        //            throw new InvalidOperationException("Failed to obtain the JWT token");
        //        }
        //        return result.AccessToken;
        //    }
        //    catch (Exception)
        //    {
        //        //throw;
        //        return null;
        //    }
        //}
    }
}