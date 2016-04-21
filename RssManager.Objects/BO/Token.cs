using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Objects.BO
{
    public class Token
    {
        private static Token _instance;
        private static object syncLock = new object();

        private string accessToken = string.Empty;
        private string tokenType = string.Empty;
        private int expiresIn = 0;

        public string AccessToken
        {
            get { return this.accessToken; }
            set { this.accessToken = value; }
        }

        public string TokenType
        {
            get { return this.tokenType; }
            set { this.tokenType = value; }
        }

        public int ExpiresIn
        {
            get { return this.expiresIn; }
            set { this.expiresIn = value; }
        }

        private Token()
        {

        }

        public static Token GetInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Token();
                    }
                }
            }
            return _instance;
        }
    }
}
