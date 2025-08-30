USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_SalesByMonth]    Script Date: 21/08/2017 11:45:32 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Next Technology>
-- Create date: <13-07-2017>
-- Description:	<Ventas por Mes>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_SalesByMonth]
	@Fecha datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--SET @Fecha =  convert(datetime,'2017-01-24',101)

    -- Insert statements for procedure here
	SELECT datepart(mm,fecha) MonthID, dateName(MONTH,Fecha) as  MonthName, SUM(Total) as Total, COUNT(ID) as Invoices
	FROM [ANGELITO].[dbo].VEN_FACTURAS
	WHERE datepart(yyyy,fecha) = datepart(yyyy,@Fecha) and Anulado = 0
	GROUP BY datepart(mm,fecha), dateName(MONTH,Fecha)
	ORDER BY 1
END

--  exec DSH_SalesByMonth '20170412'

