USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[CLI_Clientes_Deudas]    Script Date: 21/08/2017 11:42:07 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Douglas Bustos
-- Create date: 28 Enero 2017
-- Description:	Consulta deudas de clientes por filtros - Nexo
 
-- =============================================
ALTER PROCEDURE [dbo].[CLI_Clientes_Deudas]	--Ok 03 enero 2017
@PageNumber		INT = 1, 
@PageSize		INT = 10, --20,
@RucCliente		VARCHAR(13) = '', -- Es obligatorio
@Vencidos		bit,
@Cheques		bit,	-- 1 chequeado;  0 no chequeado
@ValorFavor		bit,
@DeudaTotal		Money OUTPUT,
@CantidadRegistros	INT OUTPUT

AS
DECLARE @DivisaID char(10);
SET @DivisaID= ( select ID from [ANGELITO].[dbo].[SIS_DIVISAS]  WITH(NOLOCK) WHERE Base = 1)

-- Obtener tipo de cambio de la divisa seleccionada
DECLARE @Cambio decimal( 12, 6 )
SELECT @Cambio = Cambio FROM [ANGELITO].[dbo].[SIS_DIVISAS]  WITH(NOLOCK) WHERE ID = @DivisaID --'0000000001'

DECLARE @Fecha datetime
SET @FECHA = GETDATE(); 

--Pregunta si el cliente tiene una deuda
   IF EXISTS (
		   SELECT TOP 1 * FROM [ANGELITO].[dbo].[CLI_CLIENTES] AS CLI
		   INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES_DEUDAS] AS DEU ON CLI.ID = DEU.ClienteID
		   WHERE (CLI.Ruc = @RucCliente)  
		   AND (DEU.Anulado = 0) AND (DEU.Saldo > 0.00) AND (DEU.Crédito = 0)
   )

   BEGIN
		-- Mostrar Deudas  cuando crédito = 0
		SELECT 
			ClienteId			=	CL.ID,
			DeudaId				=	DE.ID,
			Fecha				= CONVERT( char( 10 ), DE.Fecha, 103 ),
			Vencimiento			= CONVERT( char( 10 ), DE.Vencimiento, 103 ), 
			Dias				= CAST(DE.Vencimiento - @Fecha AS  INT),
			TipoNombre			= DE.Tipo, --Tipo de documento
			Detalle				= DE.Detalle, 
			Valor				= DE.Valor,
			Saldo				= ((CASE DE.Crédito WHEN 1 THEN - DE.Saldo ELSE DE.Saldo END) * DI.Cambio ) / @Cambio,
			NuevoSaldo			= ((CASE DE.Crédito WHEN 1 THEN - DE.Saldo ELSE DE.Saldo END) * DI.Cambio ) / @Cambio
	 
		FROM [ANGELITO].[dbo].[CLI_CLIENTES] as CL   WITH(NOLOCK) 
			LEFT OUTER JOIN [ANGELITO].[dbo].[CLI_GRUPOS] GR  WITH(NOLOCK) ON ( CL.GrupoID = GR.ID  )		
			INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES_DEUDAS] DE  WITH(NOLOCK) ON (CL.ID = DE.ClienteID)
			INNER JOIN [ANGELITO].[dbo].[SIS_SUCURSALES] SU  WITH(NOLOCK) ON (DE.SucursalID = SU.Código) 
			INNER JOIN [ANGELITO].[dbo].[SIS_DIVISAS] DI  WITH(NOLOCK) ON (DE.DivisaID = DI.ID)			
			LEFT OUTER JOIN [ANGELITO].[dbo].[EMP_EMPLEADOS] EMP  WITH(NOLOCK) ON (DE.VendedorID = EMP.ID)	
			LEFT OUTER JOIN [ANGELITO].[dbo].[SIS_ZONAS] ZO   WITH(NOLOCK) ON (ZO.ID = CL.ZonaID)			
		WHERE 
			(DE.Anulado = 0) AND DE.Saldo > 0.00 AND (DE.Crédito = 0) AND
			(( CASE @RucCliente WHEN '' THEN '' ELSE CL.Ruc END ) = @RucCliente )  AND
			(( CASE @Cheques WHEN 0 THEN '' ELSE DE.Tipo END ) != 'CLI-CH' )  AND 
			(( CASE @Vencidos WHEN 0 THEN DE.Fecha ELSE DE.Vencimiento END ) <= @Fecha ) AND
			--todo en un mismo Sp
			(( CASE @ValorFavor WHEN 0 THEN '' ELSE De.Crédito END ) = @ValorFavor )   --saldo (negativo el valor)

		
		order by DE.Vencimiento			 
		OFFSET @PageSize * (@PageNumber - 1) ROWS
		FETCH NEXT @PageSize ROWS ONLY;
END

-- deuda total de clientes
SET @DeudaTotal =  ( SELECT ISNULL(sum(ISNULL(de.Saldo,0)),0) as DeudaTotal	 --money	
					FROM [ANGELITO].[dbo].[CLI_CLIENTES] as CL   WITH(NOLOCK) 
					INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES_DEUDAS] DE  WITH(NOLOCK) ON (CL.ID = DE.ClienteID)
					INNER JOIN [ANGELITO].[dbo].[SIS_DIVISAS] DI  WITH(NOLOCK) ON (DE.DivisaID = DI.ID)			
					WHERE 
					(DE.Anulado = 0) AND DE.Saldo > 0.00 AND (DE.Crédito = 0) 
					and (CL.Ruc = @RucCliente))

-- Cuenta total de registros
SET @CantidadRegistros = (SELECT count(*)
					FROM [ANGELITO].[dbo].[CLI_CLIENTES] as CL   WITH(NOLOCK) 
						LEFT OUTER JOIN [ANGELITO].[dbo].[CLI_GRUPOS] GR  WITH(NOLOCK) ON ( CL.GrupoID = GR.ID  )		
						INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES_DEUDAS] DE  WITH(NOLOCK) ON (CL.ID = DE.ClienteID)
						INNER JOIN [ANGELITO].[dbo].[SIS_SUCURSALES] SU  WITH(NOLOCK) ON (DE.SucursalID = SU.Código) 
						INNER JOIN [ANGELITO].[dbo].[SIS_DIVISAS] DI  WITH(NOLOCK) ON (DE.DivisaID = DI.ID)			
						LEFT OUTER JOIN [ANGELITO].[dbo].[EMP_EMPLEADOS] EMP  WITH(NOLOCK) ON (DE.VendedorID = EMP.ID)	
						LEFT OUTER JOIN [ANGELITO].[dbo].[SIS_ZONAS] ZO   WITH(NOLOCK) ON (ZO.ID = CL.ZonaID)			
					WHERE 
						(DE.Anulado = 0) AND DE.Saldo > 0.00 AND (DE.Crédito = 0) AND
						(( CASE @RucCliente WHEN '' THEN '' ELSE CL.Ruc END ) = @RucCliente )  AND
						(( CASE @Cheques WHEN 0 THEN '' ELSE DE.Tipo END ) != 'CLI-CH' )  AND 
						(( CASE @Vencidos WHEN 0 THEN DE.Fecha ELSE DE.Vencimiento END ) <= @Fecha ) AND
						--todo en un mismo Sp
						(( CASE @ValorFavor WHEN 0 THEN '' ELSE De.Crédito END ) = @ValorFavor )   --saldo (negativo el valor)
						)
 
