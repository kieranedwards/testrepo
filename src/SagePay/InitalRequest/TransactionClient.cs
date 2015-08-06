using System.Net;
using System.Text;
using SagePay.Core;
using SagePay.Interfaces;

namespace SagePay.InitalRequest
{
    public class TransactionClient : ITransactionClient
    {

        /*
         * Sample Responce
         * 
         * VPSProtocol=3.00
            Status=OK REPEATED
            StatusDetail=4042 : The VendorTxCode has been used before for another transaction.  All VendorTxCodes must be unique.
            VPSTxId=DF979C85-DE1D-56F9-A68A-DB99059CA663
            SecurityKey=VWGSMYHHTM
            NextURL=https://test.sagepay.com/gateway/service/cardselection?vpstxid=DF979C85-DE1D-56F9-A68A-DB99059CA663
        */

        public SagePayMessage Request(ITransactionRequestBuilder transactionRequestBuilder)
        {
            using (var client = new WebClient())
            {
                var responseBytes = client.UploadValues(transactionRequestBuilder.SagePayEndPoint, "POST", transactionRequestBuilder.AsParameterList());
                var responseString = (new UTF8Encoding()).GetString(responseBytes);
                return new SagePayMessage(responseString, "\n");
            }
   
        }

    }
}
