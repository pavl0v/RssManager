using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RssManager.Objects.Exceptions
{
    [Serializable]
    public class DuplicateEntityException : Exception, ISerializable
    {
        public object Index { get; private set; }

        public DuplicateEntityException(object index)
        {
            this.Index = index;
        }

        public DuplicateEntityException(object index, string message)
            : base(message)
        {
            this.Index = index;
        }

        public DuplicateEntityException(object index, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Index = index;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
