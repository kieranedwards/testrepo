-- ==========================================================
-- Create Stored Procedure Template for Windows Azure SQL Database
-- ==========================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Basket_CreateWithItem
	@BasketRef uniqueidentifier,
	@PersonEntityId int,
	@PersonCentreId int, 
	@CampaignCode varchar(25),
	@ProductEntityId int, 
	@ProductCentreId int, 
	@ProductDescription varchar(500),
	@SalePrice decimal(19,2)
AS
BEGIN
		SET NOCOUNT ON;
    
		INSERT INTO prBasket(BasketRef, PersonEntityId, PersonCentreId, CampaignCode)
		Values (@BasketRef, @PersonEntityId, @PersonCentreId, @CampaignCode)

		INSERT INTO prBasketLine(BasketId, ProductEntityId, ProductCentreId, SalePrice, ProductDescription)
		Values (SCOPE_IDENTITY(), @ProductEntityId, @ProductCentreId, @SalePrice, @ProductDescription)

END
GO

