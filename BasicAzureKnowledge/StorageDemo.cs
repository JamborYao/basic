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
        private string _accountName = "willshao";
        private string _accountKey = "tgbo/iCw6KMVBSH1T7wrpT1bjoYtWJXGjYU/xnTKSbeg2uUlzelekbcfTrSH3KRGp+Gkwkfbnlhs7Pl2gKn9nw==";
        //private string _stroageConnection = string.Format("DefaultEndpointsProtocol = https; AccountName={0};AccountKey={1}");
        private string _stroageConnection {
            get { return string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", this._accountName, this._accountKey); }
        }
        private string _stroageConnectionCN
        {
            get {
                return string.Format(
                "BlobEndpoint=https://{0}.blob.core.chinacloudapi.cn/;QueueEndpoint=https://{0}.queue.core.chinacloudapi.cn/;TableEndpoint=https://{0}.table.core.chinacloudapi.cn/;FileEndpoint=https://{0}.file.core.chinacloudapi.cn/;AccountName={0};AccountKey={1}", this._accountName, this._accountKey); }
        }
        CloudBlobClient blobClient = null;
        CloudBlobContainer _container = null;
        CloudBlockBlob _blockBlob = null;
        CloudTableClient _tableClient = null;
        public string containerName { get; set; }
        public string blobName { get; set; }
        public string filePath { get; set; }
        public StorageDemo()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this._stroageConnection);
            blobClient = storageAccount.CreateCloudBlobClient();
           
        }
        public StorageDemo(string containerName)
        {
            this.containerName = containerName;
            _container = blobClient.GetContainerReference(containerName);
          
        }
        public StorageDemo(string accountName,string accountKey)
        {
            _accountName= accountName ?? _accountName;
            _accountKey = accountKey ?? _accountKey;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this._stroageConnectionCN);
            blobClient = storageAccount.CreateCloudBlobClient();
        }
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
        private  void ConfigureCors(ServiceProperties serviceProperties)
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
        public IEnumerable<TEntity> RetrieveTableEntitiesInCondition<TEntity>(string tableName, string conditions ) where TEntity : TableEntity, new()
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
        
        public void InsertToAzureTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this._stroageConnectionCN);
            var tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("test");
            table.CreateIfNotExists();
            Student stu = new Student("grade1", "class1") { Name = "james", Address = "cn" };
            TableOperation insert = TableOperation.Insert(stu);
            table.Execute(insert);
        }
        
    }
    public class Student:TableEntity
    {
        public Student()
        {
        }
        public Student(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
