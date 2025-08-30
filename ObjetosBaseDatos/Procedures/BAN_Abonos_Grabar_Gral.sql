USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[BAN_Abonos_Grabar_Gral]    Script Date: 21/08/2017 11:39:05 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Douglas Bustos - 
-- Create date: 18 enero 2017
-- Description:	Guarda la cabecera y detalle del abono 
-- =============================================
ALTER PROCEDURE [dbo].[BAN_Abonos_Grabar_Gral]
@xml XML
  
AS
BEGIN

SET NOCOUNT ON;


DECLARE @Ban_Ingreso_ID char(10)			= NULL		 
DECLARE	@CLIENTECOD		 char(25)			= NULL 		 
DECLARE @CLIENTEID							AS CHAR(10)
 

DECLARE @DivisiónID							AS CHAR(10)
DECLARE @DivisaID							AS CHAR(10)
DECLARE @Número								AS varchar(17)
DECLARE @Fecha								AS Datetime

DECLARE @Asiento_ID							AS CHAR(10)
  
DECLARE @ERROR								AS CHAR(10) 


DECLARE @RubroID		AS  CHAR(10)
	DECLARE @CtaCxCID		AS  CHAR(10)
	DECLARE @Tipo		    AS  CHAR(10)
	
	

 SET @RubroID  = '0000000001'
 SET @CtaCxCID = '0000000025'
 SET @Tipo     ='CLI-IN'




 DECLARE @SucursalID	    AS  CHAR(2)
 SET @SucursalID ='01'
 
SET @DivisiónID		= '0000000001'
SET @DivisaID		= '0000000001'
SET @Fecha			= GETDATE()


DECLARE @Cambio AS DECIMAL (12,6)  
SET @DivisaID = (SELECT ID  FROM [ANGELITO].[dbo].[SIS_DIVISAS] where ID = @DivisaID)  
SET @Cambio = (SELECT Cambio  FROM [ANGELITO].[dbo].[SIS_DIVISAS] where ID = @DivisaID)  
 
	
SET @Ban_Ingreso_ID = (SELECT  AbonoXml.value('@ID','CHAR(10)')	AS ID FROM
                    @xml.nodes('/AbonoCabDto')AS TEMPTABLE(AbonoXml))


SET @CLIENTECOD = (SELECT  AbonoXml.value('@CodCliente','CHAR(25)')	AS CodCliente FROM
@xml.nodes('/AbonoCabDto')AS TEMPTABLE(AbonoXml))

SET @CLIENTEID = (SELECT ID FROM [ANGELITO].[dbo].[CLI_CLIENTES] AS CLI	WHERE (CLI.Ruc = @CLIENTECOD) )
	
	--SELECT ID FROM [ANGELITO].[dbo].[CLI_CLIENTES] AS CLI	WHERE (CLI.Ruc = '0602652737')	 
		 

IF exists (SELECT top 1 *  FROM [ANGELITO].[dbo].[BAN_INGRESOS]  where ID = @Ban_Ingreso_ID)
 BEGIN
		--RAISEERROR (N'El código ingresado ya existe', 16, 1)
		--SELECT Error = 1
		--Return 0
		 SELECT  
                     0           AS ErrorNumber 
                    ,0			 AS ErrorSeverity 
                    ,0           AS ErrorState 
                    ,'El código ingresado ya existe'  AS ErrorProcedure 
                    ,0           AS ErrorLine 
                    ,'El código ingresado ya existe'  AS ErrorMessage; 
 END
 ELSE

       BEGIN TRANSACTION;
 
             BEGIN TRY
              
				  EXEC BAN_ABONOS_GRABAR_DOCUMENTO		@xml,  @Ban_Ingreso_ID, @CLIENTEID , @DivisiónID, @DivisaID	,  @Cambio , @SucursalID
			  
				  EXEC BAN_ABONOS_GRABAR_ING_DEUDAS		@xml , @Ban_Ingreso_ID,  @DivisiónID, @SucursalID, @RubroID,@CtaCxCID,@Tipo	
			   
				  EXEC BAN_ABONOS_GRABAR_ASIENTO		@xml , @Ban_Ingreso_ID, @CLIENTEID	, @DivisaID	 , @Cambio  , @DivisiónID, @Asiento_ID OUT, @SucursalID	 

				  EXEC BAN_ABONOS_GRABAR_CLI_DEUDAS	    @xml , @Ban_Ingreso_ID, @DivisiónID,@Asiento_ID, @DivisaID, @Cambio , @CLIENTEID,  @SucursalID , @RubroID ,@CtaCxCID,@Tipo	

				  EXEC BAN_ABONOS_GRABAR_KARDEX			@xml , @DivisiónID, @DivisaID ,@Cambio  

					    
             END TRY
             BEGIN CATCH
 
					 SELECT  
							ERROR_NUMBER()				AS ErrorNumber 
							,ERROR_SEVERITY()			AS ErrorSeverity 
							,ERROR_STATE()				AS ErrorState 
							,ERROR_PROCEDURE()			AS ErrorProcedure 
							,ERROR_LINE()				AS ErrorLine 
							,ERROR_MESSAGE()			AS ErrorMessage; 
 
							IF @@TRANCOUNT > 0 
										  ROLLBACK TRANSACTION; 
 
		     END CATCH

 

			IF @@TRANCOUNT > 0 
             BEGIN
                    COMMIT TRANSACTION; 
                    SELECT  
                     0						AS ErrorNumber 
                    ,0						AS ErrorSeverity 
                    ,0						AS ErrorState 
                    ,'NN'					AS ErrorProcedure 
                    ,0						AS ErrorLine 
                    ,'Abono registrado exitosamente'		AS ErrorMessage; 
             END

END


