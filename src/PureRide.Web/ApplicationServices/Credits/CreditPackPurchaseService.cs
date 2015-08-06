using System;
using System.Linq;
using System.Web;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Data.DataTransfer;
using PureRide.Data.Repositories;
using PureRide.Web.Exceptions;
using SagePay.Core;
using SagePay.Interfaces;

namespace PureRide.Web.ApplicationServices.Credits
{
    public interface ICreditPackPurchaseService
    {
        SagePayMessage ProcessPayment(string sagePayResponse);
        void SetOrderTransactionId(Guid basketRef, string sagePayTransactionId);
    }

    /*
     * 
     Sample response Data
        VPSProtocol=3.00&
        TxType=PAYMENT&VendorTxCode=3a972d9c-1e44-4ff7-898c-5d00f80b6bc4
        VPSTxId=%7B8B424E67-A809-036E-0C50-EF24982A5D61%7D
        Status=OK
        StatusDetail=0000+%3A+The+Authorisation+was+Successful
        TxAuthNo=8657459&AVSCV2=NO+DATA+MATCHES
        AddressResult=NOTMATCHED&PostCodeResult=NOTMATCHED
        CV2Result=NOTMATCHED&GiftAid=0
        3DSecureStatus=NOTCHECKED
        CardType=VISA&Last4Digits=0006
        VPSSignature=07A7BF2648616ACF9199B59A8F955937
        DeclineCode=00
        ExpiryDate=0255
        BankAuthCode=999777
     */

    public class CreditPackPurchaseService : ICreditPackPurchaseService
    {
        private readonly IClipCardPurchaseService _clipCardPurchaseService;
        private readonly ICreditPackBasketService _creditPackBasketService;
        private readonly IClipCardRepository _clipCardRepository;
        private readonly IPaymentListenerUrlProvider _paymentListenerUrlProvider;

        public CreditPackPurchaseService(IPaymentListenerUrlProvider paymentListenerUrlProvider, IClipCardRepository clipCardRepository, IClipCardPurchaseService clipCardPurchaseService, ICreditPackBasketService creditPackBasketService)
        {
            _paymentListenerUrlProvider = paymentListenerUrlProvider;
            _clipCardRepository = clipCardRepository;
            _clipCardPurchaseService = clipCardPurchaseService;
            _creditPackBasketService = creditPackBasketService;
        }

        private static SagePayMessage.ResponseStatus[] ValidStatuses
        {
            get { return new[] { SagePayMessage.ResponseStatus.REGISTERED, SagePayMessage.ResponseStatus.AUTHENTICATED, SagePayMessage.ResponseStatus.OK }; }
        }

        private static SagePayMessage.ResponseStatus[] InValidStatuses
        {
            get { return new[] { SagePayMessage.ResponseStatus.NOTAUTHED, SagePayMessage.ResponseStatus.ABORT, SagePayMessage.ResponseStatus.REJECTED, SagePayMessage.ResponseStatus.ERROR }; }
        }

        /// <remarks>
        /// PENDING is not supported as we have not enabledthat payment type - (for European Payment Types only), if the transaction has yet to be accepted or rejected
        /// OK, if the transaction was authorised.
        /// NOTAUTHED, if the authorisation was failed by the bank.
        /// ABORT, if the user decided to cancel the transaction whilst on our payment pages.
        /// REJECTED, if your fraud screening rules were not met.
        /// </remarks>
        public SagePayMessage ProcessPayment(string sagePayResponse)
        {
            SagePayMessage.ResponseStatus requestStatus;
            SagePayMessage.ResponseStatus responseStatus;
            var txAuthNo = string.Empty;

            try
            {
                var message = new SagePayMessage(HttpUtility.UrlDecode(sagePayResponse), "&");
                var basketData = GetBasketData(sagePayResponse, message);
                
                txAuthNo = message.TxAuthNo.ToString();
                requestStatus = message.Status;
                responseStatus = SagePayMessage.ResponseStatus.OK;

                if(UpdateExerpWithPurchase(requestStatus, basketData, message))
                    RemoveBasketCookie();

            }
            catch (Exception ex)
            {
                if (ex is System.Web.Services.Protocols.SoapException || ex is HttpException)
                    throw;

                requestStatus = SagePayMessage.ResponseStatus.ERROR;
                responseStatus = SagePayMessage.ResponseStatus.ERROR;
            }

            return new SagePayMessage(responseStatus, _paymentListenerUrlProvider.GetReturnUrlForStatus(requestStatus, txAuthNo), string.Empty);
        }

        public void SetOrderTransactionId(Guid basketRef, string sagePayTransactionId)
        {
            _clipCardRepository.SetOrderTransactionId(basketRef, sagePayTransactionId);
        }

        private void RemoveBasketCookie()
        {
                _creditPackBasketService.ClearSelectedClipCard();
        }

        private ClipCardOrder GetBasketData(string sagePayResponse, SagePayMessage message)
        {
            var basketData = _clipCardRepository.UpdateOrderTransaction(message.VpsTxId, message.Status.ToString(),
                message.StatusDetail, message.TxAuthNo, sagePayResponse);

            if (basketData == null)
                throw new BasketNotFoundException(message.VpsTxId);

            return basketData;
        }

        private bool UpdateExerpWithPurchase(SagePayMessage.ResponseStatus requestStatus, ClipCardOrder basketData, SagePayMessage message)
        {
            if (!ValidStatuses.Contains(requestStatus)) 
                return false;

            _clipCardPurchaseService.PurchaseClipCard(new PurchaseDetails()
            {
                AmountPaid = basketData.SalePrice,
                CampaignCode = basketData.CampaignCode,
                PersonId = new Identity(basketData.PersonCentreId, basketData.PersonEntityId),
                ProductId = new Identity(basketData.ProductCentreId, basketData.ProductEntityId),
                SageTxAuthNo = message.TxAuthNo.ToString()
            });

            return true;
        }
    }
}
