using System;
using SagePay.Core;

namespace PureRide.Web.Exceptions
{
    public class SagePayInitalRequestException : Exception
    {
        public SagePayInitalRequestException(SagePayMessage.ResponseStatus status, string statusDetail)
            : base(string.Concat(status.ToString(), "  -  ", statusDetail))
        {
        }
    }

}