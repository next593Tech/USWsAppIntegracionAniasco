USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[DSH_ValuesOfTheMonth]    Script Date: 21/08/2017 11:46:15 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 
-- =============================================
-- Author:     <Next Technology>
-- Create date: <13-07-2017>
-- Description:   <Ventas por dia>
-- =============================================
ALTER PROCEDURE [dbo].[DSH_ValuesOfTheMonth]
              @Fecha datetime  
			
AS
BEGIN


DECLARE @TotalCustomerOfMonth	INT		 , 
		@TotalExpensesOfMonth	DECIMAL  ,
		@TotalIncomeOfMonth		DECIMAL  ,
		@TotalSalesOfMonth		DECIMAL   

DECLARE @TTotalCustomerOfMonth	DECIMAL	 = 400 , 
		@TTotalExpensesOfMonth	DECIMAL  = 500000 ,
		@TTotalIncomeOfMonth	DECIMAL  = 600000 ,
		@TTotalSalesOfMonth		DECIMAL  = 700000 


              -- SET NOCOUNT ON added to prevent extra result sets from
     SET NOCOUNT ON;

	--  SET @Fecha =  convert(datetime,'2017-01-24',101)

 
    -- Insert statements for procedure here
              SELECT @TotalCustomerOfMonth = ISNULL(COUNT(ID),0)  
              FROM [ANGELITO].[dbo].CLI_CLIENTES
              WHERE (datepart(mm,Creadodate) = datepart(mm,@Fecha)) AND
                               datepart(YYYY,Creadodate) = datepart(YYYY,@Fecha) AND Anulado = 0


			  SELECT @TotalExpensesOfMonth = ISNULL( SUM(Valor) ,0)
              FROM [ANGELITO].[dbo].BAN_EGRESOS
              WHERE (datepart(mm,Fecha) = datepart(mm,@Fecha)) AND
                               datepart(YYYY,Fecha) = datepart(YYYY,@Fecha) AND Anulado = 0


              SELECT @TotalIncomeOfMonth = ISNULL(SUM(Valor),0)
              FROM [ANGELITO].[dbo].BAN_INGRESOS
              WHERE (datepart(mm,Fecha) = datepart(mm,@Fecha)) AND
                               datepart(YYYY,Fecha) = datepart(YYYY,@Fecha) AND Anulado = 0
 
  
          SELECT @TotalSalesOfMonth = ISNULL(SUM(Subtotal - Descuento),0)
              FROM [ANGELITO].[dbo].VEN_FACTURAS
              WHERE (datepart(mm,Fecha) = datepart(mm,@Fecha)) AND
                               datepart(YYYY,Fecha) = datepart(YYYY,@Fecha) AND Anulado = 0



		SELECT 1 AS Id,  
				@TotalCustomerOfMonth	AS TotalCustomerOfMonth , 
				(@TotalCustomerOfMonth / @TTotalCustomerOfMonth *100)	AS PTotalCustomerOfMonth , 
				@TotalExpensesOfMonth	AS TotalExpensesOfMonth , 
				(@TotalExpensesOfMonth/ @TTotalExpensesOfMonth * 100)	AS PTotalExpensesOfMonth , 
				@TotalIncomeOfMonth		AS TotalIncomeOfMonth, 
				(@TotalIncomeOfMonth / @TTotalIncomeOfMonth *100)	AS PTotalIncomeOfMonth, 
				@TotalSalesOfMonth		AS TotalSalesOfMonth,
				(@TotalSalesOfMonth / @TTotalSalesOfMonth * 100)		AS PTotalSalesOfMonth	

/*

DECLARE @Fecha datetime  
 
 exec [DSH_ValuesOfTheMonth] @Fecha 

 */

END
 


