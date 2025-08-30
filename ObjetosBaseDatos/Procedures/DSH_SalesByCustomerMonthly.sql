USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_SalesByCustomerMonthly]    Script Date: 21/08/2017 11:44:06 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Next Technology>
-- Create date: <13-07-2017>
-- Description:	<Ventas por Cliente>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_SalesByCustomerMonthly]
	@Fecha datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;


--	SET @Fecha =  convert(datetime,'2017-01-24',101)


  SELECT * FROM 
	(
	SELECT CL.Nombre Name, datename(MONTH,FA.fecha) Mes, FA.total
	FROM [ANGELITO].[dbo].VEN_FACTURAS FA INNER JOIN [ANGELITO].[dbo].CLI_CLIENTES CL ON FA.ClienteID = CL.ID
	WHERE FA.Anulado = 0 AND datepart(yyyy,FA.fecha) = datepart(YYYY,@Fecha)
	) as PV 
	PIVOT (SUM(Total) for Mes in ([January],[February],[March],[April],[May],[June],[July],[August],[September],[October],[November],[December]) ) as PVT

END

-- PDSH_SalesByCustomerMonthly '20170412'

