using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RssManager.WebAPI.Providers
{
    public class MyIdentity : System.Security.Principal.IIdentity
    {
        public string AuthenticationType
        {
            get;
            private set;
        }

        public bool IsAuthenticated
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public MyIdentity(string name, string authenticationType)
        {
            this.Name = name;
            this.AuthenticationType = authenticationType;
            this.IsAuthenticated = true;
        }
    }
}