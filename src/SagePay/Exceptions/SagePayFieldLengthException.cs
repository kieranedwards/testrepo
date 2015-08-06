using System;

namespace SagePay.Exceptions
{
    public class SagePayFieldLengthException : Exception
    {
        public SagePayFieldLengthException(string message):base(message)
        {
        }
    }
}