USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[VEN_Ordenes_Grabar]    Script Date: 21/08/2017 11:50:46 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author:		Douglas Bustos
-- Create date: 24 enero 2017
-- Description:	Guarda la cabecera y detalle de la orden de pedido
-- ===============================================================

ALTER PROCEDURE [dbo].[VEN_Ordenes_Grabar]
(
	 @infoXml				XML
)
AS

SET NOCOUNT ON
 	
DECLARE @DivisiónID	CHAR(10)		
DECLARE @DivisaID   CHAR(10)	
DECLARE @Cambio		DECIMAL(12,6) 
DECLARE @ID INT
DECLARE @FECHA		DATETIME
DECLARE @USUARIOID	VARCHAR(15)
DECLARE @SucursalID VARCHAR(2)
DECLARE @TIPODOC	VARCHAR(25)

DECLARE @VENDEDORID			VARCHAR(10)
DECLARE @VENDEDORUSERNAME	VARCHAR(15)
 

SET @SucursalID='00'
SET @TIPODOC='VEN-OR-FA'

SET @ID = (SELECT ID = VEN.X.value('@ID','char(10)') FROM	@infoXml.nodes('/VEN_ORDENES_DTO') AS VEN(X))

SET @VENDEDORUSERNAME = (SELECT VENDEDORUSERNAME = VEN.X.value('@VendedorUsername','char(15)') FROM	@infoXml.nodes('/VEN_ORDENES_DTO') AS VEN(X))

 
-- Validación de estado
IF EXISTS(SELECT 1 FROM [ANGELITO].[dbo].VEN_ORDENES ORD WHERE ORD.ID = @ID AND ORD.Estado NOT IN ('BORRADOR','PENDIENTE','PREPARACIO'))
BEGIN
					SELECT  
                     16		AS ErrorNumber 
                    ,0		AS ErrorSeverity 
                    ,0		AS ErrorState 
                    ,'NN'	AS ErrorProcedure 
                    ,0           AS ErrorLine 
                    ,'El documento no puede modificarse'	AS ErrorMessage; 

					RAISERROR('El documento no puede modificarse',16,1)
					RETURN
END


SET @VENDEDORID = (SELECT EMP.ID FROM [ANGELITO].[dbo].EMP_EMPLEADOS EMP 
					INNER JOIN [ANGELITO].[dbo].SEG_USUARIOS SEG
					ON SEG.EmpleadoID = EMP.ID
					WHERE SEG.Código = @VENDEDORUSERNAME)
 
SET @DivisiónID = '0000000001'
SET @DivisaID   = '0000000001'
SET @Cambio		= 1.000000
SET @FECHA		= GETDATE()
--SET @USUARIOID	= 'DBUSTOS'

BEGIN TRANSACTION;
 
   BEGIN TRY

			IF NOT EXISTS(SELECT 1 FROM [ANGELITO].[dbo].VEN_ORDENES WHERE ID = @ID)   
				BEGIN
					INSERT INTO [ANGELITO].[dbo].VEN_ORDENES
					(
							 ID
							,ClienteID
							,Detalle
							,VendedorID
							,TérminoID
							,DivisiónID
							,Número
							,Fecha
							,Entregado
							,Tipo
							,DivisaID
							,Cambio
							,Subtotal
							,Descuento
							,Impuesto
							,Total
							,Nota
							,Nota2
							,Estado
							,Negado
							,Aprobado
							,AprobadoPor
							,AprobadoDate
							,AprobadoNota
							,Egresado
							,Facturado
							,FacturadoPor
							,FacturadoDate
							,CreadoPor
							,CreadoDate
							,Anulado
							,AnuladoPor
							,AnuladoDate
							,AnuladoNota
							,EditadoPor
							,EditadoDate
							,ExportadoDate
							,SucursalID
							,PcID
							,AsientoID
							,ExportadoPor
							,Exportado
							,ExportadoUpdate
							,ExportadoCandidate
							,CréditoPor
							,CréditoNota
							,CréditoDate
					)
					SELECT
					ID						= CAB.X.value('@ID','char(10)')
					,ClienteID				= CAB.X.value('@ClienteID','char(10)')
					,Detalle				= CAB.X.value('@ClienteNombre','varchar(200)') + ' - ' + CAB.X.value('@ID','char(10)')--CAB.X.value('@Detalle','varchar(50)')
					,VendedorID				= @VENDEDORID --CAB.X.value('@VendedorID','char(10)')
					,TérminoID				= CAB.X.value('@TerminoID','char(10)')
					,DivisiónID				= @DivisiónID --CAB.X.value('@DivisiónID','char(10)') 
					,Número					= CAB.X.value('@Número','char(10)')
					,Fecha					= @FECHA --CAB.X.value('@Fecha','datetime')
					,Entregado				= CAB.X.value('@Entregado','datetime')
					,Tipo					= @TIPODOC --CAB.X.value('@Tipo','varchar(10)')
					,DivisaID				= @DivisaID--CAB.X.value('@DivisaID','char(10)')
					,Cambio					= @Cambio --CAB.X.value('@Cambio','decimal(12,6)')
					,Subtotal				= CAB.X.value('@Subtotal','money')
					,Descuento				= CAB.X.value('@Descuento','money')
					,Impuesto				= CAB.X.value('@Impuesto','money')
					,Total					= CAB.X.value('@Total','money')
					,Nota					= CAB.X.value('@Nota','varchar(1024)')
					,Nota2					= CAB.X.value('@Nota2','varchar(1024)')
					,Estado					= CAB.X.value('@Estado','varchar(10)')
					,Negado					= 0--CAB.X.value('@Negado','bit')
					,Aprobado				= 1 --CAB.X.value('@Aprobado','bit')
					,AprobadoPor			= CAB.X.value('@AprobadoPor','varchar(15)')
					,AprobadoDate			= CAB.X.value('@AprobadoDate','datetime')
					,AprobadoNota			= CAB.X.value('@AprobadoNota','varchar(1024)')
					,Egresado				= 0--CAB.X.value('@Egresado','bit')
					,Facturado				= 0--CAB.X.value('@Facturado','bit')
					,FacturadoPor			= CAB.X.value('@FacturadoPor','varchar(15)')
					,FacturadoDate			= CAB.X.value('@FacturadoDate','datetime')
					,CreadoPor				= CAB.X.value('@CreadoPor','varchar(15)')
					,CreadoDate				= @FECHA --CAB.X.value('@CreadoDate','datetime')
					,Anulado				= 0--CAB.X.value('@Anulado','bit')
					,AnuladoPor				= CAB.X.value('@AnuladoPor','varchar(15)')
					,AnuladoDate			= CAB.X.value('@AnuladoDate','datetime')
					,AnuladoNota			= CAB.X.value('@AnuladoNota','varchar(1024)')
					,EditadoPor				= CAB.X.value('@EditadoPor','varchar(15)')
					,EditadoDate			= CAB.X.value('@EditadoDate','datetime')
					,ExportadoDate			= CAB.X.value('@ExportadoDate','datetime')
					,SucursalID				= @SucursalID --CAB.X.value('@SucursalID','char(2)')
					,PcID					= CAB.X.value('@PcID','varchar(50)')
					,AsientoID				= CAB.X.value('@AsientoID','varchar(10)')
					,ExportadoPor			= CAB.X.value('@ExportadoPor','varchar(15)')
					,Exportado				= CAB.X.value('@Exportado','numeric(1,0)')
					,ExportadoUpdate		= CAB.X.value('@ExportadoUpdate','numeric(1,0)')
					,ExportadoCandidate		= CAB.X.value('@ExportadoCandidate','numeric(1,0)')
					,CréditoPor				= CAB.X.value('@CréditoPor','varchar(20)')
					,CréditoNota			= CAB.X.value('@CréditoNota','varchar(200)')
					,CréditoDate			= CAB.X.value('@CréditoDate','datetime')
 
			FROM	@infoXml.nodes('/VEN_ORDENES_DTO') AS CAB(X)


			INSERT INTO [ANGELITO].[dbo].VEN_ORDENES_DT
					(
							 ID
							,OrdenID
							,ProductoID
							,Stock
							,Cantidad
							,Facturado
							,Egresado
							,Precio
							,Costo
							,Subtotal
							,TasaDescuento
							,Descuento
							,TasaImpuesto
							,Impuesto
							,Total
							,Clase
							,Detalle_Ex
							,Empaque
							,Embarque
							,PrecioName
							,Factor
							,SaldoAcum
							,CreadoPor
							,CreadoDate
							,EditadoPor
							,EditadoDate
							,SucursalID
							,PcID
							,ExportadoDate
							,ExportadoPor
							,Exportado
							,ExportadoUpdate
							,ExportadoCandidate
							,BodegaID
							,Promocion
							,TasaImpuestoVerde
							,ImpuestoVerde
							,TasaImpuestoIce
							,ImpuestoIce
					)
					SELECT
							 --ID						= DET.X.value('@ID','char(10)')
							  
							ID= 	 RIGHT(@ID,7) + Replicate('0', (3 - len(ltrim(str( ROW_NUMBER() OVER(ORDER BY DET.X.value('@ID','CHAR(10)') ASC)))))) + ltrim(str( ROW_NUMBER() OVER(ORDER BY DET.X.value('@ID','CHAR(10)') ASC)))
							,OrdenID				= DET.X.value('@OrdenID','char(10)')
							,ProductoID				= DET.X.value('@ProductoID','char(10)')
							,Stock					= DET.X.value('@Stock','numeric(11,2)')
							,Cantidad				= DET.X.value('@Cantidad','numeric(11,2)')
							,Facturado				= 0-- DET.X.value('@Facturado','numeric(11,2)')
							,Egresado				= 0--DET.X.value('@Egresado','numeric(11,2)')
							,Precio					= DET.X.value('@Precio','money')
							,Costo					= DET.X.value('@Costo','money')
							,Subtotal				= DET.X.value('@Subtotal','money')
							,TasaDescuento			= DET.X.value('@TasaDescuento','decimal(5,2)')
							,Descuento				= DET.X.value('@Descuento','money')
							,TasaImpuesto			= DET.X.value('@TasaImpuesto','decimal(5,2)')
							,Impuesto				= DET.X.value('@Impuesto','money')
							,Total					= DET.X.value('@Total','money')
							,Clase					= DET.X.value('@Clase','char(2)')
							,Detalle_Ex				= ''--DET.X.value('@Detalle_Ex','varchar(1024)')
							,Empaque				= DET.X.value('@Empaque','varchar(40)')
							,Embarque				= DET.X.value('@Embarque','bit')
							,PrecioName				= DET.X.value('@Embarque','bit')--DET.X.value('@PrecioName','varchar(40)')
							,Factor					= DET.X.value('@Factor','numeric(6,2)')
							,SaldoAcum				= DET.X.value('@SaldoAcum','money')
							,CreadoPor				= 'Admin'--DET.X.value('@CreadoPor','varchar(15)')
							,CreadoDate				= getdate()--DET.X.value('@CreadoDate','datetime')
							,EditadoPor				= DET.X.value('@EditadoPor','varchar(15)')
							,EditadoDate			= DET.X.value('@EditadoDate','datetime')
							,SucursalID				= @SucursalID --DET.X.value('@SucursalID','char(2)')
							,PcID					= ''--DET.X.value('@PcID','varchar(50)')
							,ExportadoDate			= DET.X.value('@ExportadoDate','datetime')
							,ExportadoPor			= DET.X.value('@ExportadoPor','varchar(15)')
							,Exportado				= 0--DET.X.value('@Exportado','numeric(1,0)')
							,ExportadoUpdate		= 0--DET.X.value('@ExportadoUpdate','numeric(1,0)')
							,ExportadoCandidate		= 0--DET.X.value('@ExportadoCandidate','numeric(1,0)')
							,BodegaID				= '0000000001'--DET.X.value('@BodegaID','char(10)')
							,Promocion				= DET.X.value('@Promocion','bit')
							,TasaImpuestoVerde		= DET.X.value('@TasaImpuestoVerde','decimal(5,2)')
							,ImpuestoVerde			= DET.X.value('@ImpuestoVerde','money')
							,TasaImpuestoIce		= DET.X.value('@TasaImpuestoIce','decimal(5,2)')
							,ImpuestoIce			= DET.X.value('@ImpuestoIce','money')
	 
					FROM	@infoXml.nodes('/VEN_ORDENES_DTO/LIST_VEN_ORDENES_DT/VEN_ORDENES_DT_DTO') AS DET(X)
				END
  
			ELSE


				BEGIN
					UPDATE	[ANGELITO].[dbo].VEN_ORDENES  SET
					 ClienteID				= CAB.X.value('@ClienteID','char(10)')
					,Detalle				= CAB.X.value('@ClienteNombre','varchar(200)') + ' - ' + CAB.X.value('@ID','char(10)')--CAB.X.value('@Detalle','varchar(50)')
					,VendedorID				= @VENDEDORID --CAB.X.value('@VendedorID','char(10)')
					,TérminoID				= CAB.X.value('@TerminoID','char(10)')
					,DivisiónID				= @DivisiónID --CAB.X.value('@DivisiónID','char(10)') 
					,Número					= CAB.X.value('@Número','char(10)')
					,Fecha					= @FECHA --CAB.X.value('@Fecha','datetime')
					,Entregado				= CAB.X.value('@Entregado','datetime')
					,Tipo					= @TIPODOC --CAB.X.value('@Tipo','varchar(10)')
					,DivisaID				= @DivisaID--CAB.X.value('@DivisaID','char(10)')
					,Cambio					= @Cambio --CAB.X.value('@Cambio','decimal(12,6)')
					,Subtotal				= CAB.X.value('@Subtotal','money')
					,Descuento				= CAB.X.value('@Descuento','money')
					,Impuesto				= CAB.X.value('@Impuesto','money')
					,Total					= CAB.X.value('@Total','money')
					,Nota					= CAB.X.value('@Nota','varchar(1024)')
					,Nota2					= CAB.X.value('@Nota2','varchar(1024)')
					,Estado					= CAB.X.value('@Estado','varchar(10)')
					,Negado					= 0--CAB.X.value('@Negado','bit')
					,Aprobado				= 1 --CAB.X.value('@Aprobado','bit')
					,AprobadoPor			= CAB.X.value('@AprobadoPor','varchar(15)')
					,AprobadoDate			= CAB.X.value('@AprobadoDate','datetime')
					,AprobadoNota			= CAB.X.value('@AprobadoNota','varchar(1024)')
					,Egresado				= 0--CAB.X.value('@Egresado','bit')
					,Facturado				= 0--CAB.X.value('@Facturado','bit')
					,FacturadoPor			= CAB.X.value('@FacturadoPor','varchar(15)')
					,FacturadoDate			= CAB.X.value('@FacturadoDate','datetime')
					,CreadoPor				= CAB.X.value('@CreadoPor','varchar(15)')
					,CreadoDate				= @FECHA --CAB.X.value('@CreadoDate','datetime')
					,Anulado				= 0--CAB.X.value('@Anulado','bit')
					,AnuladoPor				= CAB.X.value('@AnuladoPor','varchar(15)')
					,AnuladoDate			= CAB.X.value('@AnuladoDate','datetime')
					,AnuladoNota			= CAB.X.value('@AnuladoNota','varchar(1024)')
					,EditadoPor				= CAB.X.value('@EditadoPor','varchar(15)')
					,EditadoDate			= CAB.X.value('@EditadoDate','datetime')
					,ExportadoDate			= CAB.X.value('@ExportadoDate','datetime')
					,SucursalID				= @SucursalID --CAB.X.value('@SucursalID','char(2)')
					,PcID					= CAB.X.value('@PcID','varchar(50)')
					,AsientoID				= CAB.X.value('@AsientoID','varchar(10)')
					,ExportadoPor			= CAB.X.value('@ExportadoPor','varchar(15)')
					,Exportado				= CAB.X.value('@Exportado','numeric(1,0)')
					,ExportadoUpdate		= CAB.X.value('@ExportadoUpdate','numeric(1,0)')
					,ExportadoCandidate		= CAB.X.value('@ExportadoCandidate','numeric(1,0)')
					,CréditoPor				= CAB.X.value('@CréditoPor','varchar(20)')
					,CréditoNota			= CAB.X.value('@CréditoNota','varchar(200)')
					,CréditoDate			= CAB.X.value('@CréditoDate','datetime')
					FROM	@infoXml.nodes('/VEN_ORDENES_DTO')  AS CAB(X)
					WHERE ID	= CAB.X.value('@ID','char(10)')

					 
					DELETE FROM	[ANGELITO].[dbo].VEN_ORDENES_DT  WHERE OrdenID = @ID
					
					INSERT INTO [ANGELITO].[dbo].VEN_ORDENES_DT
					(
							 ID
							,OrdenID
							,ProductoID
							,Stock
							,Cantidad
							,Facturado
							,Egresado
							,Precio
							,Costo
							,Subtotal
							,TasaDescuento
							,Descuento
							,TasaImpuesto
							,Impuesto
							,Total
							,Clase
							,Detalle_Ex
							,Empaque
							,Embarque
							,PrecioName
							,Factor
							,SaldoAcum
							,CreadoPor
							,CreadoDate
							,EditadoPor
							,EditadoDate
							,SucursalID
							,PcID
							,ExportadoDate
							,ExportadoPor
							,Exportado
							,ExportadoUpdate
							,ExportadoCandidate
							,BodegaID
							,Promocion
							,TasaImpuestoVerde
							,ImpuestoVerde
							,TasaImpuestoIce
							,ImpuestoIce
					)
					SELECT
							 --ID						= DET.X.value('@ID','char(10)')
							 ID= 	 RIGHT(@ID,7) + Replicate('0', (3 - len(ltrim(str( ROW_NUMBER() OVER(ORDER BY DET.X.value('@ID','CHAR(10)') ASC)))))) + ltrim(str( ROW_NUMBER() OVER(ORDER BY DET.X.value('@ID','CHAR(10)') ASC)))
							,OrdenID				= DET.X.value('@OrdenID','char(10)')
							,ProductoID				= DET.X.value('@ProductoID','char(10)')
							,Stock					= DET.X.value('@Stock','numeric(11,2)')
							,Cantidad				= DET.X.value('@Cantidad','numeric(11,2)')
							,Facturado				= 0-- DET.X.value('@Facturado','numeric(11,2)')
							,Egresado				= 0--DET.X.value('@Egresado','numeric(11,2)')
							,Precio					= DET.X.value('@Precio','money')
							,Costo					= DET.X.value('@Costo','money')
							,Subtotal				= DET.X.value('@Subtotal','money')
							,TasaDescuento			= DET.X.value('@TasaDescuento','decimal(5,2)')
							,Descuento				= DET.X.value('@Descuento','money')
							,TasaImpuesto			= DET.X.value('@TasaImpuesto','decimal(5,2)')
							,Impuesto				= DET.X.value('@Impuesto','money')
							,Total					= DET.X.value('@Total','money')
							,Clase					= DET.X.value('@Clase','char(2)')
							,Detalle_Ex				= ''--DET.X.value('@Detalle_Ex','varchar(1024)')
							,Empaque				= DET.X.value('@Empaque','varchar(40)')
							,Embarque				= DET.X.value('@Embarque','bit')
							,PrecioName				= DET.X.value('@Empaque','varchar(40)')--DET.X.value('@PrecioName','varchar(40)')
							,Factor					= DET.X.value('@Factor','numeric(6,2)')
							,SaldoAcum				= DET.X.value('@SaldoAcum','money')
							,CreadoPor				= 'Admin'--DET.X.value('@CreadoPor','varchar(15)')
							,CreadoDate				= getdate()--DET.X.value('@CreadoDate','datetime')
							,EditadoPor				= DET.X.value('@EditadoPor','varchar(15)')
							,EditadoDate			= DET.X.value('@EditadoDate','datetime')
							,SucursalID				= @SucursalID --DET.X.value('@SucursalID','char(2)')
							,PcID					= ''--DET.X.value('@PcID','varchar(50)')
							,ExportadoDate			= DET.X.value('@ExportadoDate','datetime')
							,ExportadoPor			= DET.X.value('@ExportadoPor','varchar(15)')
							,Exportado				= 0--DET.X.value('@Exportado','numeric(1,0)')
							,ExportadoUpdate		= 0--DET.X.value('@ExportadoUpdate','numeric(1,0)')
							,ExportadoCandidate		= 0--DET.X.value('@ExportadoCandidate','numeric(1,0)')
							,BodegaID				= '0000000001'--DET.X.value('@BodegaID','char(10)')
							,Promocion				= DET.X.value('@Promocion','bit')
							,TasaImpuestoVerde		= DET.X.value('@TasaImpuestoVerde','decimal(5,2)')
							,ImpuestoVerde			= DET.X.value('@ImpuestoVerde','money')
							,TasaImpuestoIce		= DET.X.value('@TasaImpuestoIce','decimal(5,2)')
							,ImpuestoIce			= DET.X.value('@ImpuestoIce','money')
	 
					FROM	@infoXml.nodes('/VEN_ORDENES_DTO/LIST_VEN_ORDENES_DT/VEN_ORDENES_DT_DTO') AS DET(X)
					 

				END

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
                    ,'Grabación exitosa'	AS ErrorMessage; 
             END

