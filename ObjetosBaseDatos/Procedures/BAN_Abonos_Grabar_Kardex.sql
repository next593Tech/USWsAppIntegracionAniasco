USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[BAN_Abonos_Grabar_Kardex]    Script Date: 21/08/2017 11:39:46 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos - 
-- Create date: 18 enero 2017
-- Description:	Guarda la cabecera y detalle del abono 
-- =============================================
ALTER PROCEDURE [dbo].[BAN_Abonos_Grabar_Kardex]


@xml XML,    @DivisiónID	 CHAR(10), @DivisaID	CHAR(10),@Cambio DECIMAL (12,6)   

AS
BEGIN

SET NOCOUNT ON;
      
DECLARE @Número		AS varchar(17)
DECLARE @Fecha		AS Datetime
DECLARE @ERROR								AS CHAR(10)     

 

 DECLARE @BancoID	CHAR(10)
SET @BancoID = '0000000004'


SET @Fecha			= GETDATE()
  
 
				BEGIN TRY 
				 
					 --Bancos kardex
					
					 
					DECLARE @BANCOSKARDESID		CHAR(10) 
					DECLARE @BAN_BANCOS_CARDEX_ID_00 CHAR(50)
					SET @BAN_BANCOS_CARDEX_ID_00 = 'BAN_BANCOS_CARDEX-ID-00'

					EXEC   [dbo].[SIS_GetNextIDOUT]   @BAN_BANCOS_CARDEX_ID_00,1, @BANCOSKARDESID OUT, @ERROR OUT
					 


					INSERT [ANGELITO].[dbo].[BAN_BANCOS_CARDEX]
					(ID, BancoID, DocumentoID, AsientoID, SucursalID, PcID, DivisaID,Cambio, Fecha, Fecha_Banco,Tipo, Detalle , Débito, Valor,
					 Valor_Base,Conciliado,ExportadoDate ,CreadoPor, CreadoDate,Número,  Cheque, Fecha_Cheque, Beneficiario,DivisiónID,
					 Anulado ,AnuladoPor,AnuladoDate, AnuladoNota, ExportadoPor ,Exportado,ExportadoUpdate, ExportadoCandidate  )
                    SELECT
					@BANCOSKARDESID													AS ID, 
					@BancoID  														AS BancoID,	
					AbonoXml.value('@DocumentoID','char(10)' )						AS DocumentoID,
					AbonoXml.value('@AsientoID','char(10)' )						AS AsientoID,
					AbonoXml.value('@SucursalID','char(2)' )						AS SucursalID,
					AbonoXml.value('@PcID','varchar(50)' )							AS PcID,
					@DivisaID														AS DivisaID,
					@Cambio															AS Cambio,
					@Fecha															AS Fecha,
					@Fecha															AS Fecha_Banco,
					AbonoXml.value('@Tipo','varchar(10)' )							AS Tipo,
					AbonoXml.value('@Detalle','varchar(100)' )						AS Detalle,    
					1																AS Débito,  
					AbonoXml.value('@Valor','money' )								AS Valor,
					AbonoXml.value('@Valor_Base','money' )							AS Valor_Base,
					0																AS Conciliado,
					NULL															AS ExportadoDate,
					NULL															AS CreadoPor,
					@Fecha															AS CreadoDate,
					AbonoXml.value('@Número','char(10)' )							AS Número,
					AbonoXml.value('@Cheque','varchar(10)' )						AS Cheque,
					@Fecha															AS Fecha_Cheque,
					AbonoXml.value('@Beneficiario','varchar(50)' )					AS Beneficiario,
					@DivisiónID														AS DivisiónID,	
					0																AS Anulado,
					NULL															AS AnuladoPor, 
					NULL															AS AnuladoDate,
					NULL															AS AnuladoNota,
					NULL															AS ExportadoPor,
					0																AS Exportado,
					NULL															AS ExportadoUpdate,
					NULL															AS ExportadoCandidate
					
					FROM
                    @xml.nodes('/AbonoCabDto')AS TEMPTABLE(AbonoXml)
					  
					   
             END TRY

             BEGIN CATCH
 
				 THROW 
		     END CATCH
			  

END


					

				 
