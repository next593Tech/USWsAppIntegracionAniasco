USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[Ban_Abonos_Imprimir]    Script Date: 21/08/2017 11:40:08 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- ===============================================================
-- Author:		Douglas Bustos
-- Create date: 25 enero 2017
-- Description:	Recupera la cabecera y detalle de la orden de pedido
-- ===============================================================

-- exec [Ban_Abonos_Imprimir] '0000002648'

ALTER PROCEDURE [dbo].[Ban_Abonos_Imprimir]
(
	 @AbonoId varchar(10)
)
AS

SET NOCOUNT ON
 	 		     

DECLARE @NombreEmpresa VARCHAR(50)
DECLARE @RucEmpresa VARCHAR(50)
      

SET @NombreEmpresa = 'COMERCIAL DEMO S.A.'
SET @RucEmpresa = '0994518586015'

 
BEGIN
  

				SELECT 
				@NombreEmpresa as NombreEmpresa,
				@RucEmpresa	   as RucEmpresa,
				BIC.Id AS Id,
				NumDcto		= BIC.DeudorID,
				Concepto	= BIC.Detalle,
				Tipo		= BIC.Tipo,
				Fecha		= BIC.Fecha,
				Asiento		= BIC.AsientoID,
				Estado		= CASE WHEN BIC.Anulado = 1 THEN 'ANULADO'
								ELSE 'ACTIVO'
								END,
				Codigo		=  CLI.Código,
				ClienteNombre= CLI.Nombre ,
				ClienteEmail = CLI.Email,
				NumRecibo	= BIC.NumRecibo,
				Banco		= BID.Banco,
				Cuenta		= BID.Cuenta,
				NoCheque	= BID.Número,
				Girador		= BID.Girador,
				DivisaID	= CONVERT(decimal(12,6),BID.Cambio),
				Valor		= CONVERT(decimal(12,6),BID.Valor)
  
				FROM [ANGELITO].[dbo].[BAN_INGRESOS] BIC
				INNER JOIN [ANGELITO].[dbo].[BAN_INGRESOS_DT] BID ON BIC.ID = BID.IngresoID
				INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES] CLI ON BIC.DeudorID =CLI.ID
				WHERE BIC.ID=@AbonoId


				SELECT 
				CONVERT(INT,ROW_NUMBER() OVER(ORDER BY DeudorID ASC)) AS Id,
				NumDcto		= BIC.DeudorID,
				Concepto	= BIC.Detalle,
				Tipo		= BIC.Tipo,
				Fecha		= BIC.Fecha,
				Asiento		= BIC.AsientoID,
				Estado		= CASE WHEN BIC.Anulado = 1 THEN 'ANULADO'
								ELSE 'ACTIVO'
								END,
				Detalle		= BID.Detalle,
				Vencimiento	= BID.Vencimiento,
				SaldoInicial= CONVERT(decimal(12,2),BID.Saldo),
				Abono		= CONVERT(decimal(12,2),BID.Valor),
				Saldo		= CONVERT(decimal(12,2),BID.Saldo) - CONVERT(decimal(12,2),BID.Valor)
	 
				FROM [ANGELITO].[dbo].[BAN_INGRESOS] BIC
				INNER JOIN [ANGELITO].[dbo].[BAN_INGRESOS_DEUDAS] BID ON BIC.ID = BID.IngresoID
				INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES] CLI ON BIC.DeudorID =CLI.ID
				 WHERE BIC.ID=@AbonoId
				order by BIC.Fecha desc
				 
END

 


