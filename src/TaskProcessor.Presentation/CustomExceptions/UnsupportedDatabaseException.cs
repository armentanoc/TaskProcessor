using System.Runtime.Serialization;

namespace TaskProcessor.Presentation.CustomExceptions
{
    [Serializable]
    internal class UnsupportedDatabaseException : Exception
    {
        public UnsupportedDatabaseException() : base("Database provider not supported")
        {
            
        }
        public UnsupportedDatabaseException(string? message) : base(message)
        {
        }

        public UnsupportedDatabaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnsupportedDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}