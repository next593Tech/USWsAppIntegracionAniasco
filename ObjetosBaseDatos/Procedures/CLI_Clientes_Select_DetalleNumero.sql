USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[CLI_Clientes_Select_DetalleNumero]    Script Date: 21/08/2017 11:42:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Adela Lorena Cujilema
-- Create date: 24 enero 2017
-- Description:	Obtener detalle y numero de documento abonos
-- =============================================
ALTER PROCEDURE [dbo].[CLI_Clientes_Select_DetalleNumero]
	@codCliente varchar(13),
	@IdClienteDeuda char(10) -- seria una lista de ID cliente deudas

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--For cada ID, guarda el ID en CANCELA:
	---------------- DETALLE -------------
		select  Deu.Número, CLi.Nombre+ ' Cancela: '+DEU.Tipo+' '+DEU.Número + '1'+ '2' AS Detalle
		from [MINIMARKET].[dbo].[CLI_CLIENTES] as CLI
		inner join [MINIMARKET].[dbo].[CLI_CLIENTES_DEUDAS] as DEU ON CLI.ID = DEU.ClienteID
		where CLi.RUC = @codCliente		--'0102057114'   
		AND DEU.ID = @IdClienteDeuda	--'0010002655'	

END

