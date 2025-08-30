USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[BAN_Abonos_Grabar_Cli_Deudas]    Script Date: 21/08/2017 11:38:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos - 
-- Create date: 18 enero 2017
-- Description:	Guarda la cabecera y detalle del abono 
-- =============================================
ALTER PROCEDURE [dbo].[BAN_Abonos_Grabar_Cli_Deudas]


@xml XML, @Ban_Ingreso_ID char(10),  @DivisiónID	 CHAR(10),@Asiento_ID	 CHAR(10),
@DivisaID	CHAR(10),@Cambio DECIMAL (12,6)  , @CLIENTEID	 CHAR(10), @SucursalID  CHAR(2),
@RubroID		AS  CHAR(10), @CtaCxCID		AS  CHAR(10), @Tipo		    AS  CHAR(10)
  
AS
BEGIN

SET NOCOUNT ON;
      
DECLARE @Número		AS varchar(17)
DECLARE @Fecha		AS Datetime
DECLARE @ERROR								AS CHAR(10)     

 
SET @Fecha			= GETDATE()
  
 
				BEGIN TRY 
				 
					--Clientes deudas
					

					DECLARE @CLIENTE_DEUDA_ID_STR	CHAR(10)
					DECLARE @INDEXROW				INT

					DECLARE @CLI_CLIENTES_DEUDAS_ID_00			AS CHAR(50)
					SET @CLI_CLIENTES_DEUDAS_ID_00 = 'CLI_CLIENTES_DEUDAS-ID-00'
		 
					EXEC   [dbo].[SIS_GetNextIDOUT]   @CLI_CLIENTES_DEUDAS_ID_00,1, @CLIENTE_DEUDA_ID_STR OUT, @ERROR OUT

					

					SET @INDEXROW =	CONVERT(CHAR(10), CONVERT(int , @CLIENTE_DEUDA_ID_STR)  )
					--select @INDEXROW,@CLIENTE_DEUDA_ID_STR
  
					INSERT [ANGELITO].[dbo].[CLI_CLIENTES_DEUDAS]
                    (ID, 
					ClienteID, 
					DocumentoID, 
					AsientoID, 
					Número, 
					Detalle, 
					Valor, 
					ValorBase, 
					Fecha, 
					Vencimiento, 
					RubroID, 
					CtaCxCID, 
					DivisaID,
					 Cambio,
					 Saldo, 
					 Tipo, 
					 Crédito, 
					 Facturado, 
					 DeudaID,
					 CambioDeuda, 
					 VendedorID,
					 Anulado, 
					 ExportadoDate,
					 CreadoPor,
					 CreadoDate,
					 EditadoPor, 
					 EditadoDate, 
					 AnuladoPor, 
					 AnuladoDate, 
					 AnuladoNota, 
					 SucursalID, 
					 PCID, 
					 DivisiónID,
					 Retenido  , 
					 ExportadoPor  , 
					 Exportado ,  
					 ExportadoUpdate ,
					 ExportadoCandidate    ,  
					 MesArriendo , 
					 ContratoID,  
					 Inmobiliaria )
					 SELECT
					(Replicate('0', (10 - len(ltrim(str(( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@ID','CHAR(10)') ASC) + @INDEXROW)) )))))+
					(CONVERT(CHAR(10),( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@ID','CHAR(10)') ASC) + @INDEXROW))  ) AS ID,  

					@CLIENTEID												AS ClienteID,	
					AbonoXml.value('@IngresoID','char(10)' )				AS DocumentoID,	
					@Asiento_ID												AS AsientoID,	
						
					(Replicate('0', (10 - len(ltrim(str(( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@ID','CHAR(10)') ASC) + @INDEXROW)) )))))+
					(CONVERT(CHAR(10),( ROW_NUMBER() OVER(ORDER BY AbonoXml.value('@ID','CHAR(10)') ASC) + @INDEXROW))  )	AS Número,	
					
					AbonoXml.value('@Detalle','varchar(100)' )				AS Detalle,	
					AbonoXml.value('@Abono','money' )						AS Valor,	     
					AbonoXml.value('@Abono','money' )						AS ValorBase,	
					@Fecha													AS Fecha,	
					AbonoXml.value('@Vencimiento','datetime' )				AS Vencimiento,	
					@RubroID												AS RubroID,	
					@CtaCxCID												AS CtaCxCID,	
					@DivisaID												AS DivisaID,
					@Cambio													AS Cambio,	
					 0													  	AS Saldo,	
					@Tipo													AS Tipo,	
					1														AS Crédito,	
					0														AS Facturado,	
					AbonoXml.value('@DeudaID','varchar(10)')				AS DeudaID,	
					1														AS CambioDeuda,	 
					AbonoXml.value('@VendedorID','varchar(10)')				AS VendedorID,
					0														AS Anulado,
					NULL													AS ExportadoDate, 
					AbonoXml.value('@CreadoPor','VARCHAR(15)')				AS CreadoPor,          
					@Fecha													AS CreadoDate,
					NULL													AS EditadoPor, 
					NULL													AS EditadoDate, 
					NULL													AS AnuladoPor, 
					NULL													AS AnuladoDate, 
					NULL													AS AnuladoNota,
					@SucursalID												AS SucursalID,  
					AbonoXml.value('@PCID','VARCHAR(50)')					AS PCID, 
					@DivisiónID												AS DivisiónID,
					--Secuencia is timestamp column
					0														AS Retenido,
					NULL													AS ExportadoPor,
					0														AS Exportado,
					NULL													AS ExportadoUpdate,
					0														AS ExportadoCandidate,
					NULL													AS MesArriendo,
					NULL													AS ContratoID,
					NULL													AS Inmobiliaria
					
					FROM
                    @xml.nodes('/AbonoCabDto/Distribucion')AS TEMPTABLE(AbonoXml)
					WHERE AbonoXml.value('@Abono','money') >0


					DECLARE @REGISTROSDETDIST INT

					SET @REGISTROSDETDIST = (SELECT COUNT(*) AS CANTIDADDETALLES FROM  @xml.nodes('/AbonoCabDto/Distribucion')AS TEMPTABLE(AbonoXml) WHERE AbonoXml.value('@Abono','money') >0)
				
				 
					EXEC   [dbo].[SIS_GetNextIDOUT]   @CLI_CLIENTES_DEUDAS_ID_00,@REGISTROSDETDIST, @CLIENTE_DEUDA_ID_STR OUT, @ERROR OUT


					-- ACTUALIZAR SALDO
					
					UPDATE  DE
						SET SALDO = AbonoXml.value('@Saldo','money') - (AbonoXml.value('@Abono', 'money') * @cambio / DI.Cambio)  		 			 
					FROM  @xml.nodes('/AbonoCabDto/Distribucion') AS TEMPTABLE(AbonoXml)
					INNER JOIN [ANGELITO].[dbo].[SIS_DIVISAS] DI
					ON DI.ID = AbonoXml.value('@DivisaID', 'CHAR(10)')
					INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES_DEUDAS] DE 
					ON  DE.DivisaID = DI.ID AND DE.ID = AbonoXml.value ('@DeudaID','VARCHAR(10)') 
					WHERE AbonoXml.value('@Abono','money') >0
			  

					   
             END TRY

             BEGIN CATCH
 
				 THROW 
		     END CATCH
			  

END


					

				 
