USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[VEN_Ordenes_Clonar]    Script Date: 21/08/2017 11:49:59 a. m. ******/
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

ALTER PROCEDURE [dbo].[VEN_Ordenes_Clonar]
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
 
 
-- exec [VEN_Ordenes_Clonar] '0000000248' 

DECLARE @CodSecId	    AS varchar(50)
 
DECLARE @ERROR			AS CHAR(10)


DECLARE @NUEVOID		CHAR(10)  
SET @CodSecId		= 'VEN_ORDENES-ID-00' 
 
--SELECT @NUEVOID

DECLARE @Precio			money

DECLARE @ClienteID   CHAR(10)



  
	 BEGIN TRY  

	  SET @ClienteID = (SELECT TOP 1 ClienteID FROM [ANGELITO].[dbo].VEN_ORDENES WHERE ID = @OrdenId)

	  BEGIN TRAN
	  
			EXEC [dbo].[SIS_GetNextIDOUT] @CodSecId,1, @NUEVOID OUT, @ERROR OUT

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
							 ID =  @NUEVOID
							,ORD.ClienteID
							,Detalle   = UPPER(CLI.Nombre) + ' - ' + @NUEVOID 
							,ORD.VendedorID
							,ORD.TérminoID
							,ORD.DivisiónID
							,Número = @NUEVOID
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
				FROM [ANGELITO].[dbo].VEN_ORDENES	ORD
				INNER JOIN [ANGELITO].[dbo].SEG_USUARIOS SEG
				ON ORD.VendedorID = SEG.EmpleadoId
				INNER JOIN [ANGELITO].[dbo].CLI_CLIENTES CLI ON CLI.ID = ORD.ClienteID
				WHERE ORD.ID = @OrdenId

			 

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
							 ID =  RIGHT(@NUEVOID,7) + Replicate('0', (3 - len(ltrim(str( ROW_NUMBER() OVER(ORDER BY DT.ID ASC)))))) + ltrim(str( ROW_NUMBER() OVER(ORDER BY DT.ID ASC)))
							,OrdenID = @NUEVOID
							,DT.ProductoID
							,DT.Stock
							,DT.Cantidad
							,DT.Facturado
							,DT.Egresado
							,Precio = dbo.fnObtenerPrecioProductoxClaseCliente(@ClienteID,DT.ProductoID,DT.Cantidad)
							  
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
							,TasaImpuestoVerde	= ISNULL(DT.TasaImpuestoVerde,0)
							,ImpuestoVerde		= ISNULL(DT.ImpuestoVerde,0)
							,TasaImpuestoIce	= ISNULL(DT.TasaImpuestoIce,0)
							,ImpuestoIce		= ISNULL(DT.ImpuestoIce,0)
							 
					FROM [ANGELITO].[dbo].VEN_ORDENES_DT	DT
				  	WHERE OrdenId = @OrdenId

		 

	END TRY  
	BEGIN CATCH 
	 
			 IF @@TRANCOUNT > 0 
				 ROLLBACK TRANSACTION; 
				  
			SELECT  
			ERROR_NUMBER()		AS ErrorNumber 
			,ERROR_SEVERITY()	AS ErrorSeverity 
			,ERROR_STATE()		AS ErrorState 
			,ERROR_PROCEDURE()  AS ErrorProcedure 
			,ERROR_LINE()       AS ErrorLine 
			,ERROR_MESSAGE()	AS ErrorMessage
			--,ErrorMessage
			--= CASE WHEN
			--	ERROR_NUMBER() = 2627 THEN 'ALREADYEXISTSTHERECORD'   
			--	ELSE
			--	ERROR_MESSAGE()
			--	END
  		   

	END CATCH  
 
			IF @@TRANCOUNT > 0 
             BEGIN
                    COMMIT TRANSACTION; 

					EXEC [VEN_ORDENES_RECUPERAR] @NUEVOID

                    SELECT  
                     0		AS ErrorNumber 
                    ,0		AS ErrorSeverity 
                    ,0		AS ErrorState 
                    ,'NN'	AS ErrorProcedure 
                    ,0           AS ErrorLine 
                    ,'Grabación exitosa'	AS ErrorMessage; 
             END
  
