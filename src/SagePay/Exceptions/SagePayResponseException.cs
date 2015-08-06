using System;

namespace SagePay.Exceptions
{
    internal class SagePayResponseException : Exception
    {
        public SagePayResponseException(string message)
            : base(message)
        {
            
        }
    }
}