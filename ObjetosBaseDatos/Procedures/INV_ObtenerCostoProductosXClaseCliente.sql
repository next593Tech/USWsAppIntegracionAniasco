USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[INV_ObtenerCostoProductosXClaseCliente]    Script Date: 21/08/2017 11:47:27 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos  
-- Create date: 20 Enero 2017
-- Description:	Consulta de Lista de Precios de un Productos por clase de cliente
-- =============================================
ALTER PROCEDURE [dbo].[INV_ObtenerCostoProductosXClaseCliente] 
  @ClienteId AS VARCHAR(13), @ProductoID AS VARCHAR(10), @Cantidad decimal(8,2), @Precio money OUTPUT
AS

BEGIN

	SET NOCOUNT ON;

	/* 

	SELECT * FROM [ANGELITO].[dbo].INV_PRODUCTOS_PRECIOS WHERE productoid='0000006477'

	SELECT * FROM [ANGELITO].[dbo].INV_PRODUCTOS WHERE ID='0000006477'


	DECLARE @Precio money
	
	EXEC [INV_ObtenerCostoProductosXClaseCliente] '0992997176001','0000006477',25,@PRECIO OUT

	SELECT @Precio

	SELECT CLI.Clase,* FROM [ANGELITO].[dbo].CLI_CLIENTES AS CLI WHERE CLI.id='0000011816'


	*/
	--	SELECT CLI.Clase,* FROM CLI_CLIENTES AS CLI WHERE CLASE<>'01'
	DECLARE @clase	as char(2);
	DECLARE @existe as char(1);
	
	 
	SET @clase = (SELECT CLI.Clase FROM [ANGELITO].[dbo].CLI_CLIENTES AS CLI WHERE CLI.Ruc = @ClienteId)
	 

	IF (@clase) = '01'			--	CLIENTE NORMAL
	BEGIN
		SET @Precio = (SELECT TOP 1 (PRE.Precio) FROM [ANGELITO].[dbo].INV_PRODUCTOS_PRECIOS PRE
		INNER JOIN [ANGELITO].[dbo].INV_PRODUCTOS PRO
		ON PRE.ProductoID = PRO.ID
		WHERE PRO.ID = @ProductoID AND @Cantidad BETWEEN Rango1 AND Rango2 )
 	END

	ELSE IF (@clase) = '02'		--	CLIENTE MAYORISTA
	BEGIN
		SET @Precio = (SELECT TOP 1 (PRO.Precio5) FROM [ANGELITO].[dbo].INV_PRODUCTOS AS PRO 
		WHERE PRO.ID = @ProductoID 	)
 	END

	ELSE IF (@clase) = '03'		--	CLIENTE DISTRIBUIDOR
	BEGIN

		SET @Precio = (SELECT TOP 1 (PRO.Precio6) FROM [ANGELITO].[dbo].INV_PRODUCTOS AS PRO 
		WHERE PRO.ID = @ProductoID 	)

	 
 	END

	ELSE IF (@clase) = '04'		--	CLIENTE ESPECIAL
	BEGIN

		SET @Precio = (SELECT TOP 1 (PRO.Precio7) FROM [ANGELITO].[dbo].INV_PRODUCTOS AS PRO 
		WHERE PRO.ID = @ProductoID 	)
		 
 	END

END

