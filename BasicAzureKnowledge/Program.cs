using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicAzureKnowledge
{
    class Program
    {
        static void Main(string[] args)
        {
            /*storage upload demo
            thread URL: http://forums.asp.net/t/2068198.aspx
            */
            //upload()
            /*azure async await demo*/
            //AsyncDemo. MyAsync1();
            // MyHttpGet();
            // StorageDemo test = new StorageDemo("test1","test2");


            //     Demo demo = new Demo();
            // // demo.upload();
            // //demo.AsyncMethod();
            //JsonDemo. JArrayTest();
            string key = "tgbo/iCw6KMVBSH1T7wrpT1bjoYtWJXGjYU/xnTKSbeg2uUlzelekbcfTrSH3KRGp+Gkwkfbnlhs7Pl2gKn9nw==";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=willshao;AccountKey="+key+"");

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference("test");
            CloudBlockBlob blob = container.GetBlockBlobReference("Capture.PNG");
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow;
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            string url = blob.Uri + sasBlobToken;
            SAS.GetContainerSasUri(container);

            // SAS. UploadBlobWithRestAPISasPermissionOnBlobContainer("http://willshao.blob.core.windows.net/test?sv=2014-02-14&sr=c&sig=X0GDpACdEQB53NmDvy6tQpkn0Nu9rNUjvTi3SnRnWxs%3D&st=2016-01-10T16%3A00%3A00Z&se=2016-01-18T16%3A00%3A00Z&sp=rw");
            Console.WriteLine("finished!");
            Console.ReadKey();
        }
      
        public static async void MyHttpGet() 
        {
            string url = "http://10.168.172.243:8080/ThreadsManagerService.svc/GetThreadsByNumber?num=1";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.UseDefaultCredentials = false;
            //request.Credentials = CredentialCache.DefaultNetworkCredentials;
            //request.Credentials = new NetworkCredential("v-jayao", "Change!13", "fareast.corp.microsoft.com");

            var response = request.GetResponseAsync();
            await response;
            Stream stream = response.Result.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
          //  httpContent.Text = content;


        }
       

    }

    public class Demo : AllDemoList
    {

    }
}
