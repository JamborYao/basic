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
           // string token = GetAToken();
            string requestUri = "https://manage.windowsazure.cn/subscriptions/cbfce2e1-1ab1-4f95-a5a1-df8be530f290/providers/Microsoft.Commerce/UsageAggregates?api-version=2015-06-01-preview&reportedStartTime=2015-01-01T00%3a00%3a00%2b00%3a00&reportedEndTime=2015-09-23T00%3a00%3a00%2b00%3a00&aggreagationGranularity=Daily&showDetails=true";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string content = string.Empty;
            using (StreamReader sr = new StreamReader(responseStream))
            {
                content = sr.ReadToEnd();
            }
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