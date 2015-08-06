using System;
using System.Data;
using System.Linq;
using DapperWrapper;
using Exerp.Api.DataTransfer;
using PureRide.Data.DataTransfer;

namespace PureRide.Data.Repositories
{
    public class ClipCardRepository : IClipCardRepository
    {
        private readonly IDbExecutor _databaseExecutor;

        public ClipCardRepository(IDbExecutor databaseExecutor)
        {
            _databaseExecutor = databaseExecutor;
        }

        public Guid CreateOrder(Identity personId, Identity productId, string name, string price, string campaignCode)
        {
            var basketRef = Guid.NewGuid();

            _databaseExecutor.Execute("Basket_CreateWithItem", 
                new { BasketRef = basketRef,
                    ProductDescription = name,
                    PersonEntityId = personId.EntityId, 
                    PersonCentreId = personId.CentreId,
                    CampaignCode = campaignCode, 
                    ProductEntityId = productId.EntityId,
                    ProductCentreId = productId.CentreId,
                    SalePrice = price 
                }, commandType: CommandType.StoredProcedure);

            return basketRef;
        }

        public ClipCardOrder GetBasket(Guid basketRef)
        {
          return  _databaseExecutor.Query<ClipCardOrder>("Basket_GetWithItemByBasketRef",
             new {BasketRef = basketRef}, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public ClipCardOrder UpdateOrderTransaction(string vpsTxId, string statusCode, string statusDetail, long txAuthNo, string rawResponse)
        {
             return _databaseExecutor.Query<ClipCardOrder>("BasketPayment_UpdateWithResponse",
            new { vpsTxId, statusCode, statusDetail, txAuthNo, rawResponse }, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public void SetOrderTransactionId(Guid basketRef, string sagePayTransactionId)
        {
            _databaseExecutor.Execute("BasketPayment_CreateWithTransactionId",
           new { vpsTxId = sagePayTransactionId, basketRef }, commandType: CommandType.StoredProcedure);
        }
    }

    public interface IClipCardRepository
    {
        Guid CreateOrder(Identity personId, Identity productId, string name, string price, string campaignCode);
        ClipCardOrder GetBasket(Guid basketRef);
        ClipCardOrder UpdateOrderTransaction(string vpsTxId, string statusCode, string statusDetail, long txAuthNo, string rawResponse);
        void SetOrderTransactionId(Guid basketRef, string sagePayTransactionId);
    }
}
