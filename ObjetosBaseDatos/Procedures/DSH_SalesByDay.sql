USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_SalesByDay]    Script Date: 21/08/2017 11:44:28 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Next Technology>
-- Create date: <13-07-2017>
-- Description:	<Ventas por dia>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_SalesByDay]
	@Fecha datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

--SET @Fecha =  convert(datetime,'2017-01-24',101)

    -- Insert statements for procedure here
	SELECT   dateName(DW,Fecha) DayName, SUM(Total) as Total, COUNT(ID) as Invoices
	FROM [ANGELITO].[dbo].VEN_FACTURAS
	WHERE (datepart(mm,Fecha) = datepart(mm,@Fecha)) AND 
		   datepart(YYYY,Fecha) = datepart(YYYY,@Fecha) AND Anulado = 0
	GROUP BY dateName(DW,Fecha) 
	ORDER BY 1


END

