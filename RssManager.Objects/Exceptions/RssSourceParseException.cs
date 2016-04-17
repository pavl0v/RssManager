using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Objects.Exceptions
{
    [Serializable]
    public class RssSourceParseException : Exception, ISerializable
    {
        public string Url { get; private set; }

        public RssSourceParseException(string url)
        {
            this.Url = url;
        }

        public RssSourceParseException(string url, string message) 
            : base (message)
        {
            this.Url = url;
        }

        public RssSourceParseException(string url, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Url = url;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Url", this.Url, typeof(string));
        }
    }
}
