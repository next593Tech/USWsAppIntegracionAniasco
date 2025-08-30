USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[BAN_Abonos_Grabar_Ing_Deudas]    Script Date: 21/08/2017 11:39:26 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos - 
-- Create date: 18 enero 2017
-- Description:	Guarda la cabecera y detalle del abono 
-- =============================================
ALTER PROCEDURE [dbo].[BAN_Abonos_Grabar_Ing_Deudas]
@xml XML, @Ban_Ingreso_ID char(10),  @DivisiónID	 CHAR(10), @SucursalID	    AS  CHAR(2),
@RubroID		AS  CHAR(10), @CtaCxCID		AS  CHAR(10), @Tipo		    AS  CHAR(10)
  
AS
BEGIN

SET NOCOUNT ON;
      
DECLARE @Número		AS varchar(17)
DECLARE @Fecha		AS Datetime
    

	

  
   
SET @Fecha			= GETDATE()
  
 
				BEGIN TRY 

					--Distribución
					 
					INSERT INTO [ANGELITO].[dbo].[BAN_INGRESOS_DEUDAS]
                    SELECT        
					@Ban_Ingreso_ID											AS IngresoID, 
					AbonoXml.value('@DeudaID','VARCHAR(10)')				AS DeudaID,  
                    @SucursalID												AS SucursalID,   
					AbonoXml.value('@PcID','VARCHAR(50)')					AS PcID, 
					AbonoXml.value('@Abono','money')						AS Valor, 
					AbonoXml.value('@Cambio','decimal(12,6)')				AS Cambio, 
					AbonoXml.value('@DivisaID', 'CHAR(10)')					AS DivisaID,
					AbonoXml.value('@Saldo','money') 	 					AS Saldo,
					0														AS DifenCambio,
					@Fecha													AS Fecha, 
					AbonoXml.value('@Vencimiento','datetime')				AS Vencimiento,   
					@Tipo													AS Tipo,   
					@Ban_Ingreso_ID											AS Número,  
					AbonoXml.value('@Detalle','VARCHAR(100)')				AS Detalle, 
					AbonoXml.value('@Crédito','BIT')						AS Crédito,
					 @RubroID												AS RubroID,
					 @CtaCxCID												AS CtaCxCID,
					1														AS CambioDia, 
					@DivisiónID												AS DivisiónID,
					NULL													AS ExportadoDate,
					AbonoXml.value('@CreadoPor','VARCHAR(15)')				AS CreadoPor,   
					@Fecha													AS CreadoDate,  
					AbonoXml.value('@ExportadoPor','varchar(15)' )			AS ExportadoPor,
					0														AS Exportado,
					AbonoXml.value('@ExportadoUpdate','numeric(1, 0)' )		AS ExportadoUpdate,
					AbonoXml.value('@ExportadoCandidate','numeric(1, 0) ')	AS ExportadoCandidate
					
                    FROM
                    @xml.nodes('/AbonoCabDto/Distribucion')AS TEMPTABLE(AbonoXml)
					WHERE AbonoXml.value('@Abono','money') >0

					   
             END TRY

             BEGIN CATCH
 
				 THROW 
		     END CATCH
			  

END

