using System;
using System.Runtime.Serialization;

namespace DomeinLaag.Exceptions
{
    [Serializable]
    public class BoomBeheerderException : Exception
    {
        public BoomBeheerderException()
        {
        }

        public BoomBeheerderException(string message) : base(message)
        {
        }

        public BoomBeheerderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoomBeheerderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
