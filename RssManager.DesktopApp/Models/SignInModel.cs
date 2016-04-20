using Newtonsoft.Json;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RssManager.DesktopApp.Models
{
    public class SignInModel
    {
        public void Auth(string username, string password)
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
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (Stream r = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(r))
                    {
                        json = sr.ReadToEnd();
                    }
                }
            }

            Token token = JsonConvert.DeserializeObject<Token>(json);
        }
    }
}
