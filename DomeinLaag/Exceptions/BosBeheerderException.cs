using System;
using System.Runtime.Serialization;

namespace DomeinLaag.Exceptions
{
    [Serializable]
    public class BosBeheerderException : Exception
    {
        public BosBeheerderException()
        {
        }

        public BosBeheerderException(string message) : base(message)
        {
        }

        public BosBeheerderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BosBeheerderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
