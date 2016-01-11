using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BasicAzureKnowledge
{
    public class SAS
    {
        public static void UploadBlobWithRestAPISasPermissionOnBlobContainer(string blobContainerSasUri)
        {
            string blobName = "sample.txt";
            string sampleContent = "This is sample text.";
            int contentLength = Encoding.UTF8.GetByteCount(sampleContent);
            string queryString = (new Uri(blobContainerSasUri)).Query;
            string blobContainerUri = blobContainerSasUri.Split('?')[0];
            string requestUri = string.Format(CultureInfo.InvariantCulture, "{0}/{1}{2}", blobContainerUri, blobName, queryString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "PUT";
            request.Headers.Add("x-ms-blob-type", "BlockBlob");
            request.ContentLength = contentLength;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(Encoding.UTF8.GetBytes(sampleContent), 0, contentLength);
            }
            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {

            }
        }

        public static string GenerateSAS()
        {
            string sas = "";
            DateTime start = DateTime.UtcNow;
            DateTime end = DateTime.UtcNow.AddDays(1);
            string accountName = "willshao";
            string accountKey = "tgbo/iCw6KMVBSH1T7wrpT1bjoYtWJXGjYU/xnTKSbeg2uUlzelekbcfTrSH3KRGp+Gkwkfbnlhs7Pl2gKn9nw==";
            string signedpermissions = "r";
            string signedstart = DateTime.UtcNow.ToString("O");
            string signedexpiry = DateTime.UtcNow.AddDays(1).ToString("O");
            string canonicalizedresource = "blob/"+accountName+"/test/Capture.PNG";
            string signedidentifier = "";
            string signedversion = "2015-04-05";
            string rscc = "";
            string rscd = "file; attachment";
            string rsce = "";
            string rscl = "";
            string rsct = "binary";


            string StringToSign = signedpermissions + "\n" +
               signedstart + "\n" +
               signedexpiry + "\n" +
               canonicalizedresource + "\n" +
               signedidentifier + "\n" +
               "" + "\n" +
               "" + "\n" +
               signedversion + "\n" +
               rscc + "\n" +
               rscd + "\n" +
               rsce + "\n" +
               rscl + "\n" +
               rsct;
            byte[] SignatureBytes = System.Text.Encoding.UTF8.GetBytes(StringToSign);
            System.Security.Cryptography.HMACSHA256 SHA256 = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(accountKey));
            string sig = Convert.ToBase64String(SHA256.ComputeHash(SignatureBytes));

            string sasURL = string.Format("http://{0}.blob.core.windows.net/test/Capture.PNG?sv={1}&sr={2}&sig={3}&st={4}&se={5}&sp={6}&rscd={7}&rsct={8}",
                 HttpUtility.UrlEncode(accountName),
                HttpUtility.UrlEncode(signedversion),
                HttpUtility.UrlEncode("b"),
                HttpUtility.UrlEncode(sig),
                HttpUtility.UrlEncode(signedstart),
                HttpUtility.UrlEncode(signedexpiry),
                HttpUtility.UrlEncode(signedpermissions),
                  HttpUtility.UrlEncode(rscd),
                HttpUtility.UrlEncode(rsct)
                );


            return sas;
        }

        public static string GetContainerSasUri(CloudBlobContainer container)
        {
            GenerateSAS();
            //Set the expiry time and permissions for the container.
            //In this case no start time is specified, so the shared access signature becomes valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List;

            //Generate the shared access signature on the container, setting the constraints directly on the signature.
            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return container.Uri + sasContainerToken;
        }
    }
}
