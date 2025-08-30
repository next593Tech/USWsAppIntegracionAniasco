USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[SIS_GetNextIdOut]    Script Date: 21/08/2017 11:49:02 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


 
ALTER PROCEDURE [dbo].[SIS_GetNextIdOut] @CodContador varchar(50), @Incremento INT = 1,  @NextID CHAR(10) OUTPUT, @ErrorCode varchar(256) OUTPUT

AS
		BEGIN

		DECLARE  @IDTMP CHAR(10)  
	
		SET NOCOUNT ON;

		  		SET @IDTMP = 1
				SET  @IDTMP = (SELECT ( Valor + @Incremento ) FROM [ANGELITO].[dbo].SIS_CONTADORES   WHERE Código = @CodContador )
				IF ( @@rowcount > 0 ) 
				UPDATE [ANGELITO].[dbo].SIS_CONTADORES SET Valor = @IDTMP , ExportadoUpdate = 1 WHERE Código = @CodContador 
				ELSE
				INSERT INTO [ANGELITO].[dbo].SIS_CONTADORES ( Código, Valor ) VALUES ( UPPER(@CodContador), @IDTMP )
				SET @NextID  = Replicate('0', (10 - len(ltrim(str(@IDTMP))))) + ltrim(str(@IDTMP))  

				--SELECT @NextID, @NextID2
				  
				IF @@ERROR>0 SET @ErrorCode = 'ERRORDB0001'
					 	 
		END
