using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAzureKnowledge
{
    public class StorageDemo
    {
        #region Azure storage connection string (_stroageConnection,_stroageConnectionCN)
        private string _accountName = "willshao";
        private string _accountKey = "***";

        private string _accountNameCN = "jambor";
        private string _accountKeyCN = "****";
        //private string _stroageConnection = string.Format("DefaultEndpointsProtocol = https; AccountName={0};AccountKey={1}");
        private string _stroageConnection
        {
            get { return string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", this._accountName, this._accountKey); }
        }
        private string _stroageConnectionCN
        {
            get
            {
                //return string.Format("BlobEndpoint=https://{0}.blob.core.chinacloudapi.cn/;QueueEndpoint=https://{0}.queue.core.chinacloudapi.cn/;TableEndpoint=https://{0}.table.core.chinacloudapi.cn/;FileEndpoint=https://{0}.file.core.chinacloudapi.cn/;AccountName={0};AccountKey={1}", this._accountName, this._accountKey);
                return string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};EndpointSuffix=core.chinacloudapi.cn;",this._accountNameCN,this._accountKeyCN);
            }
        }

        #endregion

        CloudStorageAccount storageAccount = null;
        CloudBlobClient blobClient = null;
        CloudBlobContainer _container = null;
        CloudBlockBlob _blockBlob = null;
        CloudTableClient _tableClient = null;
   

        public string containerName { get; set; }
        public string blobName { get; set; }
        public string filePath { get; set; }

        #region contruct function
        public StorageDemo()
        {
            string containerName = "mytest1";
            string blobName = "test";
            InitAzureStorage(ref containerName,ref blobName);

        }
        public StorageDemo(string containerName)
        {           
            string blobName = "test";
            InitAzureStorage(ref containerName, ref blobName);
        }
        public StorageDemo(string accountName, string accountKey)
        {
            this._accountName = accountName ?? _accountName;
            this._accountKey = accountKey ?? _accountKey;
            string containerName = "mytest1";
            string blobName = "test";
            InitAzureStorage(ref containerName, ref blobName);
        }
        public void InitAzureStorage(ref string containerName,ref string blobName)
        {
            storageAccount= CloudStorageAccount.Parse(_stroageConnectionCN);
            blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExists();
            _blockBlob = _container.GetBlockBlobReference(blobName);            
            _tableClient = storageAccount.CreateCloudTableClient();
        }
        #endregion

        #region upload to Azure
        public async Task<CloudBlockBlob> uploadToAzure()
        {

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
        #endregion

        #region add cors
        private void ConfigureCors(ServiceProperties serviceProperties)
        {
            serviceProperties.Cors = new CorsProperties();
            serviceProperties.Cors.CorsRules.Add(new CorsRule()
            {
                AllowedHeaders = new List<string>() { "content-type,accept,x-ms-*" },
                AllowedMethods = CorsHttpMethods.Put | CorsHttpMethods.None, // | CorsHttpMethods.Head | CorsHttpMethods.Post,
                AllowedOrigins = new List<string>() { "*" },
                ExposedHeaders = new List<string>() { "x-ms-*" },
                MaxAgeInSeconds = 1800 // 30 minutes
            });
        }
        public void InitBlobCors()
        {
            ServiceProperties blobServiceProperties = blobClient.GetServiceProperties();
            ConfigureCors(blobServiceProperties);
            blobClient.SetServiceProperties(blobServiceProperties);
        }
        #endregion

        #region retrive data from table
        public IEnumerable<TEntity> RetrieveTableEntitiesInCondition<TEntity>(string tableName, string conditions) where TEntity : TableEntity, new()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this._stroageConnectionCN);
            var tableClient = storageAccount.CreateCloudTableClient();
            IEnumerable<TEntity> entities = null;
            try
            {
                CloudTable table = tableClient.GetTableReference(tableName);
                TableQuery<TEntity> query = new TableQuery<TEntity>().Where(conditions);
                entities = table.ExecuteQuery(query);
                foreach (TEntity entity in entities)
                {
                    Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                        entity.PartitionKey, entity.RowKey);
                }
            }
            catch (Exception ex)
            {
                // logger.Warn("Retrieve condition entity failed: {0}.", ex.ToString());
            }

            return entities;
        }
        #endregion
    }
}