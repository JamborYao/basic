using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementLB
{
    class Program
    {
         static  void Main(string[] args)
        {
            var token = GetAuthorizationHeader();
            var credential = new TokenCloudCredentials(
              "*********", token);
            CreateVirtualMachine(credential).Wait();
        }
        public async Task Create(SubscriptionCloudCredentials credential)
        {
            await CreateVirtualMachine(credential);
        }
        private static string GetAuthorizationHeader()
        {
            AuthenticationResult result = null;

            var context = new AuthenticationContext(string.Format(
              "https://login.windows.net/{0}",
              "e4162ad0-e9e3-4a16-bf40-0d8a906a06d4"));
            //https://graph.windows.net/e4162ad0-e9e3-4a16-bf40-0d8a906a06d4
            //https://login.microsoftonline.com/e4162ad0-e9e3-4a16-bf40-0d8a906a06d4/oauth2/token
            var thread = new Thread(() =>
            {
                result = context.AcquireToken(
                  "https://management.core.windows.net/",
                  "482cdabc-2e48-4594-b7f6-697836694479",
                  new Uri("https://localhost/"));
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = "AquireTokenThread";
            thread.Start();
            thread.Join();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            string token = result.AccessToken;
            return token;
        }
        public async static Task<string> CreateVirtualMachine(
  SubscriptionCloudCredentials credentials)
        {
            using (var computeClient = new ComputeManagementClient(credentials))
            {
                var operatingSystemImageListResult =
     await computeClient.VirtualMachineOSImages.ListAsync();
                var imageName =
                  operatingSystemImageListResult.Images.FirstOrDefault(
                    x => x.Label.Contains(
                      "Windows Server 2012 R2 Datacenter, March 2014")).Name;

                var windowsConfigSet = new ConfigurationSet
                {
                    ConfigurationSetType =
    ConfigurationSetTypes.WindowsProvisioningConfiguration,
                    AdminPassword =
    "123Test",
                    AdminUserName = "jambor",
                    ComputerName = "libraryvm01",
                    HostName = string.Format("{0}.cloudapp.net",
    "libraryvm01")
                };
            }
            return "Successfully created Virtual Machine";
        }
    }

}
