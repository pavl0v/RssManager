using Newtonsoft.Json;
using RssManager.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.DesktopApp.Models
{
    public class SignInModel
    {
        public TokenDTO Auth(string username, string password)
        {
            HttpWebRequest request = WebRequest.Create("http://localhost:64910/token") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            var authCredentials = "grant_type=password&username=" + username + "&password=" + password;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(authCredentials);
            request.ContentLength = bytes.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

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

            TokenDTO token = JsonConvert.DeserializeObject<TokenDTO>(json);
            return token;
        }
    }
}
