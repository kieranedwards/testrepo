-- ==========================================================
-- Create Stored Procedure Template for Windows Azure SQL Database
-- ==========================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE BasketPayment_UpdateWithResponse
	@vpsTxId nvarchar(38),
	@TxAuthNo bigint,
	@StatusCode nvarchar(15),
	@statusDetail nvarchar(255),
	@RawResponse nvarchar(max)
AS
BEGIN
		SET NOCOUNT ON;

		UPDATE prBasketPayment set 
			TxAuthNo=@TxAuthNo,
			StatusCode =@StatusCode, 
			StatusDetail = @statusDetail,
			RawResponse = @RawResponse,
			DateUpdated = getdate()
		WHERE VPSTxId = @VPSTxId

		SELECT Top 1 SalePrice, ProductDescription, BasketRef,CampaignCode,ProductEntityId,PersonEntityId,PersonCentreId,ProductCentreId
		FROM prBasket b
		JOIN prBasketLine bl ON b.BasketId = bl.BasketId
		JOIN prBasketPayment bp on b.BasketId = bp.BasketId
		WHERE VPSTxId = @VPSTxId
 
END
GO

