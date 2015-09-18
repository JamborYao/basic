using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAzureKnowledge
{
    public class StorageDemo
    {
        private string _accountName = "willshao";
        private string _accountKey = "tgbo/iCw6KMVBSH1T7wrpT1bjoYtWJXGjYU/xnTKSbeg2uUlzelekbcfTrSH3KRGp+Gkwkfbnlhs7Pl2gKn9nw==";
        //private string _stroageConnection = string.Format("DefaultEndpointsProtocol = https; AccountName={0};AccountKey={1}");
        private string _stroageConnection {
            get { return string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", this._accountName, this._accountKey); }
        }
        CloudBlobClient blobClient = null;
        CloudBlobContainer _container = null;
        CloudBlockBlob _blockBlob = null;
        public string containerName { get; set; }
        public string blobName { get; set; }
        public string filePath { get; set; }

        public async Task<CloudBlockBlob> uploadToAzure()
        {
            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this._stroageConnection);
            blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
           
            await _container.CreateIfNotExistsAsync();
            await _container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            //Get reference to a blob

            _blockBlob = _container.GetBlockBlobReference(blobName);

            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                await _blockBlob.UploadFromStreamAsync(fileStream);
            }
            return _blockBlob;
        }

      
    }
}
