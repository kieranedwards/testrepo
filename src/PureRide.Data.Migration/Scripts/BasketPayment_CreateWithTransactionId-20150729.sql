-- ==========================================================
-- Create Stored Procedure Template for Windows Azure SQL Database
-- ==========================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE BasketPayment_CreateWithTransactionId
	@BasketRef uniqueidentifier,
	@VPSTxId varchar(38)
AS
BEGIN
		SET NOCOUNT ON;
    
		declare @BasketId int

		SELECT TOP 1 @BasketId=BasketId FROM prBasket WHERE BasketRef = @BasketRef
		
		INSERT INTO prBasketPayment(BasketId,VPSTxId)
		VALUES (@BasketId, @VPSTxId)
		
END
GO
