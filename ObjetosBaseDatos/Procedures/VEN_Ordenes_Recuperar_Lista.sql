USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[VEN_Ordenes_Recuperar_Lista]    Script Date: 21/08/2017 11:51:55 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author:		Douglas Bustos
-- Create date: 25 enero 2017
-- Description:	Recupera listado de Pedidos X Estado
-- ===============================================================

ALTER PROCEDURE [dbo].[VEN_Ordenes_Recuperar_Lista]
(
	@PageNumber			INT = 1, 
	@PageSize			INT = 5, 
	@EstadoId			varchar(10), 
	@ClienteNombre		varchar(50), 
	@ClienteCedulaRuc	varchar(25), 
	@VendedorUserName	varchar(25), 
	@Dias				int,
	@CntRegistros		INT OUTPUT

)
AS
SET NOCOUNT ON
 	 
BEGIN

  SET @Dias=300

/* 
declare @CntRegistros	INT
exec dbo.[VEN_ORDENES_RECUPERAR_LISTA] 1,10,'BORRADOR',NULL,NULL,'RLEON',7, @CntRegistros OUT
select @CntRegistros

0000000002

  DECLARE @EstadoId varchar(10) SET @EstadoId=''
*/	 
	 

	 --ISNULL(CLI.Cédula,'') =
		--		CASE 
		--			WHEN LEN(ISNULL(@ClienteCedulaRuc,''))=0 THEN ISNULL(CLI.Cédula,'')
		--			ELSE RTRIM(LTRIM(ISNULL(@ClienteCedulaRuc,'')))
		--		END


		SELECT 
					 ORD.ID
					,ORD.ClienteID
					,ClienteNombre	= CLI.Nombre 
					,ClienteRuc		= CLI.Ruc 
					,ORD.VendedorID
					,VendedorNombre = EMP.Nombre
					,VendedorUserName = SEG.Código 
					,ORD.Fecha
					,ORD.Total
					,ORD.Estado
					,ORD.Negado
					,ORD.Aprobado 
					,ORD.Egresado
					,ORD.Facturado
					,ORD.Anulado
					--,DATEDIFF(DAY,ORD.Fecha,GETDATE())
					,BackColor = CASE 
							 WHEN (DATEDIFF(DAY,GETDATE(),ORD.Fecha)=0) THEN 'background-color:aquamarine'
							 ELSE 'background-color:white'
							 END

		
		FROM [ANGELITO].[dbo].VEN_ORDENES ORD
		INNER JOIN [ANGELITO].[dbo].CLI_CLIENTES CLI
		ON ORD.ClienteID = CLI.ID
		INNER JOIN [ANGELITO].[dbo].EMP_EMPLEADOS EMP
		ON ISNULL(EMP.ID,'') = ORD.VendedorID
		INNER JOIN [ANGELITO].[dbo].SEG_USUARIOS SEG
				ON ISNULL(ORD.VendedorID,'') = ISNULL(SEG.EmpleadoId,'')

		WHERE  
		 ISNULL(CLI.Nombre,'') like '%'+RTRIM(LTRIM( ISNULL(@ClienteNombre,'')))+'%'

			AND  ORD.FECHA >= DATEADD(DAY, -@Dias,GETDATE())
    		AND	ISNULL(ORD.ESTADO,'') =
					CASE 
						WHEN LEN(@EstadoId)=0 THEN ISNULL(ORD.ESTADO,'')
						ELSE @EstadoId
					END
		     AND ISNULL(SEG.Código,'') =
				CASE 
					WHEN LEN(@VendedorUserName)=0 THEN ISNULL(SEG.Código,'')
					ELSE @VendedorUserName
				END
		ORDER BY  ORD.Fecha  ASC
		OFFSET @PageSize * (@PageNumber - 1) ROWS
		FETCH NEXT @PageSize ROWS ONLY;


	SELECT @CntRegistros = COUNT(*)
	  	FROM [ANGELITO].[dbo].VEN_ORDENES ORD
		INNER JOIN [ANGELITO].[dbo].CLI_CLIENTES CLI
		ON ORD.ClienteID = CLI.ID
		INNER JOIN [ANGELITO].[dbo].EMP_EMPLEADOS EMP
		ON ISNULL(EMP.ID,'') = ORD.VendedorID
		INNER JOIN [ANGELITO].[dbo].SEG_USUARIOS SEG
				ON ISNULL(ORD.VendedorID,'') = ISNULL(SEG.EmpleadoId,'')
 		WHERE   
			 ISNULL(CLI.Nombre,'') like '%'+RTRIM(LTRIM( ISNULL(@ClienteNombre,'')))+'%'
			AND ORD.FECHA >= DATEADD(DAY, -@Dias,GETDATE())
			AND	ISNULL(ORD.ESTADO,'') =
					CASE 
						WHEN LEN(@EstadoId)=0 THEN ISNULL(ORD.ESTADO,'')
						ELSE @EstadoId
					END
		    AND ISNULL(SEG.Código,'') =
				CASE 
					WHEN LEN(@VendedorUserName)=0 THEN ISNULL(SEG.Código,'')
					ELSE @VendedorUserName
				END
				  
			 

END

 

-- select * from [ANGELITO].[dbo].CLI_CLIENTES CLI
