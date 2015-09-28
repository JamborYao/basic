using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace BasicAzureMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string token = GetAToken();
            string requestUri = "https://manage.windowsazure.com/subscriptions/ed0caab7-c6d4-45e9-9289-c7e5997c9241/providers/Microsoft.Commerce/UsageAggregates?api-version=2015-06-01-preview&reportedStartTime=2015-01-01T00%3a00%3a00%2b00%3a00&reportedEndTime=2015-09-23T00%3a00%3a00%2b00%3a00&aggreagationGranularity=Daily&showDetails=true";
            requestUri = "https://management.azure.com/subscriptions/ed0caab7-c6d4-45e9-9289-c7e5997c9241/providers/Microsoft.Commerce/UsageAggregates?api-version=2015-06-01-preview&reportedstartTime=2015-03-01+00%3a00%3a00Z&reportedEndTime=2015-05-18+00%3a00%3a00Z";


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
          //  request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string content = string.Empty;
            using (StreamReader sr = new StreamReader(responseStream))
            {
                content = sr.ReadToEnd();
            }


            return View();
        }
        public string GetAToken()
        {
            try
            {
                //https://login.chinacloudapi.cn/ed0caab7-c6d4-45e9-9289-c7e5997c9241/oauth2/authorize  
                //subscription id = ed0caab7-c6d4-45e9-9289-c7e5997c9241
                //https://graph.windows.net/e4162ad0-e9e3-4a16-bf40-0d8a906a06d4  

                //ClientCredential 
                AuthenticationContext authenticationContext = new AuthenticationContext("https://login.windows.net/e4162ad0-e9e3-4a16-bf40-0d8a906a06d4");
                AuthenticationResult result = null;
                var thread = new Thread(() =>
                {
                        result = authenticationContext.AcquireToken(
                        resource: "https://management.core.windows.net/", clientId: "06bbd520-87fb-4550-9b94-f6b68a858452", redirectUri: new Uri("https://localhost:44300/"));
                    
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Name = "AquireTokenThread";
                thread.Start();
                thread.Join();
               
                string token = result.AccessToken;
                return token;
            }
            catch (Exception e)
            {

                //throw;
                return null;
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}