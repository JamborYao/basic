using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XMLHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXML();
            ReadXML();
            DataSetXML();
        }
        public static void CreateXML()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclar = doc.CreateXmlDeclaration(version:"1.0",standalone:"yes",encoding:"");
            doc.PrependChild(xmlDeclar);
            XmlElement element = doc.CreateElement(name: "Users");
            doc.AppendChild(element);
            XmlElement user = doc.CreateElement(name: "User");
          
            user.SetAttribute(name: "ID", value: "1");
            element.AppendChild(newChild: user);
            XmlElement userage = doc.CreateElement(name: "Age");
            element.FirstChild.AppendChild(userage);
            XmlElement userName = doc.CreateElement(name: "Name");
            userName.InnerText = "jambor";
            element.FirstChild.PrependChild(newChild: userName);
            XmlElement userPhone= doc.CreateElement(name: "Phone");
            XmlNode xmlNode = doc.DocumentElement;
            element.FirstChild.InsertBefore(newChild: userPhone, refChild: xmlNode.FirstChild.FirstChild);
            //XmlElement userAdd = doc.CreateElement(name: "Address");

            //xmlNode.InsertAfter(newChild: userPhone, refChild: xmlNode.SelectNodes(xpath:"Name")[0]);
           
            doc.Save(filename: @"d:\user.xml");


            
        }
        public static void ReadXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename: @"d:\user.xml");
            XmlNode xmlNode = doc.DocumentElement;
            xmlNode.FirstChild.FirstChild.InnerText = "afadf";
            doc.Save(filename: @"d:\user.xml");
            XmlElement element = doc.DocumentElement;
            element.GetElementsByTagName(name: "Phone");

            XDocument xdoc = XDocument.Load(uri:@"d:\user.xml");
           
            var text = from t in xdoc.Descendants("Name").Where(w=>w.Value=="jambor")  select t;

            var a= text.Single<XElement>();
            a.ReplaceWith(new XElement("XName", "Karen"));
            xdoc.Save(@"d:\user.xml");


        }
        public static void DataSetXML()
        {
           
            

        }
    }
}
