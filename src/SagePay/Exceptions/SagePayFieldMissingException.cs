using System;

namespace SagePay.Exceptions
{
    public class SagePayFieldMissingException : Exception
    {
        public SagePayFieldMissingException(string message):base(message)
        {
        }
    }
}