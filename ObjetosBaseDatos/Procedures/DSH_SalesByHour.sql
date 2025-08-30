USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_SalesByHour]    Script Date: 21/08/2017 11:44:51 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Next Technology>
-- Create date: <13-07-2017>
-- Description:	<Ventas por hora>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_SalesByHour]
	@Fecha datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--SET @Fecha =  convert(datetime,'2017-01-24',101)

    -- Insert statements for procedure here
	SELECT 
	CONVERT(VARCHAR,(datepart(hh,CreadoDate))) + '-' +CONVERT(VARCHAR,(datepart(hh,CreadoDate)+1)) [Hour], 
	SUM(Total) as Total, COUNT(ID) as Invoices
	FROM [ANGELITO].[dbo].VEN_FACTURAS
	  WHERE fecha = @Fecha and Anulado = 0
	GROUP BY datepart(hh,CreadoDate)
	ORDER BY datepart(hh,CreadoDate)

END

