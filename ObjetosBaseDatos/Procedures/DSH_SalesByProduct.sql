USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_SalesByProduct]    Script Date: 21/08/2017 11:45:53 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Next Technology>
-- Create date: <13-07-2017>
-- Description:	<Ventas por Cliente>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_SalesByProduct]
	@Fecha datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH VentasPorProducto(Nombre, Total) AS 
	(
		SELECT  PD.Nombre, SUM(DT.SubTotal -  (DT.Cantidad * DT.Costo)) as Total 
		FROM [ANGELITO].[dbo].VEN_FACTURAS FA INNER JOIN [ANGELITO].[dbo].VEN_FACTURAS_DT DT ON FA.ID = DT.facturaID
		INNER JOIN [ANGELITO].[dbo].INV_PRODUCTOS PD ON DT.ProductoID = PD.ID
		WHERE datepart(YYYY,FA.Fecha) = datepart(YYYY,@Fecha) AND FA.Anulado = 0
		GROUP BY PD.Nombre
	)
	SELECT Nombre Name, Total,
		rank() over (order by Total desc) as Ranking
	FROM VentasPorProducto
END

-- PDSH_SalesByProduct '20170412'

