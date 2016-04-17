using log4net;
using RssManager.Interfaces.RssContentReader;
using System.IO;
using System.Net;
using System.Text;

namespace RssManager.Objects.RssContentReader
{
    public class RssHttpContentReader : IRssSourceContentReader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RssHttpContentReader));

        private string url = string.Empty;

        public RssHttpContentReader(string url)
        {
            this.url = url;
        }

        public string Uri
        {
            get
            {
                return this.url;
            }
            //set
            //{
            //    this.url = value;
            //}
        }

        public string Read()
        {
            string content = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream s = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(s, Encoding.UTF8))
            {
                content = sr.ReadToEnd();
            }

            return content;
        }
    }
}
