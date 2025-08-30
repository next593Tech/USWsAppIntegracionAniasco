USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[BAN_Abonos_Grabar_Asiento]    Script Date: 21/08/2017 11:37:39 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos - 
-- Create date: 18 enero 2017
-- Description:	Guarda la cabecera y detalle del abono 
-- =============================================
ALTER PROCEDURE [dbo].[BAN_Abonos_Grabar_Asiento]
@xml XML ,@Ban_Ingreso_ID char(10), @CLIENTEID	 CHAR(10), @DivisaID	CHAR(10), @Cambio DECIMAL (12,6), @DivisiónID	 CHAR(10) 
 , @Asiento_ID		 CHAR(10) OUT,  @SucursalID 	    AS  CHAR(2)
AS
BEGIN

SET NOCOUNT ON;
  
DECLARE @Número								AS varchar(17)
DECLARE @Fecha								AS Datetime

 
DECLARE @AsientoDT_ID						AS CHAR(10)
DECLARE @CodCabAsiento						AS varchar(50)
DECLARE @CodDtAsiento						AS varchar(50)

DECLARE @Codigo_Caja_General				AS CHAR(10)
DECLARE @Codigo_Docum_CtaXCobrar_Cliente	AS CHAR(10)
 
DECLARE @ERROR								AS CHAR(10) 

SET @CodCabAsiento	= 'ACC_ASIENTOS-ID-00'   
SET @CodDtAsiento	= 'ACC_ASIENTOS_DT-ID-00'  
 

DECLARE @BancoID	CHAR(10)
SET @BancoID = '0000000004'
 
SET @Fecha			= GETDATE()
 
 -- select * from [ANGELITO].[dbo].[ACC_ASIENTOS]
 -- select CuentaId,cta.* from [ANGELITO].[dbo].[ACC_ASIENTOS_DT] dt inner join [ANGELITO].dbo.ACC_CUENTAS cta on dt.CuentaId = cta.Id
	
SET @Codigo_Caja_General = '0000000005' -- buscar parametro
SET @Codigo_Docum_CtaXCobrar_Cliente = '0000000025' -- buscar parametro
 
  
             BEGIN TRY


			 EXEC   [dbo].[SIS_GetNextIDOUT]   @CodCabAsiento,1, @Asiento_ID OUT, @ERROR OUT

				
  
					--Asiento cab
					INSERT INTO [ANGELITO].[dbo].[ACC_ASIENTOS]
                    SELECT        
					@Asiento_ID									AS	ID, 
					@Ban_Ingreso_ID								AS	DocumentoID,   
					@Asiento_ID									AS	Número, 
					@Fecha										AS	Fecha, 
					AbonoXml.value('@Detalle','VARCHAR(100)')	AS	Detalle, 
					'CLI-IN'									AS	Tipo, 
					NULL										AS	Nota,
					0											AS	Anulado,
					0											AS	Pendiente,
					NULL										AS	ExportadoDate,
					NULL										AS	CreadoPor, 
					NULL										AS	EditadoPor, 
					NULL										AS	AnuladoPor, 
					NULL										AS	PCID, 
                    @SucursalID									AS	SucursalID,   
					@Fecha										AS	CreadoDate,
					NULL										AS	EditadoDate,
					NULL										AS	AnuladoDate, 
					NULL										AS	AnuladoNota,
					@DivisiónID									AS	DivisiónID,
					NULL										AS	ExportadoPor,
					0											AS	Exportado,
					0											AS	ExportadoUpdate,
					0											AS	ExportadoCandidate
                    FROM
                    @xml.nodes('/AbonoCabDto')AS TEMPTABLE(AbonoXml)


					--Update cab Abono
					Update [ANGELITO].[dbo].BAN_INGRESOS SET AsientoID = @Asiento_ID WHERE ID = @Ban_Ingreso_ID

					  
					EXEC   [dbo].[SIS_GetNextIDOUT]   @CodDtAsiento,1, @AsientoDT_ID OUT, @ERROR OUT
					 

					DECLARE @REGISTROSDETASIENTOS INT

				--Asiento DT
					INSERT INTO [ANGELITO].[dbo].[ACC_ASIENTOS_DT]
					(ID,AsientoID,CuentaID,SucursalID,Débito,Valor,Detalle,	ExportadoDate,PCID,Valor_Base,CreadoPor,
						DivisaID,Cambio ,CreadoDate  ,ExportadoPor ,Exportado,ExportadoUpdate,ExportadoCandidate ,
						Anulado,EditadoPor )
                    SELECT        
					RIGHT(@AsientoDT_ID,7) + Replicate('0', (3 - len(ltrim(str( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@Valor','money') ASC)))))) + ltrim(str( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@Valor','money') ASC))) AS ID,
					@Asiento_ID												AS AsientoID,
					@Codigo_Caja_General									AS CuentaID,
					@SucursalID												AS SucursalID, 
					1														AS Débito,
					AbonoXml.value('@Valor','money')						AS Valor, 
					AbonoXml.value('@Detalle','VARCHAR(100)')				AS Detalle, 
					NULL													AS ExportadoDate,
					NULL													AS PCID, 
					AbonoXml.value('@Valor_Base','money')					AS Valor_Base,
					AbonoXml.value('@CreadoPor','VARCHAR(15)')				AS CreadoPor,
					@DivisaID												AS DivisaID, 
					@Cambio													AS Cambio,
					@Fecha													AS CreadoDate,
					NULL													AS ExportadoPor,
					0														AS Exportado,
					0														AS ExportadoUpdate,
					0														AS ExportadoCandidate,
					0														AS Anulado,
					NULL													AS EditadoPor
                    FROM
                    @xml.nodes('/AbonoCabDto/Detalles')AS TEMPTABLE(AbonoXml)
					

					SET @REGISTROSDETASIENTOS = (SELECT COUNT(*) AS CANTIDADDETALLES FROM  @xml.nodes('/AbonoCabDto/Detalles')AS TEMPTABLE(AbonoXml))
					  

					INSERT INTO [ANGELITO].[dbo].[ACC_ASIENTOS_DT]	
					(ID,AsientoID,CuentaID,SucursalID,Débito,Valor,Detalle,	ExportadoDate,PCID,Valor_Base,CreadoPor,
					DivisaID,Cambio ,CreadoDate  ,ExportadoPor ,Exportado,ExportadoUpdate,ExportadoCandidate ,
					Anulado,EditadoPor )
					SELECT        
					RIGHT(@AsientoDT_ID,7) + Replicate('0', (3 - len(@REGISTROSDETASIENTOS))) + ltrim(str( @REGISTROSDETASIENTOS+1))  AS ID,
					@Asiento_ID												AS AsientoID,
					@Codigo_Docum_CtaXCobrar_Cliente						AS CuentaID,
					@SucursalID												AS SucursalID, 
					0														AS Débito,
					AbonoXml.value('@Valor','money')						AS Valor, 
					AbonoXml.value('@Detalle','VARCHAR(100)')				AS Detalle, 
					NULL													AS ExportadoDate,
					NULL													AS PCID, 
					AbonoXml.value('@Valor_Base','money')					AS Valor_Base,
					AbonoXml.value('@CreadoPor','VARCHAR(15)')				AS CreadoPor,
					@DivisaID												AS DivisaID, 
					@Cambio													AS Cambio,
					@Fecha													AS CreadoDate,
					NULL													AS ExportadoPor,
					0														AS Exportado,
					0														AS ExportadoUpdate,
					0														AS ExportadoCandidate,
					0														AS Anulado,
					NULL													AS EditadoPor
                    FROM
                    @xml.nodes('/AbonoCabDto')		AS			TEMPTABLE(AbonoXml)
					 
		
					EXEC   [dbo].[SIS_GetNextIDOUT]   @CodDtAsiento,@REGISTROSDETASIENTOS, @AsientoDT_ID OUT, @ERROR OUT

					  

             END TRY
             BEGIN CATCH
					THROW
 
		     END CATCH

 
  

END

