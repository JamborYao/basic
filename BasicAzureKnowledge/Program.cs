﻿using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
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
            StorageDemo test = new StorageDemo("test1","test2");


                Demo demo = new Demo();
            // demo.upload();
            //demo.AsyncMethod();
           JsonDemo. JArrayTest();
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
