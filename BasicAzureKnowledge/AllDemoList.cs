using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAzureKnowledge
{
    public abstract class AllDemoList
    {
        /// <summary>
        /// azure storage upload demo
        /// </summary>
        public virtual void upload()
        {
            StorageDemo myStorage = new StorageDemo();
            myStorage.containerName = "mycontainer1";
            myStorage.blobName = "forcasetest";
            myStorage.filePath = @"D:\test.txt";
            var result = myStorage.uploadToAzure();
            CloudBlockBlob blob = result.Result;
        }

        /// <summary>
        /// async demo
        /// </summary>
        public virtual void AsyncMethod()
        {
            AsyncDemo.MyAsync1();
        }
    }
}
