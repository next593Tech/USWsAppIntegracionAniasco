USE [NexoDobraWebApiDEMO]
GO
/****** Object:  StoredProcedure [dbo].[CLI_Clientes_CreateMin]    Script Date: 21/08/2017 11:41:46 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER   PROCEDURE [dbo].[CLI_Clientes_CreateMin]
@ID							char (10) OUT,
@Nombre						varchar (50),
@ClaseCodigo				char(2),
@TipoIdentificacionCodigo	char(2), 
@SecuenciaId				char(10),
@TipoPersonaCodigo			char(2), 
@Vendedorid					char(10),
@CedRucPass					char(13),
@Direccion					varchar	(200), 
@Telefono					varchar (20),
@Email						varchar	(200)

AS



DECLARE 
@Código varchar (15),
@GrupoID char	(10), 
@ZonaID char (10),
 

@DivisiónID char (10), 
@Identificación varchar(2),
@TérminoID char (10),
@FormaPago varchar(20), 
@Banco varchar(30),
@Cuenta varchar(20),
@TasaDescuento numeric(6,2),
@TasaRecargo numeric(6,2),
@Contacto varchar (60),

@Dirección varchar (100),
@Teléfono1 varchar (20),
@Teléfono2 varchar (20),
@Teléfono3 varchar (20),
@Teléfono4 varchar (20),

@Ciudad varchar(40),
@Cupo	money,
@CupoFactura numeric(2),
@SecuenciaRuta Numeric(5),

@Foto varchar (200),
@Fecha datetime,
@Firma1 varchar (200),
@Firma2 varchar (200),
@nacimiento datetime,

@Folder varchar	 (6),
@Calificación char (1),
@GNombre varchar(50),
@GCédula varchar(15),
@GEmpresa varchar(50),
@GZonaID char(10),
@GDirección varchar(100),
@GTeléfono1 char(20),
@GTeléfono2 char(20),
@GNota varchar(1024),
@ClienteID char(10),
@CuentaID char(10),
@DiaCorte numeric(3,0),
@IsClub bit,
@PrintSaldos bit,
@EmpresaPrint bit,
@Publica bit,
@Anulado bit,
@FactVencidas bit,
@WWW varchar (50),
@ValorVivienda	money,
@TiempoVivienda	numeric(5,2) ,
@NoCargas	numeric(5,2),
@Sexo	Varchar(1) ,
@EstadoCivil	varchar(2),
@OrigenIngreso	varchar(1) ,
@TipoVivienda	varchar(2),
@ClaseCodigoSujeto	varchar(1),
@CreadoPor varchar (15),
@Nota varchar (1024),
@SucursalID char (2), 
@PcID varchar (50),
@relacionado bit = 0,
@EmailFE varchar(100) = '',
@EMailCCFE	varchar(100) = '',
@RazonSocial varchar(200) = '',
@TipoCliente varchar(2) = ''


DECLARE @ERROR			AS CHAR(10) 
DECLARE @CodSecCliId		AS varchar(50)
SET @CodSecCliId			= 'CLI_CLIENTES-ID-00' 

--forma de pago AL Contado

SET @GrupoID			= '0000000001'
SET  @TérminoID			= '0000001583'
SET @FormaPago			='CONTADO'
SET @identificación		= ''
SET @DivisiónID			= '0000000001'
SET @SucursalID			='00'
SET @Dirección			= @Direccion
SET @Teléfono1			= @Telefono
SET @TipoCliente		= @TipoPersonaCodigo
SET @ZonaID				='0000000001'
SET @Ciudad				='GUAYAQUIL'
 

 BEGIN TRY  

 --select * from [ANGELITO].[dbo].SIS_CONTADORES where código like '%VEN_ORDENES-ID-00%'
 
EXEC [dbo].[SIS_GetNextIDOUT] @CodSecCliId,1, @ID OUT, @ERROR OUT
 
SET @Código= @CedRucPass

SET @Anulado =0
 

INSERT INTO [ANGELITO]..CLI_CLIENTES ( 
	ID,    Código,  GrupoID, ZonaID,   Vendedorid, Clase, Contacto, Cédula,     Dirección, Teléfono1, Teléfono2, tipo_identificación,
	TérminoID, DivisiónID, FormaPago, Banco, Cuenta,TasaDescuento, SecuenciaID, SecuenciaRuta, TasaRecargo,
	Teléfono3, Teléfono4, Ruc, Ciudad, Cupo, CupoFactura, Nombre, Foto, Fecha, Firma1, Firma2, Nacimiento, Email, Folder, Calificación, 
	GNombre, GCédula, GEmpresa, GZonaID, GDirección, GTeléfono1, GTeléfono2, GNota, Anulado, IsClub, PrintSaldos, ClienteID, CuentaID, DíaCorte, EmpresaPrint, Publica, FactVencidas,
	ValorVivienda, TiempoVivienda, NoCargas, Sexo, EstadoCivil, OrigenIngreso, TipoVivienda, ClaseSujeto, 
	WWW, CreadoPor, CreadoDate, Nota, SucursalID, PcID, XMLAnexos, Relacionado, EmailFE, EMailCCFE, RazonSocial, TipoCliente )

VALUES ( 
	@ID, @Código, @GrupoID, @ZonaID, @VendedorID, @ClaseCodigo, @Contacto, SUBSTRING (@CedRucPass,1,10), @Dirección, @Teléfono1, @Teléfono2, @TipoIdentificacionCodigo,
	@TérminoID, @DivisiónID, @FormaPago, @Banco, @Cuenta,@TasaDescuento,@SecuenciaID, @SecuenciaRuta, @TasaRecargo,
	@Teléfono3, @Teléfono4, @CedRucPass, @Ciudad, @Cupo, @CupoFactura, @Nombre, @Foto, @Fecha, @Firma1, @Firma2, @Nacimiento, @Email, @Folder, @Calificación, 
	@GNombre, @GCédula, @GEmpresa, @GZonaID, @GDirección, @GTeléfono1, @GTeléfono2, @GNota, @Anulado,  @IsClub,  @PrintSaldos,  @ClienteID, @CuentaID, @DiaCorte, @EmpresaPrint, @Publica, @FactVencidas,
	@ValorVivienda, @TiempoVivienda, @NoCargas, @Sexo, @EstadoCivil, @OrigenIngreso, @TipoVivienda, @ClaseCodigoSujeto, 
	@WWW, @CreadoPor, GETDATE(), @Nota, @SucursalID, @PCID, NULL, @relacionado, @EmailFE, @EMailCCFE, @RazonSocial, @TipoCliente )


 END TRY  
BEGIN CATCH 


						 SELECT  
							ERROR_NUMBER()		AS ErrorNumber 
							,ERROR_SEVERITY()	AS ErrorSeverity 
							,ERROR_STATE()		AS ErrorState 
							,ERROR_PROCEDURE()  AS ErrorProcedure 
							,ERROR_LINE()       AS ErrorLine 
							,ErrorMessage
							= CASE WHEN
								ERROR_NUMBER() = 2627 THEN 'ALREADYEXISTSTHERECORD'   
							  ELSE
							   ERROR_MESSAGE()
							  END
 
							IF @@TRANCOUNT > 0 
										  ROLLBACK TRANSACTION; 


END CATCH  
 
 IF @@TRANCOUNT > 0 
             BEGIN
                    COMMIT TRANSACTION; 
                    SELECT  
                     0		AS ErrorNumber 
                    ,0		AS ErrorSeverity 
                    ,0		AS ErrorState 
                    ,'NN'	AS ErrorProcedure 
                    ,0           AS ErrorLine 
                    ,'Grabación exitosa'	AS ErrorMessage; 
             END
  
