USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_SalesByBrand]    Script Date: 21/08/2017 11:43:20 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 
-- =============================================
-- Author:                         <Next Technology>
-- Create date: <13-07-2017>
-- Description:   <Ventas por Cliente>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_SalesByBrand]
              @Fecha datetime
AS
BEGIN
              -- SET NOCOUNT ON added to prevent extra result sets from
              SET NOCOUNT ON;
 
    -- Insert statements for procedure here
              WITH VentasPorProducto(Nombre, Total) AS
              (
                            SELECT  PD.Marca, SUM(DT.SubTotal -  (DT.Cantidad * DT.Costo)) as Total
                            FROM [ANGELITO].[dbo].ven_facturas FA INNER JOIN [ANGELITO].[dbo].VEN_FACTURAS_DT DT ON FA.ID = DT.facturaID
                            INNER JOIN [ANGELITO].[dbo].INV_PRODUCTOS PD ON DT.ProductoID = PD.ID
                            WHERE datepart(YYYY,FA.Fecha) = datepart(YYYY,@Fecha) AND FA.Anulado = 0
                            GROUP BY PD.Marca
              )
              SELECT CASE WHEN Nombre = '' THEN 'None' else Nombre END Name, Total,
                           CONVERT(INT,rank() over (order by Total desc)) as Ranking
              FROM  VentasPorProducto
END
 
-- DSH_SalesByBrand '20170412'
