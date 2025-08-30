
declare @infoxml xml

set @infoxml ='<VEN_ORDENES_DTO xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" ID="0000000089" Número="0000000089" ClienteID="0000000099" VendedorUsername="ADMIN" TérminoID="0000001594" Fecha="2017-01-27T11:57:46.3957475-05:00" Subtotal="590.8" Descuento="0" Impuesto="140" ImpuestoIce="0" ImpuestoVerde="0" Total="673.512" PcID="DJBM">
  <LIST_VEN_ORDENES_DT>
    <VEN_ORDENES_DT_DTO ID="1" OrdenID="0000000089" ProductoID="0000000337" Stock="0.0" Cantidad="1000" Precio="0.5908" Costo="0.5397" Subtotal="590.8" TasaDescuento="0.0" Descuento="0.0" TasaImpuesto="14" Impuesto="82.712" TasaImpuestoVerde="0" ImpuestoVerde="0.0" TasaImpuestoIce="0" ImpuestoIce="0.0" Total="673.512" Clase="01" Empaque="U" Embarque="false" PrecioName="Cja." Factor="0" SaldoAcum="0.0" Promocion="false" />
  </LIST_VEN_ORDENES_DT>
</VEN_ORDENES_DTO>'

DECLARE @VENDEDORUSERNAME	VARCHAR(15)

SET @VENDEDORUSERNAME = (SELECT VENDEDORUSERNAME = VEN.X.value('@VendedorUsername','char(15)') FROM	@infoXml.nodes('/VEN_ORDENES_DTO') AS VEN(X))
SELECT @VENDEDORUSERNAME


EXEC [dbo].[SIS_GetNextID]

EXEC dbo.VEN_ORDENES_Grabar @infoxml

--select  @infoxml
-- delete from  [MINIMARKET].[dbo].VEN_ORDENES where ID='49'  and Número='2' and ClienteID='0000000099'
-- Select * from  [MINIMARKET].[dbo].VEN_ORDENES where ID='49'  and Número='2' and ClienteID='0000000099'
-- Select * from  [MINIMARKET].[dbo].VEN_ORDENES_DT where ORDENID='49'   
-- DELETE from  [MINIMARKET].[dbo].VEN_ORDENES_DT where ORDENID='3'   

 
 INSERT INTO [MINIMARKET].[dbo].VEN_ORDENES_DT
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
					)
 SELECT
				ID						= DET.X.value('@ID','char(10)')
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
				,Detalle_Ex				= DET.X.value('@Detalle_Ex','varchar(1024)')
				,Empaque				= DET.X.value('@Empaque','varchar(40)')
				,Embarque				= DET.X.value('@Embarque','bit')
				,PrecioName				= DET.X.value('@PrecioName','varchar(40)')
				,Factor					= DET.X.value('@Factor','numeric(6,2)')
				,SaldoAcum				= DET.X.value('@SaldoAcum','money')
				,CreadoPor				= DET.X.value('@CreadoPor','varchar(15)')
				,CreadoDate				= DET.X.value('@CreadoDate','datetime')
				,EditadoPor				= DET.X.value('@EditadoPor','varchar(15)')
				,EditadoDate			= DET.X.value('@EditadoDate','datetime')
				,SucursalID				= DET.X.value('@SucursalID','char(2)')
				,PcID					= DET.X.value('@PcID','varchar(50)')
				,ExportadoDate			= DET.X.value('@ExportadoDate','datetime')
				,ExportadoPor			= DET.X.value('@ExportadoPor','varchar(15)')
				,Exportado				= DET.X.value('@Exportado','numeric(1,0)')
				,ExportadoUpdate		= DET.X.value('@ExportadoUpdate','numeric(1,0)')
				,ExportadoCandidate		= DET.X.value('@ExportadoCandidate','numeric(1,0)')
				,BodegaID				= DET.X.value('@BodegaID','char(10)')
				,Promocion				= DET.X.value('@Promocion','bit')

			FROM	@infoXml.nodes('/VEN_ORDENES_DTO/LIST_VEN_ORDENES_DT/VEN_ORDENES_DT_DTO') AS DET(X)