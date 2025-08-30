USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[BAN_Abonos_Recuperar_Lista]    Script Date: 21/08/2017 11:40:56 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author:		Douglas Bustos
-- Create date: 25 Julio 2017
-- Description:	Recupera listado de Abonos X Estado
-- ===============================================================

ALTER PROCEDURE [dbo].[BAN_Abonos_Recuperar_Lista]
(
	@PageNumber			INT = 1, 
	@PageSize			INT = 5, 
	@ClienteNombre		varchar(50), 
	@VendedorUserName	varchar(25), 
	@Dias				int,
	@CntRegistros		INT OUTPUT

)
AS
SET NOCOUNT ON
 	 
BEGIN

/*
declare @CntRegistros	INT  
exec  [dbo].[BAN_Abonos_Recuperar_Lista] 1,10,'ma','rleon',100, @CntRegistros out
select @CntRegistros
*/
   
   SET @Dias = 1000
			SELECT 

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
			 	Valor		= CONVERT(decimal(12,6),BIC.Valor)
				 
				FROM [ANGELITO].[dbo].[BAN_INGRESOS] BIC
				INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES] CLI ON BIC.DeudorID =CLI.ID
				--INNER JOIN [ANGELITO].[dbo].SEG_USUARIOS SEG
				--ON ISNULL(CLI.VendedorID,'') = ISNULL(SEG.EmpleadoId,'')

		WHERE ISNULL(CLI.Nombre,'') like '%'+RTRIM(LTRIM( ISNULL(@ClienteNombre,'')))+'%'
			 --  AND ISNULL(SEG.Código,'') =
				--CASE 
				--	WHEN LEN(@VendedorUserName)=0 THEN ISNULL(SEG.Código,'')
				--	ELSE @VendedorUserName
				--END 
		      AND  BIC.Fecha >= DATEADD(DAY, -@Dias,GETDATE())
		ORDER BY  BIC.Fecha  ASC
		OFFSET @PageSize * (@PageNumber - 1) ROWS
		FETCH NEXT @PageSize ROWS ONLY;


	SELECT @CntRegistros = COUNT(*)
	  	  	FROM [ANGELITO].[dbo].[BAN_INGRESOS] BIC
				INNER JOIN [ANGELITO].[dbo].[BAN_INGRESOS_DT] BID ON BIC.ID = BID.IngresoID
				INNER JOIN [ANGELITO].[dbo].[CLI_CLIENTES] CLI ON BIC.DeudorID =CLI.ID
				INNER JOIN [ANGELITO].[dbo].SEG_USUARIOS SEG
				ON ISNULL(CLI.VendedorID,'') = ISNULL(SEG.EmpleadoId,'')
			 	WHERE ISNULL(CLI.Nombre,'') like '%'+RTRIM(LTRIM( ISNULL(@ClienteNombre,'')))+'%'
			   AND ISNULL(SEG.Código,'') =
				CASE 
					WHEN LEN(@VendedorUserName)=0 THEN ISNULL(SEG.Código,'')
					ELSE @VendedorUserName
				END 
				AND  BIC.Fecha >= DATEADD(DAY, -@Dias,GETDATE())

END
 
