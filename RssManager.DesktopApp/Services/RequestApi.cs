using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.DesktopApp.Services
{
    public enum Method
    {
        GET,
        POST
    }

    public class RequestApi : IRequestApi
    {
        public string Request(Method method, string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method.ToString();
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + Token.GetInstance().AccessToken);

            string json = string.Empty;

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            json = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                string errorMessage = string.Empty;
                using (WebResponse wr = ex.Response)
                {
                    using (Stream stream = wr.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            errorMessage = sr.ReadToEnd();
                        }
                    }
                }
                throw new Exception(errorMessage);
            }

            return json;
        }
    }
}
