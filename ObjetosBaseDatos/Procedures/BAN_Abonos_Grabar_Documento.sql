USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[BAN_Abonos_Grabar_Documento]    Script Date: 21/08/2017 11:38:41 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos - 
-- Create date: 18 enero 2017
-- Description:	Guarda la cabecera y detalle del abono 
-- =============================================
ALTER PROCEDURE [dbo].[BAN_Abonos_Grabar_Documento]
 @xml XML , @Ban_Ingreso_ID char(10), @CLIENTEID CHAR(10), @DivisiónID	 CHAR(10), @DivisaID	 CHAR(10),  @Cambio DECIMAL (12,6)  ,  @SucursalID  CHAR(2)
  
AS
BEGIN

SET NOCOUNT ON;

 
DECLARE @VENDEDORID							AS CHAR(10)
 
 
DECLARE @Número								AS varchar(17)
DECLARE @Fecha								AS Datetime
 
DECLARE	@Ban_IngresoDT_ID					AS CHAR(10)
 
DECLARE @CodDtIngreso						AS varchar(50)
 
DECLARE @ERROR								AS CHAR(10) 
  
SET @CodDtIngreso	= 'BAN_INGRESOS_DT-ID-00'	--Para la linea de detalle Ban Ingreso
 
SET @Fecha			= GETDATE()
  
    BEGIN TRY
			   	
 				  
					   
		--Codigo de banco : caja general
		--------------------------------
	 
 
					
		-- Abono cabecera
        INSERT INTO [ANGELITO].[dbo].[BAN_INGRESOS]
        SELECT        
        AbonoXml.value('@ID','CHAR(10)')						AS ID,  
		AbonoXml.value('@BancoID','CHAR(10)')					AS BancoID, 
		AbonoXml.value('@AsientoID','CHAR(10)')					AS AsientoID,  
		@CLIENTEID												AS DeudorID, 
		AbonoXml.value('@PcID','VARCHAR(50)')					AS PcID, 
		AbonoXml.value('@ID','CHAR(10)')						AS Número,   
		@Fecha													AS Fecha,   
		AbonoXml.value('@Detalle','VARCHAR(100)')				AS Detalle,
		AbonoXml.value('@Tipo','VARCHAR(10)')					AS Tipo,   
		AbonoXml.value('@RFIR','MONEY')							AS RFIR,   
		AbonoXml.value('@RFIVA','MONEY')						AS RFIVA,   
		AbonoXml.value('@Valor','money')						AS Valor,  					 
		AbonoXml.value('@Descuento','money')					AS Descuento,   
		AbonoXml.value('@Financiero','money')					AS Financiero,   
		AbonoXml.value('@Faltante','money')						AS Faltante,   
		AbonoXml.value('@Sobrante','money')						AS Sobrante,   
		AbonoXml.value('@Valor_Base','money')					AS Valor_Base,   
		0														AS Anulado,   
		AbonoXml.value('@DivisaID', 'CHAR(10)')					AS DivisaID, 
		AbonoXml.value('@Cambio','decimal(12,6)')				AS Cambio,  
		AbonoXml.value('@CreadoPor','VARCHAR(15)')				AS CreadoPor,   
		AbonoXml.value('@Nota','VARCHAR(1024)')					AS Nota,   
		AbonoXml.value('@ExportadoDate', 'datetime')			AS ExportadoDate,
		AbonoXml.value('@EditadoPor', 'VARCHAR(15)')			AS EditadoPor,
		AbonoXml.value('@AnuladoPor' ,'varchar(15)')			AS AnuladoPor,
		NULL													AS AnuladoDate, 
		AbonoXml.value('@AnuladoNota', 'varchar(1024)')			AS AnuladoNota,
		AbonoXml.value('@EditadoDate', 'datetime')				AS EditadoDate,					
		@SucursalID												AS SucursalID,   
		@Fecha													AS CreadoDate,   
		@DivisiónID												AS DivisiónID,  
		AbonoXml.value('@ExportadoPor','varchar(15)' )			AS ExportadoPor,
		0														AS Exportado,
		AbonoXml.value('@ExportadoUpdate','numeric(1, 0)' )		AS ExportadoUpdate,
		AbonoXml.value('@ExportadoCandidate','numeric(1, 0) ')	AS ExportadoCandidate,
		AbonoXml.value('@OrdenID','varchar(10)')				AS OrdenID,
		' '														AS NumRecibo --blanco en la tabla
					
        FROM
        @xml.nodes('/AbonoCabDto')AS TEMPTABLE(AbonoXml)
  
		EXEC   [dbo].[SIS_GetNextIDOUT]   @CodDtIngreso ,1, @Ban_IngresoDT_ID OUT, @ERROR OUT
  
		INSERT INTO [ANGELITO].[dbo].[BAN_INGRESOS_DT]
        SELECT        
        RIGHT(@Ban_IngresoDT_ID,7) + Replicate('0', (3 - len(ltrim(str( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@ID','CHAR(10)') ASC)))))) + ltrim(str( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@ID','CHAR(10)') ASC))) AS ID,
		@Ban_Ingreso_ID												AS IngresoID,   
		@SucursalID													AS SucursalID,   
		AbonoXml.value('@PcID','VARCHAR(50)')						AS PcID, 
		AbonoXml.value('@Tipo','VARCHAR(10)')						AS Tipo,   
		AbonoXml.value('@Numero','VARCHAR(10)')						AS Número,  
		--RIGHT(@Ban_IngresoDT_ID,7) + Replicate('0', (3 - len(ltrim(str( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@ID','CHAR(10)') ASC)))))) + ltrim(str( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@ID','CHAR(10)') ASC))) AS Número,  
		AbonoXml.value('@Banco','VARCHAR(30)')						AS Banco,   
		AbonoXml.value('@Cuenta','VARCHAR(15)')						AS Cuenta,   
		AbonoXml.value('@Girador','VARCHAR(30)')					AS Girador,   
		@Fecha														AS Fecha,   
		AbonoXml.value('@Valor','money')							AS Valor,  
		AbonoXml.value('@Valor_Base','money')						AS Valor_Base,  
		AbonoXml.value('@DifenCambio','money')						AS DifenCambio,  
		AbonoXml.value('@Depositado','money')						AS Depositado,
		-- Diferentes divisas y cambio				 
		AbonoXml.value('@DivisaID', 'CHAR(10)')						AS DivisaID,  
		AbonoXml.value('@Cambio','decimal(12,6)')					AS Cambio,
		--------------------------------
		AbonoXml.value('@ExportadoDate', 'datetime')				AS ExportadoDate,
		AbonoXml.value('@CreadoPor','VARCHAR(15)')					AS CreadoPor,   
		@Fecha														AS CreadoDate,   
		AbonoXml.value('@ExportadoPor','varchar(15)' )				AS ExportadoPor,
		0															AS Exportado,
		AbonoXml.value('@ExportadoUpdate','numeric(1, 0)' )			AS ExportadoUpdate,
		AbonoXml.value('@ExportadoCandidate','numeric(1, 0) ')		AS ExportadoCandidate,
		AbonoXml.value('@Transferido','BIT')						AS Transferido,
		AbonoXml.value('@TransferenciaID','VARCHAR(10)')			AS TransferenciaID,
		AbonoXml.value('@FormaPago','VARCHAR(2) ')					AS FormaPago
					 
        FROM
        @xml.nodes('/AbonoCabDto/Detalles')AS TEMPTABLE(AbonoXml)

					  
    END TRY
    BEGIN CATCH
 
		THROW
 
	END CATCH

  
END

