USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[SIS_GetNextId]    Script Date: 21/08/2017 11:48:48 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- =====================================================
-- Author:		Douglas Bustos
-- Create date: 25 enero 2017
-- Description:	Recupera secuencia para documentos
-- =====================================================
 
ALTER PROCEDURE [dbo].[SIS_GetNextId] @Contador varchar(50)  ,
@ErrorCode varchar(256) OUTPUT

AS

/* 
declare @ErrorCode varchar(256)  
EXEC [dbo].[SIS_GetNextID] 'VEN_ORDENES-ID-00', @ErrorCode OUT

*/
		BEGIN
	
		SET NOCOUNT ON;

		BEGIN TRANSACTION;
 
					BEGIN TRY	


				DECLARE @NextID numeric(10)

					SELECT @NextID = 1
					SELECT @NextID = ( Valor + 1 ) FROM [ANGELITO].[dbo].SIS_CONTADORES   WHERE Código = @Contador -- WITH(NoLock): Recomienda Eliminar
					IF ( @@rowcount > 0 ) 
						UPDATE [ANGELITO].[dbo].SIS_CONTADORES SET Valor = @NextID , ExportadoUpdate = 1 WHERE Código = @Contador 
					ELSE
						INSERT INTO [ANGELITO].[dbo].SIS_CONTADORES ( Código, Valor ) VALUES ( UPPER(@Contador), @NextID )
				SELECT  Replicate('0', (10 - len(ltrim(str(@NextID))))) + ltrim(str(@NextID)) NextID

				  
				--return @NextID    ---- nueva linea

 
				END TRY
				BEGIN CATCH
					SET @ErrorCode = 'ERRORDB0001'
					IF @@TRANCOUNT > 0 
							ROLLBACK TRANSACTION; 					
				END CATCH
 

				IF @@TRANCOUNT > 0 
				BEGIN
					SET @ErrorCode = 'EMPTY00000'
					COMMIT TRANSACTION;                    
				END
		END
