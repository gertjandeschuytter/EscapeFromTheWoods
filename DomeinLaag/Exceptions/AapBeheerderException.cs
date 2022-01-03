using System;
using System.Runtime.Serialization;

namespace DomeinLaag.Exceptions
{
    [Serializable]
    public class AapBeheerderException : Exception
    {
        public AapBeheerderException()
        {
        }

        public AapBeheerderException(string message) : base(message)
        {
        }

        public AapBeheerderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AapBeheerderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
