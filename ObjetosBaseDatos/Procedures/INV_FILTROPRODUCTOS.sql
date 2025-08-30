USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[INV_FiltroProductos]    Script Date: 21/08/2017 11:47:10 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Douglas Bustos  
-- Create date: 20 Enero 2017
-- Description:	Consulta Paginada de Productos 
-- =============================================

/*

declare @salida int
 exec [dbo].[INV_FiltroProductos] 1,50,'01','Fresco solo naran', @salida OUT
 select @salida

*/
ALTER PROCEDURE [dbo].[INV_FiltroProductos] 
 @PageNumber as int, @PageSize as int,  @ClaseCliente AS varchar(2), @Txt as varchar(50), @TotalSalida as int OUTPUT
AS

BEGIN

	SET NOCOUNT ON;
	 
	SELECT 
		PRO.ID, 
		PRO.Código AS Codigo,
		PRO.Nombre, 
		PRO.Descripción AS Descripcion,
		PRO.Foto, 
		ltrim(rtrim(EMP.Nombre)) AS Empaque,

		Costo = PRO.CostoPromedio,

		EMP.Factor AS Factor,
		
		Precio = CASE
				WHEN @ClaseCliente='01' THEN Convert(decimal(18,4),EMP.Precio)
				WHEN @ClaseCliente='02' THEN Convert(decimal(18,4),PRO.Precio5/EMP.Factor)
				WHEN @ClaseCliente='03' THEN Convert(decimal(18,4),PRO.Precio6/EMP.Factor)
				WHEN @ClaseCliente='04' THEN Convert(decimal(18,4),PRO.Precio7/EMP.Factor)
				WHEN @ClaseCliente='05' THEN Convert(decimal(18,4),PRO.Precio8/EMP.Factor)
				WHEN @ClaseCliente='06' THEN Convert(decimal(18,4),PRO.Precio9/EMP.Factor)
				WHEN @ClaseCliente='07' THEN Convert(decimal(18,4),PRO.Precio10/EMP.Factor)
				 
				ELSE Convert(decimal(18,4),PRO.Precio1/EMP.Factor)
				END,
		PRO.stock, 
		AIVA = CASE 
				WHEN IVA.Valor  IS NULL THEN Convert(bit,0)	
				ELSE Convert(bit,1)
				END,

		PIVA = CASE 
				WHEN IVA.Valor  IS NULL THEN Convert(bit,0)	
				ELSE Convert(decimal(18,4),IVA.Valor)
				END,
		  
		AICE = CASE 
				WHEN  ICE.Valor  IS NULL THEN Convert(bit,0)	
				ELSE Convert(bit,1)
				END,
	
		PICE = CASE 
				WHEN  ICE.Valor IS NULL THEN Convert(bit,0)	
				ELSE Convert(decimal(18,4), ICE.Valor)
				END,

		AVER = CASE 
				WHEN  VER.Valor  IS NULL THEN Convert(bit,0)	
				ELSE Convert(bit,1)
				END,
		PVER = CASE 
				WHEN  VER.Valor  IS NULL THEN 0	
				ELSE Convert(decimal(18,4),VER.Valor)
				END 
	 
		

	FROM [ANGELITO].[dbo].INV_PRODUCTOS AS PRO 
	LEFT JOIN [ANGELITO].[dbo].[SRI_PORCENTAJE_ICE] ICE ON PRO.Impuesto4ID	= ICE.ID 
	LEFT JOIN [ANGELITO].[dbo].[SIS_PARAMETROS]	  IVA ON IVA.ID				= PRO.ImpuestoID 
	LEFT JOIN [ANGELITO].[dbo].[SIS_PARAMETROS]	  VER ON VER.ID				= PRO.[ImpuestoVerdeID] 
	INNER JOIN 	[ANGELITO].[dbo].[INV_PRODUCTOS_EMPAQUES]     EMP ON EMP.ProductoID = PRO.ID 	
	INNER JOIN [ANGELITO].[dbo].[SIS_PARAMETROS]	          PEMP ON PEMP.Nombre				= EMP.Nombre		 
	WHERE  EMP.Anulado=0 and  ((CHARINDEX( @Txt,PRO.Nombre)>0)  OR (LTRIM(RTRIM(PRO.Nombre)) LIKE  '%'+ LTRIM(RTRIM(@Txt)) + '%') OR (LTRIM(RTRIM(PRO.Código)) LIKE  '%'+ @Txt + '%'))
	ORDER BY PRO.Nombre, Factor ASC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;	

	SET @TotalSalida = (SELECT Count(*) FROM [ANGELITO].[dbo].INV_PRODUCTOS as PRO WITH(NOLOCK)  WHERE  
		( (CHARINDEX(  @Txt,PRO.Nombre)>0) OR (LTRIM(RTRIM(PRO.Nombre)) LIKE  '%'+ LTRIM(RTRIM(@Txt)) + '%') OR (LTRIM(RTRIM(PRO.Código)) LIKE  '%'+ @Txt + '%')  ))

END

/* 
select * from [ANGELITO].[dbo].INV_PRODUCTOS_EMPAQUES where ProductoId='0000002492'
 select * from  [ANGELITO].[dbo].INV_PRODUCTOS where nombre like '%FRESCO SOLO NARANJA 10g%'
 select * from [ANGELITO].[dbo].[SIS_PARAMETROS] where PadreId='0000000016' Nombre=ltrim(rtrim('Und.'))

 */
