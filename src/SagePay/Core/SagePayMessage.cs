using System;
using SagePay.Exceptions;
using SagePay.Helpers;

namespace SagePay.Core
{
    public class SagePayMessage
    {
        public string VpsTxId { get; private set; }
        public string NextUrl { get; private set; }
        public ResponseStatus Status { get; private set; }
        public string StatusDetail { get; private set; }
        public string RedirectUrl { get; set; }
        public Int64 TxAuthNo { get; set; }

        public SagePayMessage(string responseData, string separator)
        {
            if(string.IsNullOrWhiteSpace(responseData))
                throw new SagePayResponseException("No response data was provided");

            var data = responseData.ExtractDataFromSageResponse(separator);
            VpsTxId = data.SafeFetch<string>("VPSTxId");
            NextUrl = data.SafeFetch<string>("NextURL");
            TxAuthNo = data.SafeFetch<Int64>("TxAuthNo");
            Status = GetStatusFromString(data.SafeFetch<string>("Status"));
            StatusDetail = data.SafeFetch<string>("StatusDetail");
        }

        public SagePayMessage(ResponseStatus status,string redirectUrl, string statusDetail)
        {
            StatusDetail = statusDetail;
            Status = status;
            RedirectUrl = redirectUrl;
        }

        private ResponseStatus GetStatusFromString(string status)
        {
            if(string.IsNullOrWhiteSpace(status))
                throw new SagePayResponseException("Response status not found");

            ResponseStatus result;
            if(!Enum.TryParse(status.Replace(" ", "_"), out result))
            {
                 throw new SagePayResponseException(string.Concat("Unexpected response status - ", status));
            }

            return result;
        }

        public string AsFormattedResponse()
        {
            return string.Format("Status={1}{0}RedirectURL={2}{0}StatusDetail={3}", Environment.NewLine, Status,RedirectUrl,StatusDetail);
        }

        // ReSharper disable InconsistentNaming
        public enum ResponseStatus 
        {
            OK,
            OK_REPEATED,
            MALFORMED,
            INVALID,
            NOTAUTHED,
            ABORT, 
            REJECTED, 
            ERROR,
            REGISTERED, 
            AUTHENTICATED 
        };
 
    }
}