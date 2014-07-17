using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace RustAutoLauncher
{
    class RustConnector
    {
        private String host;
        private String port;
        private String rustpath;

        public RustConnector (String host = null, String port = null, String rustpath = null)
        {
            this.host = host;
            this.port = port;
            
            if(rustpath != null)
            {
                this.rustpath = rustpath;
            }
            else
            {
                this.rustpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }

        public void connect(Boolean autoconnect = true)
        {
            if(!checkClientCfgExist())
            {
                try
                { 
                    copyConfigCfgtoClientCfg(); 
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException("Rust Config files not found.  Please make sure the launcher is in the same directory as rust.exe, or check your rust config files!");
                }

            }
            deleteExistingNetConnect();
            if (autoconnect)
            {
                appendNetConnect();
            }
            exec();
        }

        private bool checkClientCfgExist()
        {
            return File.Exists(rustpath + "\\cfg\\client.cfg");
        }

        private void copyConfigCfgtoClientCfg()
        {
            try
            {
                File.Copy(rustpath + "\\cfg\\config.cfg", rustpath + "\\cfg\\client.cfg", false);
            }
            catch
            {
                throw;
            }

        }

        private void deleteExistingNetConnect()
        {
            String filename = rustpath + "\\cfg\\client.cfg";
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(filename))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.Contains("net.connect"))
                    {
                        sw.WriteLine(line);
                    }
                }
            }

            File.Delete(filename);
            File.Move(tempFile, filename);
        }

        private void appendNetConnect()
        {
            String filename = rustpath + "\\cfg\\client.cfg";
            using (StreamWriter w = File.AppendText(filename))
            {
                w.WriteLine(String.Format("net.connect {0}:{1}", host, port));
            }
        }

        private void exec()
        {
            Process p = new Process();
            p.EnableRaisingEvents = false;
            p.StartInfo.FileName = rustpath + "\\rust.exe";
            p.Start();
        }

    }
}
