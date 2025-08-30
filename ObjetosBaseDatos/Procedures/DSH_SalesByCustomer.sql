USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_SalesByCustomer]    Script Date: 21/08/2017 11:43:46 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Next Technology>
-- Create date: <13-07-2017>
-- Description:	<Ventas por Cliente>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_SalesByCustomer]
	@Fecha datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH VentasPorCliente(Nombre, Total) AS 
	(
		SELECT CL.Nombre, SUM(FA.Total) as Total 
		FROM [ANGELITO].[dbo].ven_facturas FA INNER JOIN [ANGELITO].[dbo].CLI_CLIENTES CL ON FA.ClienteID = CL.ID
		WHERE datepart(YYYY,FA.Fecha) = datepart(YYYY,@Fecha) AND FA.Anulado = 0
		GROUP BY CL.Nombre
	)
	SELECT Nombre Name, Total,
		rank() over (order by Total desc) as Ranking
	FROM VentasPorCliente
END

-- exec DSH_SalesByCustomer '20170412'

