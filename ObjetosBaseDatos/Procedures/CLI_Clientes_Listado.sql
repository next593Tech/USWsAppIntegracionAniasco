USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[CLI_Clientes_Listado]    Script Date: 21/08/2017 11:42:25 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Douglas Bustos
-- Create date: 03 enero 2017
-- Description:	Lista de Clientes
-- =============================================
ALTER PROCEDURE [dbo].[CLI_Clientes_Listado]
@PageNumber		INT = 1, 
@PageSize		INT = 5, 
@nombre			varchar(200)= NULL,
@TotalSalida	INT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;

	--declare @Nombre as varchar(200)= '';	declare @total as int;
	IF (@Nombre is NULL)
	BEGIN
		SET @Nombre = ''
	END
	------------
	SET @TotalSalida = ( SELECT count(*) FROM [ANGELITO].[dbo].[CLI_CLIENTES] AS CLI   WHERE CLI.Nombre like '%'+ @Nombre +'%')
	------------

    SELECT RucCliente = CLI.Ruc, Nombre =UPPER(LTRIM(CLI.Nombre)), CLI.Clase,CLI.Cupo ,
	Saldo = (
		SELECT 
			Saldo = ISNULL( SUM(( CASE DE.Crédito WHEN 1 THEN -DE.Saldo ELSE DE.Saldo END ) * DI.Cambio ) , 0 )
		FROM 
			[ANGELITO].[dbo].CLI_CLIENTES_DEUDAS DE  WITH(NOLOCK) INNER JOIN  [ANGELITO].[dbo].SIS_DIVISAS DI  WITH(NOLOCK) ON DE.DivisaID = DI.ID
		WHERE 
			( DE.ClienteID = CLI.ID ) AND ( DE.Saldo > 0 )  AND ( DE.Anulado = 0 )
	)

    FROM [ANGELITO].[dbo].[CLI_CLIENTES] AS CLI
    WHERE CLI.Nombre like '%'+ @Nombre +'%'
    ORDER BY NOMBRE   
 	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END

 