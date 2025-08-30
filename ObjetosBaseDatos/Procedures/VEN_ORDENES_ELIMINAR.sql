USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[VEN_Ordenes_Eliminar]    Script Date: 21/08/2017 11:50:25 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author:		Douglas Bustos
-- Create date: 25 enero 2017
-- Description:	Elimina la orden de pedido
-- ===============================================================

ALTER PROCEDURE [dbo].[VEN_Ordenes_Eliminar]
(
	 @OrdenId varchar(10)
)
AS

SET NOCOUNT ON
 	


	-- EXEC [dbo].[VEN_ORDENES_ELIMINAR] '3'       
 
 BEGIN 
	BEGIN TRANSACTION;
 
				BEGIN TRY 
							DELETE FROM [ANGELITO].[dbo].VEN_ORDENES_DT
											WHERE OrdenId = LTRIM(RTRIM(@OrdenId))

							DELETE FROM [ANGELITO].[dbo].VEN_ORDENES	 
							WHERE ID = LTRIM(RTRIM(@OrdenId))
 
							
				END TRY
				BEGIN CATCH
 
						SELECT  
							ERROR_NUMBER()		AS ErrorNumber 
							,ERROR_SEVERITY()	AS ErrorSeverity 
							,ERROR_STATE()		AS ErrorState 
							,ERROR_PROCEDURE()  AS ErrorProcedure 
							,ERROR_LINE()       AS ErrorLine 
							,ERROR_MESSAGE()    AS ErrorMessage; 
 
							IF @@TRANCOUNT > 0 
											ROLLBACK TRANSACTION; 
 
				END CATCH
			  

				IF @@TRANCOUNT > 0 
				BEGIN
					COMMIT TRANSACTION; 
					SELECT  
						 0		AS ErrorNumber 
						,0		AS ErrorSeverity 
						,0		AS ErrorState 
						,'NN'	AS ErrorProcedure 
						,0           AS ErrorLine 
						,'Operación exitosa'	AS ErrorMessage; 
				END

END

 
