USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[SRI_TIPOS_IDENTICACION_VENTA_RECUPERAR]    Script Date: 21/08/2017 11:49:35 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Douglas Bustos
-- Create date: 12-06-2017
-- Description:	Proporciona lista de tipos de identificación
-- =============================================
ALTER PROCEDURE [dbo].[SRI_TIPOS_IDENTICACION_VENTA_RECUPERAR] 
	 
 
AS
BEGIN
	 
	SET NOCOUNT ON;
	 
	SELECT 
		srisec.Id, 
		par.Código as Codigo,
		par.Nombre ,
		par.Valor 
	FROM
	[ANGELITO].[dbo].[SRI_SECUENCIAL] srisec
	INNER JOIN  [ANGELITO].[dbo].[SIS_PARAMETROS] par  on par.Código = srisec.tipoIdentificacion
	WHERE srisec.TipoTransaccion=2 and  par.PadreID = '0000002425' order by par.Valor

END
