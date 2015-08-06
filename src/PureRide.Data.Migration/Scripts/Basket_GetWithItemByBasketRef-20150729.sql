-- ==========================================================
-- Create Stored Procedure Template for Windows Azure SQL Database
-- ==========================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Basket_GetWithItemByBasketRef
	@BasketRef uniqueidentifier 
AS
BEGIN
		SET NOCOUNT ON;
    
		SELECT Top 1 SalePrice, ProductDescription, BasketRef,CampaignCode,ProductEntityId,PersonEntityId,PersonCentreId,ProductCentreId
		FROM prBasket b
		JOIN prBasketLine bl ON b.BasketId = bl.BasketId
		WHERE BasketRef=@BasketRef
		
END
GO

