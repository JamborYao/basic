using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.Storage;
using Microsoft.WindowsAzure.Management.Storage.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
              "ed0caab7-c6d4-45e9-9289-c7e5997c9241", token);
            var x509Certificate2 = new X509Certificate2();
            CertificateCloudCredentials cer = new CertificateCloudCredentials("ed0caab7-c6d4-45e9-9289-c7e5997c9241", x509Certificate2);
            // CreateVirtualMachine(credential).Wait();
            CreateResources(credential);
            Console.ReadLine();
        }
        public async static void CreateResources(
  TokenCloudCredentials credential)
        {
            Console.WriteLine(
              "Creating the storage account. This may take a few minutes...");
            var stResult =
              await CreateStorageAccount(credential);
            Console.WriteLine(stResult);
        }
        public async static Task<string> CreateStorageAccount(
  SubscriptionCloudCredentials credentials)
        {
            using (var storageClient = new StorageManagementClient(credentials))
            {
                try
                {
                    await storageClient.StorageAccounts.CreateAsync(
                      new StorageAccountCreateParameters
                      {
                      // GeoReplicationEnabled = false,
                      Label = "Sample Storage Account",
                          Location = LocationNames.WestUS,
                          Name = "mbjastorage"
                      });
                }
                catch (Exception e)
                { }
            }
            return "Successfully created account.";
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
