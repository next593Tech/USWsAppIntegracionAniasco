USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[FEL_ConsultaDocsV1]    Script Date: 21/08/2017 11:46:55 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- EXEC dbo.FEL_CONSULTADOCS 1,2000,'01/01/2016','01/12/2016',NULL,'0990834369001',NULL
-- EXEC dbo.FEL_CONSULTADOCS 1,20,'01/01/2016','01/08/2016','POS-FA',NULL
-- EXEC dbo.FEL_CONSULTADOCS 1,20,'01/01/2016','01/08/2016',NULL,'001-001-000000157'

-- =============================================
-- Author:		Douglas Bustos
-- Create date: 28 Enero 2017
-- Description:	Consulta documentos de clientes por filtros - Nexo
 
-- =============================================
ALTER PROCEDURE [dbo].[FEL_ConsultaDocsV1] 
  @PageNumber		INT = 1, 
  @PageSize			INT = 20,
  @FechaDesde		VARCHAR(10),
  @FechaHasta		VARCHAR(10),
  @TipoDoc			VARCHAR(10),
  @CodCliente		VARCHAR(15),
  @CodDocumento		VARCHAR(100),
   @TenantId			INT=1
AS
   
 
SELECT
		ROW_NUMBER() OVER(ORDER BY CONVERT(datetime,[FechaEmision], 103) ASC) AS Id
	  , [TipoDoc]
      ,[Ruc]
      ,[Nombre]
      ,[Documento]
      ,CONVERT(varchar,[FechaEmision], 103) as FechaEmision
      ,[TipoFac]
      ,[Autorizacion]
      ,CONVERT(varchar,[FechaAutoriza], 103) as FechaAutoriza
      ,[Total]
      ,[EMAIL] 
FROM  [ANGELITO].[dbo].[FEL_DOCUMENTOS_WEB] 
WHERE [FechaEmision] BETWEEN CONVERT(datetime,@FechaDesde, 103) AND CONVERT(datetime,@FechaHasta, 103)
	  AND [RUC] = LTRIM(RTRIM(@CodCliente))
	  AND [TipoDoc] =
	  CASE WHEN COALESCE(@TipoDoc,'')<>'' THEN @TipoDoc
	  ELSE [TipoDoc] END
	  AND [Documento] =
	  CASE WHEN COALESCE(@CodDocumento,'')<>'' THEN @CodDocumento
	  ELSE [Documento] END
ORDER BY  [FechaEmision]  ASC
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;

 

