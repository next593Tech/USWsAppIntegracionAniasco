USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[INV_PreciosProductosXClaseCliente]    Script Date: 21/08/2017 11:47:54 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos  
-- Create date: 20 Enero 2017
-- Description:	Consulta de Lista de Precios de un Productos por clase de cliente
-- =============================================
ALTER PROCEDURE [dbo].[INV_PreciosProductosXClaseCliente] 
  @ClienteId AS VARCHAR(13), @ProductoID AS VARCHAR(10)
AS

BEGIN

	SET NOCOUNT ON;

	--	EXEC [INV_PreciosProductosXClaseCliente] '0000015909','0000000006';
	--	SELECT CLI.Clase,* FROM CLI_CLIENTES AS CLI WHERE CLASE<>'01'
	DECLARE @clase	as char(2);
	DECLARE @existe as char(1);
	
	 
	SET @clase = (SELECT CLI.Clase FROM [ANGELITO].[dbo].CLI_CLIENTES AS CLI WHERE CLI.Ruc = @ClienteId)
 

	IF (@clase) = '01'			--	CLIENTE NORMAL
	BEGIN
		SELECT  PRE.ProductoID, Rango1, Rango2, Precio FROM [ANGELITO].[dbo].INV_PRODUCTOS_PRECIOS PRE
		INNER JOIN [ANGELITO].[dbo].INV_PRODUCTOS PRO
		ON PRE.ProductoID = PRO.ID
		WHERE PRO.ID = @ProductoID ORDER BY RANGO1
 	END

	ELSE IF (@clase) = '02'		--	CLIENTE MAYORISTA
	BEGIN
		SELECT PRO.ID,1 AS Rango1,1 AS Rango2,PRO.Precio5  FROM [ANGELITO].[dbo].INV_PRODUCTOS AS PRO 
		WHERE PRO.ID = @ProductoID
 	END

	ELSE IF (@clase) = '03'		--	CLIENTE DISTRIBUIDOR
	BEGIN
		SELECT PRO.ID,1 AS Rango1,1 AS Rango2,PRO.Precio6  FROM [ANGELITO].[dbo].INV_PRODUCTOS AS PRO 
		WHERE PRO.ID = @ProductoID
 	END

	ELSE IF (@clase) = '04'		--	CLIENTE ESPECIAL
	BEGIN
		SELECT PRO.ID,1 AS Rango1,1 AS Rango2,PRO.Precio7  FROM [ANGELITO].[dbo].INV_PRODUCTOS AS PRO 
		WHERE PRO.ID = @ProductoID
 	END

END

