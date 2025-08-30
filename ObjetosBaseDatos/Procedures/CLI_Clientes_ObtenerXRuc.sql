USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[CLI_Clientes_ObtenerXRuc]    Script Date: 21/08/2017 11:42:41 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	    Douglas Bustos
-- Create date: 24 enero 2017
-- Description:	Lista de Clientes
-- =============================================
ALTER PROCEDURE [dbo].[CLI_Clientes_ObtenerXRuc]
 
@Ruc VARCHAR(13)= NULL 
 
AS
BEGIN

-- exec [dbo].[CLI_Clientes_ObtenerXRuc] '0909395626'
-- select *  FROM [ANGELITO].[dbo].[CLI_CLIENTES] AS CLI where ID='0000000108'
	
	SET NOCOUNT ON;
	 
	IF (@Ruc is NULL)
	BEGIN
		SET @Ruc = ''
	END
	 
    SELECT RucCliente = CLI.Ruc,TerminoID = CLI.TérminoID,ClienteID =CLI.ID , Nombre =UPPER(LTRIM(CLI.Nombre)), CLI.Clase, ISNULL(CLI.Cupo,0) ,
	Saldo = (
		SELECT 
			Saldo = ISNULL( SUM(( CASE DE.Crédito WHEN 1 THEN -DE.Saldo ELSE DE.Saldo END ) * DI.Cambio ) , 0 )
		FROM 
			[ANGELITO].[dbo].CLI_CLIENTES_DEUDAS DE  WITH(NOLOCK) INNER JOIN  [ANGELITO].[dbo].SIS_DIVISAS DI  WITH(NOLOCK) ON DE.DivisaID = DI.ID
		WHERE 
			( DE.ClienteID = CLI.ID ) AND ( DE.Saldo > 0 )  AND ( DE.Anulado = 0 )
	)

    FROM [ANGELITO].[dbo].[CLI_CLIENTES] AS CLI
    WHERE CLI.Ruc = @Ruc  
     
END

