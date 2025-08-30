USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[SIS_Divisas_Recuperar]    Script Date: 21/08/2017 11:48:30 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos
-- Create date: 20 enero 2017
-- Description:	Listado de divisas ID y CAMBIO
-- =============================================
ALTER PROCEDURE [dbo].[SIS_Divisas_Recuperar]
AS
BEGIN
	
	SET NOCOUNT ON;
	   
			 SELECT  
			 	Id		=LTRIM(RTRIM( DIV.ID)),
				Codigo	= '',
				Nombre	= DIV.Nombre,
				Tipo	= '',
				Valor	= CONVERT(VARCHAR(10),DIV.Cambio)

			FROM [ANGELITO].[dbo].[SIS_DIVISAS] DIV ORDER BY 1

		 
END
