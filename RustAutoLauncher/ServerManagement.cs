using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.IO;

namespace RustAutoLauncher
{
    class ServerManagement
    {
        //public DataTable serverlist;
        XmlDocument xmldoc = new XmlDocument();
        public ServerManagement()
        {
            loadxmldoc();
            
        }

        private String getServerXmlPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\serverlist.xml";
        }

        private void loadxmldoc()
        {
            try
            {
                xmldoc.Load(getServerXmlPath());
            }
            catch (FileNotFoundException)
            {
                XmlDeclaration dec = xmldoc.CreateXmlDeclaration("1.0", null, null);
                xmldoc.AppendChild(dec);
                XmlElement serversroot = xmldoc.CreateElement("Servers");
                xmldoc.AppendChild(serversroot);
                xmldoc.Save(getServerXmlPath());
            }
        }
        
        public void addServer(String name, String server, String port)
        {
            XmlNodeList already = xmldoc.SelectNodes(String.Format("Servers/Server[text() = '{0}']", name));
            if (already.Count == 0)
            {
                XmlElement element = xmldoc.CreateElement("Server");
                element.InnerText = name;
                element.SetAttribute("server", server);
                element.SetAttribute("port", port);
                xmldoc.SelectSingleNode("Servers").AppendChild(element);
                xmldoc.Save(getServerXmlPath());
            }
            else
            {
                throw new DuplicateNameException(String.Format("A server with the name {0} already exists", name));
            }
        }

        public void removeServer(String name)
        {
            XmlNode removeserver = xmldoc.SelectSingleNode(String.Format("Servers/Server[text() = '{0}']", name));
            xmldoc.SelectSingleNode("Servers").RemoveChild(removeserver);
            xmldoc.Save(getServerXmlPath());
        }

        public XmlNodeList getServerList()
        {
            return xmldoc.SelectNodes("Servers/Server");
        }

        public XmlNode getServerByName(String name)
        {
            return xmldoc.SelectSingleNode(String.Format("Servers/Server[text() = '{0}']", name));
        }
    }
}
