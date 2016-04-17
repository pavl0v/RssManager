using log4net;
using RssManager.Interfaces.RssContentReader;
using System;
using System.IO;
using System.Text;

namespace RssManager.Objects.RssContentReader
{
    public class RssLocalContentReader : IRssSourceContentReader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssHttpContentReader));

        private string path = string.Empty;

        public string Uri
        {
            get
            {
                return this.path;
            }
            //set
            //{
            //    this.path = value;
            //}
        }

        public RssLocalContentReader(string path)
        {
            this.path = path;
        }

        public string Read()
        {
            string content = string.Empty;

            try
            {
                using (FileStream fs = new FileStream(this.path, FileMode.Open))
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    content = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Rss local content read failed. Exception message: {0}", ex.Message));
            }

            return content;
        }
    }
}
