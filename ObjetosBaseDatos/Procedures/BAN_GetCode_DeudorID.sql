USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[BAN_GetCode_DeudorID]    Script Date: 21/08/2017 11:41:19 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Adela Lorena Cujilema
-- Create date: 16 enero 2017
-- Description:	Obtener ID de cliente en el caso de DeudorID
-- =============================================
ALTER PROCEDURE [dbo].[BAN_GetCode_DeudorID]
	@DeudorID as CHAR(10), 
	@ErrorCode varchar(256) OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;

	BEGIN TRANSACTION;
 
             BEGIN TRY

			 --1 Obtener ID de cliente en el caso de DeudorID
			SET @DeudorID = (SELECT ID FROM [ANGELITO].[dbo].[CLI_CLIENTES] AS CLI	WHERE (CLI.Ruc = @DeudorID) )
			select @DeudorID as DeudorID

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

