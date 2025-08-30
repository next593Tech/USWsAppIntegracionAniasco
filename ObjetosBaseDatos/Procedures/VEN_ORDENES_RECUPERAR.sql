USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[VEN_Ordenes_Recuperar]    Script Date: 21/08/2017 11:51:35 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author:		Douglas Bustos
-- Create date: 25 enero 2017
-- Description:	Recupera la cabecera y detalle de la orden de pedido
-- ===============================================================

--exec [VEN_Ordenes_Recuperar] '0000000015'
-- select * from ANGELITO.dbo.

ALTER PROCEDURE [dbo].[VEN_Ordenes_Recuperar]
(
	 @OrdenId varchar(10)
)
AS

SET NOCOUNT ON
 	
DECLARE @DivisiónID	CHAR(10)		
DECLARE @DivisaID   CHAR(10)	
DECLARE @Cambio		DECIMAL(12,6) 
DECLARE @ID			INT
DECLARE @FECHA		DATETIME
DECLARE @USUARIOID	VARCHAR(15)
 
 
SET @DivisiónID = '0000000001'
SET @DivisaID   = '0000000001'
SET @Cambio		= 1.000000
SET @FECHA		= GETDATE()
--`SET @USUARIOID	= 'DBUSTOS'

-- exec [VEN_ORDENES_RECUPERAR] '0000000091'

BEGIN

			SELECT 
							 ORD.ID
							,ORD.ClienteID
							,CLI.Clase as ClienteClase
							,CLI.Ruc as ClienteRUC
							,ORD.Detalle
							,ORD.VendedorID
							,VendedorUserName = SEG.Código 
							,TerminoID = ORD.TérminoID
							,ORD.DivisiónID
							,ORD.Número
							,ORD.Fecha
							,ORD.Entregado
							,ORD.Tipo
							,ORD.DivisaID
							,ORD.Cambio
							,ORD.Subtotal
							,ORD.Descuento
							,ORD.Impuesto
							,ORD.Total
							,ORD.Nota
							,ORD.Nota2
							,ORD.Estado
							,ORD.Negado
							,ORD.Aprobado
							,ORD.AprobadoPor
							,ORD.AprobadoDate
							,ORD.AprobadoNota
							,ORD.Egresado
							,ORD.Facturado
							,ORD.FacturadoPor
							,ORD.FacturadoDate
							,ORD.CreadoPor
							,ORD.CreadoDate
							,ORD.Anulado
							,ORD.AnuladoPor
							,ORD.AnuladoDate
							,ORD.AnuladoNota
							,ORD.EditadoPor
							,ORD.EditadoDate
							,ORD.ExportadoDate
							,ORD.SucursalID
							,ORD.PcID
							,ORD.AsientoID
							,ORD.ExportadoPor
							,ORD.Exportado
							,ORD.ExportadoUpdate
							,ORD.ExportadoCandidate
							,ORD.CréditoPor
							,ORD.CréditoNota
							,ORD.CréditoDate
				FROM [ANGELITO].[dbo].VEN_ORDENES			ORD
				INNER JOIN [ANGELITO].[dbo].SEG_USUARIOS SEG
				ON ORD.VendedorID = SEG.EmpleadoId
				INNER JOIN [ANGELITO].[dbo].CLI_CLIENTES CLI ON CLI.ID = ORD.ClienteID
				WHERE ORD.ID = @OrdenId


				SELECT 
							 DT.ID
							,DT.OrdenID
							,DT.ProductoID
							,Nombre		 = PR.Nombre
							,Descripcion = PR.Descripción
							,DT.Stock
							,DT.Cantidad
							,DT.Facturado
							,DT.Egresado
							,DT.Precio
							,DT.Costo
							,DT.Subtotal
							,DT.TasaDescuento
							,DT.Descuento
							,DT.TasaImpuesto
							,DT.Impuesto
							,DT.Total
							,DT.Clase
							,DT.Detalle_Ex
							,DT.Empaque
							,DT.Embarque
							,DT.PrecioName
							,DT.Factor
							,DT.SaldoAcum
							,DT.CreadoPor
							,DT.CreadoDate
							,DT.EditadoPor
							,DT.EditadoDate
							,DT.SucursalID
							,DT.PcID
							,DT.ExportadoDate
							,DT.ExportadoPor
							,DT.Exportado
							,DT.ExportadoUpdate
							,DT.ExportadoCandidate
							,DT.BodegaID
							,DT.Promocion
							,TasaImpuestoVerde = ISNULL(DT.TasaImpuestoVerde,0)
							,ImpuestoVerde = ISNULL(DT.ImpuestoVerde,0)
							,TasaImpuestoIce = ISNULL(DT.TasaImpuestoIce,0)
							,ImpuestoIce = ISNULL(DT.ImpuestoIce,0)
							,AIVA = CASE 
									WHEN  ISNULL(DT.TasaImpuesto,0) =0 OR DT.TasaImpuesto=0  THEN Convert(bit,0)	
									ELSE Convert(bit,1)
									END
 
		  
							,AICE = CASE 
									WHEN   ISNULL(DT.TasaImpuestoVerde,0) =0 OR DT.TasaImpuestoVerde=0  THEN Convert(bit,0)	
									ELSE Convert(bit,1)
									END 
	
	   
							,AVER = CASE 
									WHEN   ISNULL(DT.TasaImpuestoIce,0) =0 OR DT.TasaImpuestoIce=0  THEN Convert(bit,0)
									ELSE Convert(bit,1)
									END 
		  
					FROM [ANGELITO].[dbo].VEN_ORDENES_DT		DT
					INNER JOIN [ANGELITO].[dbo].INV_PRODUCTOS PR
					ON DT.ProductoID = PR.ID
   
     
					WHERE OrdenId = @OrdenId

END

 
