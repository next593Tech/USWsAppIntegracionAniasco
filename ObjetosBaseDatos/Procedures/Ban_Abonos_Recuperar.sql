USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[Ban_Abonos_Recuperar]    Script Date: 21/08/2017 11:40:35 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- ===============================================================
-- Author:		Douglas Bustos
-- Create date: 25 enero 2017
-- Description:	Recupera la cabecera y detalle de la orden de pedido
-- ===============================================================

-- exec  [Ban_Abonos_Recuperar]  '0000002648'
-- exec  [Ban_Abonos_Recuperar]  '0000002333'

ALTER PROCEDURE [dbo].[Ban_Abonos_Recuperar]
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

				BIC.Id,
				NumDcto		= BIC.DeudorID,
				Concepto	= BIC.Detalle,
				Tipo		= BIC.Tipo,
			
				Fecha		= BIC.Fecha,
				Asiento		= BIC.AsientoID,
				Estado		= CASE WHEN BIC.Anulado = 1 THEN 'ANULADO'
								ELSE 'ACTIVO'
								END,
				Codigo		=  CLI.Ruc,
				ClienteNombre= CLI.Nombre ,
				ClienteEmail = CLI.Email,
				NumRecibo	= BIC.NumRecibo,
				Valor		= CONVERT(decimal(12,6),BIC.Valor),
				ValorBase		= CONVERT(decimal(12,6),BIC.Valor)
  
				FROM [ANGELITO].[dbo].[BAN_INGRESOS] BIC
			 
				INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES] CLI ON BIC.DeudorID =CLI.ID
				WHERE BIC.ID=@AbonoId

				 
				SELECT 

				BID.Id,
				TipoId      = LTRIM(RTRIM(BID.Tipo)),
				TipoNombre	= LTRIM(RTRIM(BK.ID)),
				BancoId		=  BID.Banco,
				BancoNombre	=  BID.Banco,
				BancoId		=  BID.Banco,
				Cuenta		= BID.Cuenta,
				NoCheque	= BID.Número,
				Girador		= BID.Girador,
				DivisaId	= LTRIM(RTRIM(BID.DivisaID)),
				DivisaNombre	= 'DOLLAR',
				DivisaValor	= CONVERT(decimal(12,6),BID.Cambio),
				Valor		= CONVERT(decimal(12,6),BID.Valor),
				ValorBase		= CONVERT(decimal(12,6),BID.Valor)
  
				FROM  [ANGELITO].[dbo].[BAN_INGRESOS_DT] BID 
				LEFT JOIN [ANGELITO].[dbo].[SIS_PARAMETROS] AS BK ON LTRIM(RTRIM(BK.Nombre))  = LTRIM(RTRIM(BID.Banco)) AND  LTRIM(RTRIM(BK.PadreID)) =   
					(
					SELECT DISTINCT 
						  LTRIM(RTRIM(PAR.ID ))
					FROM [ANGELITO].[dbo].[SIS_PARAMETROS] AS PAR  
					WHERE Código = LTRIM(RTRIM('BANCOS'))
					 )
 
				WHERE BID.IngresoID=@AbonoId


				SELECT 
				BID.DeudaID as Id,
				 
 				Detalle	= BID.Detalle,
				Vencimiento	= BID.Vencimiento,
				Valor = ISNULL((SELECT TOP(1) Valor FROM [ANGELITO].[dbo].CLI_Clientes_Deudas dd where dd.ID= BID.DeudaID ORDER BY ID DESC ),0),
				SaldoInicial= CONVERT(decimal(12,2),ISNULL(BID.Saldo,0)),
				Abono		= CONVERT(decimal(12,2),BID.Valor),
				Saldo		= CONVERT(decimal(12,2),BID.Saldo) - CONVERT(decimal(12,2),BID.Valor)
	 
				FROM [ANGELITO].[dbo].[BAN_INGRESOS_DEUDAS] BID  
			--	INNER JOIN  [ANGELITO].[dbo].CLI_Clientes_Deudas DO ON DO.DeudaID = BID.DeudaID
			 
				 WHERE BID.IngresoID=@AbonoId
				order by  BID.IngresoID desc
				 
END

 

  