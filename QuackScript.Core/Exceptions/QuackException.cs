using System;
using System.Runtime.Serialization;

namespace QuackScript.Core.Exceptions
{
    public class QuackException : Exception
    {
        public QuackException() { }
        public QuackException(string message) : base(message) { }
        public QuackException(string message, Exception innerException) : base(message, innerException) { }
        protected QuackException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}