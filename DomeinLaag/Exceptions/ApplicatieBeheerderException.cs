using System;
using System.Runtime.Serialization;

namespace DomeinLaag.Exceptions
{
    [Serializable]
    public class ApplicatieBeheerderException : Exception
    {
        public ApplicatieBeheerderException()
        {
        }

        public ApplicatieBeheerderException(string message) : base(message)
        {
        }

        public ApplicatieBeheerderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApplicatieBeheerderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
