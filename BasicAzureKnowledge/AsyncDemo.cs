using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicAzureKnowledge
{
    public class AsyncDemo
    {
        public static async void MyAsync1()
        {
            WebClient client = new WebClient();
            string result = await myTask();
            Console.WriteLine(result);
        }
        public static async Task<string> myTask()
        {
            await Task.Run(() => { Thread.Sleep(5000); });
            return "jambor";
        }
    }
}
