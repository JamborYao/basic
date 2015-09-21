using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
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
        
            
            Demo demo = new Demo();
            // demo.upload();
            //demo.AsyncMethod();
           JsonDemo. JArrayTest();
            Console.WriteLine("finished!");
            Console.ReadKey();
        }


       

    }

    public class Demo : AllDemoList
    {

    }
}
