USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[SIS_Lista_Parametros]    Script Date: 21/08/2017 11:49:21 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =====================================================
-- Author:		Douglas Bustos
-- Create date: 25 enero 2017
-- Description:	Listados Generales, filtrados por PadreD
-- =====================================================

 /*
 exec [SIS_Lista_Parametros] 'IDENTIFICACION_PROV'
 */
ALTER PROCEDURE [dbo].[SIS_Lista_Parametros]
	@Codigo char(50) 
AS
BEGIN
	

	SET NOCOUNT ON;

	/*SET  @Codigo='TIPO_IDENTIFICACION'

	SELECT DISTINCT 
		  LTRIM(RTRIM(PAR.ID ))
	FROM [ANGELITO].[dbo].[SIS_PARAMETROS] AS PAR  
	WHERE LTRIM(RTRIM(Código)) = LTRIM(RTRIM(@Codigo))*/

	
    SELECT
		Id		= LTRIM(RTRIM(PAR.ID)),
		Codigo	= PAR.Código,
		Nombre	= LTRIM(RTRIM(UPPER(PAR.Nombre))),
		Tipo	= PAR.Tipo,
		Valor	= PAR.Valor

	FROM [ANGELITO].[dbo].[SIS_PARAMETROS] AS PAR  
	WHERE  LTRIM(RTRIM(PadreID)) =   
	(
	SELECT DISTINCT 
		  LTRIM(RTRIM(PAR.ID ))
	FROM [ANGELITO].[dbo].[SIS_PARAMETROS] AS PAR  
	WHERE Código = LTRIM(RTRIM(@Codigo))
	 )
	--AND Anulado =0 
	ORDER BY PAR.Código

END

