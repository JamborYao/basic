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
        static void Main(string[] args)
        {
            //var token = GetAuthorizationHeader();
            //var credential = new TokenCloudCredentials(
            //  "ed0caab7-c6d4-45e9-9289-c7e5997c9241", token);

            //   CreateVM(credential);
            #region
            var x509Certificate2 = new X509Certificate2(@"D:/aktest.cer");
            CertificateCloudCredentials cer = new CertificateCloudCredentials("ed0caab7-c6d4-45e9-9289-c7e5997c9241", x509Certificate2);
            // CreateResources(credential);
            //var storageClient=CloudContext.Clients.CreateStorageManagementClient(cer);
            //storageClient.StorageAccounts.Create(new StorageAccountCreateParameters
            //{
            //    AccountType= "Standard_LRS",
            //    // GeoReplicationEnabled = false,
            //    Label = "Sample Storage Account",
            //    Location = "East US",
            //    Name = "mbjastorage"
            //});


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
    
            var vmClient = CloudContext.Clients.CreateComputeManagementClient(cer);
            var operatingSystemImageListResult = vmClient.VirtualMachineOSImages.List();
            var imageName =
                    operatingSystemImageListResult.Images.FirstOrDefault(
                      x => x.Label.Contains(
                        "SQL Server 2014 RTM Standard on Windows Server 2012 R2")).Name;

            var networkConfigSet = new ConfigurationSet
            {
                ConfigurationSetType = "NetworkConfiguration",
                InputEndpoints = new List<InputEndpoint>
  {
    new InputEndpoint
    {
      Name = "PowerShell",
      LocalPort = 5986,
      Protocol = "tcp",
      Port = 5986,
    },
    new InputEndpoint
    {
      Name = "Remote Desktop",
      LocalPort = 3389,
      Protocol = "tcp",
      Port = 3389,
    }
  }
            };
            var vhd = new OSVirtualHardDisk
            {
                SourceImageName = imageName,
                HostCaching = VirtualHardDiskHostCaching.ReadWrite,
                MediaLink = new Uri(string.Format(
    "https://{0}.blob.core.windows.net/vhds/{1}.vhd",
      "willshao", imageName))
            };
            var deploymentAttributes = new Role
            {
                RoleName = "libraryvm01",
                RoleSize = VirtualMachineRoleSize.Small,
                RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                OSVirtualHardDisk = vhd,
                ConfigurationSets = new List<ConfigurationSet>
  {
    windowsConfigSet,
    networkConfigSet
  },
                ProvisionGuestAgent = true
            };
            var createDeploymentParameters =
   new VirtualMachineCreateDeploymentParameters
   {
       Name = "libraryvm01",
       Label = "libraryvm01",
       DeploymentSlot = DeploymentSlot.Production,
       Roles = new List<Role> { deploymentAttributes }
   };

            try
            {

            var mymachine=    vmClient.VirtualMachines.Get("javmtest", "javmtest", "javmtest");
       vmClient.VirtualMachines.CreateDeploymentAsync(
       "libraryvm01",
        createDeploymentParameters);
            }
            catch (Exception e)
            { }
            #endregion
            Console.ReadLine();


        }
        public async static void CreateVM(TokenCloudCredentials credential)
        {
            Console.WriteLine("creating...");
            var stResult = await CreateVirtualMachine(credential);
            Console.WriteLine(stResult);
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
                  "2ec9eead-19e6-455e-a921-791eda349332",
                  new Uri("http://localhost/createvm"));
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
            using (var storageClient = new StorageManagementClient(credentials))
            {
                try
                {
                    storageClient.StorageAccounts.Create(new StorageAccountCreateParameters
                    {
                        AccountType = "Standard_LRS",
                        // GeoReplicationEnabled = false,
                        Label = "Sample Storage Account",
                        Location = "East US",
                        Name = "mbjastorage"
                    });
                }
                catch (Exception e)
                { }
            }
            using (var computeClient = new ComputeManagementClient(credentials))
            {
                try
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return "Successfully created Virtual Machine";
        }
    }

}
