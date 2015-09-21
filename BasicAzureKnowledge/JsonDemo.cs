using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAzureKnowledge
{
    public class JsonDemo
    {
        public static void JArrayTest()
        {
            string jsonStr = @"{'name':'jambor'}";
            //JsonConvert.DeserializeObject(jsonStr);

            JArray ja = (JArray)JsonConvert.DeserializeObject(jsonStr);

            foreach (JToken jt in ja)

            {

                JObject jo = (JObject)jt;

                JArray temp = (JArray)jo["Languages"];

                foreach (JToken token in temp)

                {

                    Console.Write(token + " ");

                }

                Console.WriteLine("\t" + jo["Name"] + "\t" + jo["Sex"]);

            }
        }

    }
}
