USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_SalesByLine]    Script Date: 21/08/2017 11:45:11 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 
-- =============================================
-- Author:                         <Next Technology>
-- Create date: <13-07-2017>
-- Description:   <Ventas por Cliente>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_SalesByLine]
              @Fecha datetime
AS
BEGIN
              -- SET NOCOUNT ON added to prevent extra result sets from
              SET NOCOUNT ON;

-- SET @Fecha =  convert(datetime,'2017-01-24',101);

    -- Insert statements for procedure here
              WITH VentasPorProducto(GrupoID, Total) AS
              (
                            SELECT  SUBSTRING( GR.Ruta,6,10) as GrupoID, SUM(CONVERT(DECIMAL(12,4),DT.SubTotal -  (DT.Cantidad * DT.Costo))) as Total
                            FROM [ANGELITO].[dbo].ven_facturas FA INNER JOIN [ANGELITO].[dbo].VEN_FACTURAS_DT DT ON FA.ID = DT.facturaID
                            INNER JOIN [ANGELITO].[dbo].INV_PRODUCTOS PD ON DT.ProductoID = PD.ID
                            LEFT OUTER JOIN [ANGELITO].[dbo].INV_GRUPOS GR ON PD.GrupoID = GR.ID
                            WHERE datepart(YYYY,FA.Fecha) = datepart(YYYY,@Fecha) AND FA.Anulado = 0
                            GROUP BY SUBSTRING( GR.Ruta,6,10)
              )
              SELECT Nombre Name, Total,
                            CONVERT(INT,rank() over (order by Total desc)) as Ranking
              FROM VentasPorProducto INNER JOIN [ANGELITO].[dbo].INV_GRUPOS GR ON GrupoID = GR.ID
END
 
-- DSH_SalesByLine '20170412'
 
 
