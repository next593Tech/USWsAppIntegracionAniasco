DECLARE @XML AS XML
SET @XML = '
  <AbonoCab_InputDto xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" CodCliente="0912604303" IdClienteDeuda="0000000048" Saldo="0" ClienteNombre="Alfred Brito" ValorTotal="2" ID="0000000360" BancoID="0000000004" PcID="" Fecha="2017-01-27T11:21:54.9218842-05:00" Detalle="Alfred Brito Cancela: " Tipo="CLI-IN" RFIR="0" RFIVA="0" Valor="2" Descuento="0" Financiero="0" Faltante="0" Sobrante="0" Valor_Base="0" Anulado="false" Cambio="0" CreadoPor="" SucursalID="" CreadoDate="2017-01-27T11:21:54.9218842-05:00" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0" NumRecibo="">
  <AbonoDistribucion_InputDto IngresoID="0000000360" DeudaID="0000000074" SucursalID="" PcID="" Valor="1" Cambio="0" Saldo="4.76" Dif_Cambio="0" Fecha="2017-01-27T11:22:01.3882542-05:00" Vencimiento="2017-01-27T11:22:01.3882542-05:00" Tipo="VEN-FA" Número="001-001-000000162" Detalle="Alfred Brito Cancela: VEN-FA001-001-000000162" Crédito="false" RubroID="1" CtaCxCID="0000000025" CambioDia="0" CreadoDate="2017-01-27T11:22:01.3892538-05:00" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0">
    <ExportadoDate>0001-01-01T00:00:00</ExportadoDate>
  </AbonoDistribucion_InputDto>
  <AbonoDistribucion_InputDto IngresoID="0000000360" DeudaID="0000000048" SucursalID="" PcID="" Valor="1" Cambio="0" Saldo="6.4" Dif_Cambio="0" Fecha="2017-01-27T11:22:01.3892538-05:00" Vencimiento="2017-01-27T11:22:01.3892538-05:00" Tipo="VEN-FA-CO" Número="001-001-000000003" Detalle="Alfred Brito Cancela: VEN-FA-CO001-001-000000003" Crédito="false" RubroID="1" CtaCxCID="0000000025" CambioDia="0" CreadoDate="2017-01-27T11:22:01.3892538-05:00" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0">
    <ExportadoDate>0001-01-01T00:00:00</ExportadoDate>
  </AbonoDistribucion_InputDto>
  <AbonoDet_InputDto IngresoID="0000000360" SucursalID="" PcID="" Tipo="Efectivo" Girador="Alfred Brito" Fecha="2017-01-27T11:22:01.3862526-05:00" Valor="2" Valor_Base="2" DifenCambio="0" Depositado="2" DivisaID="1" Cambio="1" CreadoPor="" CreadoDate="2017-01-27T11:22:01.3872547-05:00" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0" Transferido="false">
    <ExportadoDate>0001-01-01T00:00:00</ExportadoDate>
  </AbonoDet_InputDto>
  <AbonoAsiento_InputDto ID="0000000465" DocumentoID="0000000360" Número="0000000465" Fecha="2017-01-27T11:22:01.3932533-05:00" Detalle="Alfred Brito CLI-IN " Tipo="CLI-IN" Anulado="false" Pendiente="false" CreadoDate="2017-01-27T11:22:01.3942529-05:00" EditadoDate="2017-01-27T11:22:01.3942529-05:00" AnuladoDate="2017-01-27T11:22:01.3942529-05:00" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0">
    <ExportadoDate>0001-01-01T00:00:00</ExportadoDate>
    <AbonoAsientoDT_InputDto AsientoID="0000000465" CuentaID="0000000006" SucursalID="" Débito="true" Valor="2" Detalle="Alfred Brito CLI-IN " ExportadoDate="0001-01-01T00:00:00" PCID="" Valor_Base="2" CreadoPor="" Cambio="1" CreadoDate="2017-01-27T11:22:01.3872547-05:00" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0" Anulado="false" />
    <AbonoAsientoDT_InputDto AsientoID="0000000465" CuentaID="0000000025" SucursalID="" Débito="false" Valor="2" Detalle="Alfred Brito CLI-IN " ExportadoDate="0001-01-01T00:00:00" PCID="" Valor_Base="2" CreadoPor="" Cambio="0" CreadoDate="2017-01-27T11:21:54.9218842-05:00" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0" Anulado="false" />
  </AbonoAsiento_InputDto>
  <AbonoBanco_Kardex_InputDto ID="0000000236" BancoID="0000000004" DocumentoID="0000000360" SucursalID="" PcID="" Cambio="0" Fecha="2017-01-27T11:21:54.9218842-05:00" Fecha_Banco="2017-01-27T11:21:54.9218842-05:00" Tipo="CLI-IN" Detalle="Alfred Brito CLI-IN " Débito="true" Valor="2" Valor_Base="0" Conciliado="false" CreadoPor="" CreadoDate="2017-01-27T11:22:01.3982543-05:00" Cheque="" Fecha_Cheque="2017-01-27T11:22:01.3982543-05:00" DivisiónID="" Anulado="false" AnuladoDate="0001-01-01T00:00:00" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0">
    <ExportadoDate>0001-01-01T00:00:00</ExportadoDate>
  </AbonoBanco_Kardex_InputDto>
  <AbonoClienteDeudas_InputDto ID="0000000272" ClienteID="0000000099" DocumentoID="0000000360" AsientoID="0000000465" Detalle="Alfred Brito CLI-IN " Valor="1" ValorBase="1" Fecha="2017-01-27T11:22:01.3882542-05:00" Vencimiento="2017-01-27T11:22:01.3882542-05:00" RubroID="1" CtaCxCID="0000000025" Cambio="0" Saldo="0" Tipo="CLI-IN" Crédito="true" Facturado="false" DeudaID="0000000074" CambioDeuda="0" VendedorID="" Anulado="false" CreadoPor="" CreadoDate="2017-01-27T11:22:01.3922518-05:00" EditadoDate="0001-01-01T00:00:00" AnuladoDate="0001-01-01T00:00:00" SucursalID="" PcID="" DivisiónID="" Retenido="false" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0" Inmobiliaria="false">
    <ExportadoDate>0001-01-01T00:00:00</ExportadoDate>
  </AbonoClienteDeudas_InputDto>
  <AbonoClienteDeudas_InputDto ID="0000000272" ClienteID="0000000099" DocumentoID="0000000360" AsientoID="0000000465" Detalle="Alfred Brito CLI-IN " Valor="1" ValorBase="1" Fecha="2017-01-27T11:22:01.3892538-05:00" Vencimiento="2017-01-27T11:22:01.3892538-05:00" RubroID="1" CtaCxCID="0000000025" Cambio="0" Saldo="0" Tipo="CLI-IN" Crédito="true" Facturado="false" DeudaID="0000000048" CambioDeuda="0" VendedorID="" Anulado="false" CreadoPor="" CreadoDate="2017-01-27T11:22:01.3932533-05:00" EditadoDate="0001-01-01T00:00:00" AnuladoDate="0001-01-01T00:00:00" SucursalID="" PcID="" DivisiónID="" Retenido="false" Exportado="0" ExportadoUpdate="1" ExportadoCandidate="0" Inmobiliaria="false">
    <ExportadoDate>0001-01-01T00:00:00</ExportadoDate>
  </AbonoClienteDeudas_InputDto>
</AbonoCab_InputDto>'

--SELECT @XML

--execute BAN_Payment_Insert_Nexo @xml


					-- Abono cabecera
                    SELECT        
                    PaymenttXml.value('@ID','CHAR(10)') AS ID,  
					PaymenttXml.value('@BancoID','CHAR(10)') AS BancoID,   
					PaymenttXml.value('@AsientoID','CHAR(10)') AS AsientoID,  
					PaymenttXml.value('@DeudorID','VARCHAR(10)') AS DeudorID,   
					PaymenttXml.value('@PcID','VARCHAR(50)') AS PcID, 
					PaymenttXml.value('@Número','CHAR(10)') AS Número,   
					PaymenttXml.value('@Fecha','DateTime') AS Fecha,   
					PaymenttXml.value('@Detalle','VARCHAR(100)') AS Detalle,
					--@Detalle, --Voy a probar DETALLE sale o no
					PaymenttXml.value('@Tipo','VARCHAR(10)') AS Tipo,   
					PaymenttXml.value('@RFIR','MONEY') AS RFIR,   
					PaymenttXml.value('@RFIVA','MONEY') AS RFIVA,   
					PaymenttXml.value('@Valor','money') AS Valor,  					 
					PaymenttXml.value('@Descuento','money') AS Descuento,   
					PaymenttXml.value('@Financiero','money') AS Financiero,   
					PaymenttXml.value('@Faltante','money') AS Faltante,   
					PaymenttXml.value('@Sobrante','money') AS Sobrante,   
					PaymenttXml.value('@Valor_Base','money') AS Valor_Base,   
					PaymenttXml.value('@Anulado','BIT') AS Anulado,  
					 
					 PaymenttXml.value('@DivisaID', 'CHAR(10)') AS DivisaID,   					
					PaymenttXml.value('@Cambio','decimal(12,6)') AS Cambio,   
					PaymenttXml.value('@CreadoPor','VARCHAR(15)') AS CreadoPor,   
					PaymenttXml.value('@Nota','VARCHAR(1024)') AS Nota,   
					PaymenttXml.value('@ExportadoDate', 'datetime') AS ExportadoDate,
					PaymenttXml.value('@EditadoPor', 'VARCHAR(15)') AS EditadoPor,
					PaymenttXml.value('@AnuladoPor' ,'varchar(15)') AS AnuladoPor,
					PaymenttXml.value('@AnuladoDate', 'datetime') AS AnuladoDate,
					PaymenttXml.value('@AnuladoNota', 'varchar(1024)') AS AnuladoNota,
					PaymenttXml.value('@EditadoDate', 'datetime') AS EditadoDate,					
					PaymenttXml.value('@SucursalID', 'CHAR(2)') AS SucursalID,   
					PaymenttXml.value('@CreadoDate','DateTime') AS Fecha,   
					PaymenttXml.value('@DivisiónID', 'CHAR(10)') AS DivisiónID,   
					PaymenttXml.value('@ExportadoPor','varchar(15)' ) AS ExportadoPor,
					PaymenttXml.value('@Exportado','numeric(1, 0)' ) AS Exportado,
					PaymenttXml.value('@ExportadoUpdate','numeric(1, 0)' ) AS ExportadoUpdate,
					PaymenttXml.value('@ExportadoCandidate','numeric(1, 0) ') AS ExportadoCandidate,
					PaymenttXml.value('@OrdenID','varchar(10)') AS OrdenID,
					PaymenttXml.value('@NumRecibo','VARCHAR(10)') AS NumRecibo   
					
                    FROM
                    @xml.nodes('/AbonoCab_InputDto')AS TEMPTABLE(PaymenttXml)
 
 
 
                    -- Abono detalle
                    SELECT        
                    --RIGHT(@Ban_IngresoDT_ID,7) + Replicate('0', (3 - len(ltrim(str( ROW_NUMBER() OVER(ORDER BY PaymenttXml.value('@ID','CHAR(10)') ASC)))))) + ltrim(str( ROW_NUMBER() OVER(ORDER BY PaymenttXml.value('@ID','CHAR(10)') ASC))),
					PaymenttXml.value('@IngresoID','CHAR(10)') AS IngresoID,   
					PaymenttXml.value('@SucursalID', 'CHAR(2)') AS SucursalID,   
					PaymenttXml.value('@PcID','VARCHAR(50)') AS PcID, 
					PaymenttXml.value('@Tipo','VARCHAR(10)') AS Tipo,   
					PaymenttXml.value('@Número','CHAR(10)') AS Número,  
					PaymenttXml.value('@Banco','VARCHAR(30)') AS Banco,   
					PaymenttXml.value('@Cuenta','VARCHAR(15)') AS Cuenta,   
					PaymenttXml.value('@Girador','VARCHAR(30)') AS Girador,   
					PaymenttXml.value('@Fecha','DateTime') AS Fecha,   
					PaymenttXml.value('@Valor','money') AS Valor,  
					PaymenttXml.value('@Valor_Base','money') AS Valor_Base,  
					PaymenttXml.value('@DifenCambio','money') AS DifenCambio,  
					PaymenttXml.value('@Depositado','money') AS Depositado,
									 
					PaymenttXml.value('@DivisaID', 'CHAR(10)') AS DivisaID,  
					PaymenttXml.value('@Cambio','decimal(12,6)') AS Cambio,   
					PaymenttXml.value('@ExportadoDate', 'datetime') AS ExportadoDate,
					PaymenttXml.value('@CreadoPor','VARCHAR(15)') AS CreadoPor,   
					PaymenttXml.value('@CreadoDate','DateTime') AS Fecha,   
					PaymenttXml.value('@ExportadoPor','varchar(15)' ) AS ExportadoPor,
					PaymenttXml.value('@Exportado','numeric(1, 0)' ) AS Exportado,
					PaymenttXml.value('@ExportadoUpdate','numeric(1, 0)' ) AS ExportadoUpdate,
					PaymenttXml.value('@ExportadoCandidate','numeric(1, 0) ') AS ExportadoCandidate,
					PaymenttXml.value('@Transferido','BIT') AS Transferido,
					PaymenttXml.value('@TransferenciaID','VARCHAR(10)') AS TransferenciaID,
					PaymenttXml.value('@FormaPago','VARCHAR(2) ') AS FormaPago
					
					
                    FROM
                    @xml.nodes('/AbonoCab_InputDto/AbonoDet_InputDto')AS TEMPTABLE(PaymenttXml)


					--Deudas
                    SELECT        
					PaymenttXml.value('@IngresoID','CHAR(10)') AS IngresoID, 
					PaymenttXml.value('@DeudaID','VARCHAR(10)') AS DeudaID,  -- DeudorID
                    PaymenttXml.value('@SucursalID', 'CHAR(2)') AS SucursalID,   
					PaymenttXml.value('@PcID','VARCHAR(50)') AS PcID, 
					PaymenttXml.value('@Valor','money') AS Valor, 
					
					PaymenttXml.value('@Cambio','decimal(12,6)') AS Cambio, 
					PaymenttXml.value('@DivisaID', 'CHAR(10)') AS DivisaID,
					PaymenttXml.value('@Saldo','money') AS Saldo,
					PaymenttXml.value('@DifenCambio','money') AS DifenCambio,
					PaymenttXml.value('@Fecha','DateTime') AS Fecha, 
					PaymenttXml.value('@Vencimiento','DateTime') AS Vencimiento, 
					PaymenttXml.value('@Tipo','VARCHAR(10)') AS Tipo,   
					PaymenttXml.value('@Número','CHAR(10)') AS Número,  
					PaymenttXml.value('@Detalle','VARCHAR(100)') AS Detalle, 
					PaymenttXml.value('@Crédito','BIT') AS Crédito,
					PaymenttXml.value('@RubroID', 'CHAR(10)') AS RubroID,
					PaymenttXml.value('@CtaCxCID', 'CHAR(10)') AS CtaCxCID,
					PaymenttXml.value('@CambioDia','decimal(12,6)') AS CambioDia, 
					PaymenttXml.value('@DivisiónID', 'CHAR(10)') AS DivisiónID,
					PaymenttXml.value('@ExportadoDate', 'datetime') AS ExportadoDate,
					PaymenttXml.value('@CreadoPor','VARCHAR(15)') AS CreadoPor,   
					PaymenttXml.value('@CreadoDate','DateTime') AS CreadoDate,  
					PaymenttXml.value('@ExportadoPor','varchar(15)' ) AS ExportadoPor,
					PaymenttXml.value('@Exportado','numeric(1, 0)' ) AS Exportado,
					PaymenttXml.value('@ExportadoUpdate','numeric(1, 0)' ) AS ExportadoUpdate,
					PaymenttXml.value('@ExportadoCandidate','numeric(1, 0) ') AS ExportadoCandidate
					
                    FROM
                    @xml.nodes('/AbonoCab_InputDto/AbonoDistribucion_InputDto')AS TEMPTABLE(PaymenttXml)



					--Asiento cab
                    SELECT        
					PaymenttXml.value('@ID','CHAR(10)') AS ID, 
					PaymenttXml.value('@DocumentoID','CHAR(10)') AS DocumentoID,   
					PaymenttXml.value('@Número','CHAR(10)') AS Número, 
					PaymenttXml.value('@Fecha','DateTime') AS Fecha, 
					PaymenttXml.value('@Detalle','VARCHAR(100)') AS Detalle, 
					PaymenttXml.value('@Tipo','VARCHAR(10)') AS Tipo, 
					PaymenttXml.value('@Nota','VARCHAR(1024)') AS Nota,
					PaymenttXml.value('@Anulado','BIT') AS Anulado,
					PaymenttXml.value('@Pendiente','BIT') AS Pendiente,
					PaymenttXml.value('@ExportadoDate', 'datetime') AS ExportadoDate,
					PaymenttXml.value('@CreadoPor','VARCHAR(15)') AS CreadoPor, 
					PaymenttXml.value('@EditadoPor','VARCHAR(15)') AS EditadoPor, 
					PaymenttXml.value('@AnuladoPor','VARCHAR(15)') AS AnuladoPor, 
					PaymenttXml.value('@PCID','VARCHAR(50)') AS PCID, 
                    PaymenttXml.value('@SucursalID', 'CHAR(2)') AS SucursalID,   
					PaymenttXml.value('@CreadoDate','DateTime') AS CreadoDate,
					PaymenttXml.value('@EditadoDate','DateTime') AS EditadoDate,
					PaymenttXml.value('@AnuladoDate','DateTime') AS AnuladoDate,
					PaymenttXml.value('@AnuladoNota','varchar(1024)') AS AnuladoNota,
					PaymenttXml.value('@DivisiónID', 'CHAR(10)') AS DivisiónID,
					PaymenttXml.value('@ExportadoPor','varchar(15)' ) AS ExportadoPor,
					PaymenttXml.value('@Exportado','numeric(1, 0)' ) AS Exportado,
					PaymenttXml.value('@ExportadoUpdate','numeric(1, 0)' ) AS ExportadoUpdate,
					PaymenttXml.value('@ExportadoCandidate','numeric(1, 0) ') AS ExportadoCandidate
					
                    FROM
                    @xml.nodes('/AbonoCab_InputDto/AbonoAsiento_InputDto')AS TEMPTABLE(PaymenttXml)



					--Asiento DT
                    SELECT        
					--RIGHT(@AsientoDT_ID,7) + Replicate('0', (3 - len(ltrim(str( ROW_NUMBER() OVER(ORDER BY PaymenttXml.value('@ID','CHAR(10)') ASC)))))) + ltrim(str( ROW_NUMBER() OVER(ORDER BY PaymenttXml.value('@ID','CHAR(10)') ASC))),
					PaymenttXml.value('@AsientoID','CHAR(10)') AS AsientoID,
					PaymenttXml.value('@CuentaID','CHAR(10)') AS CuentaID,
					PaymenttXml.value('@SucursalID','CHAR(2)') AS SucursalID, 
					PaymenttXml.value('@Débito','BIT') AS Débito,
					PaymenttXml.value('@Valor','money') AS Valor, 
					PaymenttXml.value('@Detalle','VARCHAR(100)') AS Detalle, 
					--PaymenttXml.value('@ExportadoDate', 'datetime') AS ExportadoDate,
					NULL AS ExportadoDate,
					PaymenttXml.value('@PCID','VARCHAR(50)') AS PCID, 
					PaymenttXml.value('@Valor_Base','money') AS Valor_Base,
					PaymenttXml.value('@CreadoPor','VARCHAR(15)') AS CreadoPor,
					
					PaymenttXml.value('@DivisaID','CHAR(10)') AS DivisaID, 
					PaymenttXml.value('@Cambio', 'decimal(12,6)') AS Cambio,
					PaymenttXml.value('@CreadoDate','DateTime') AS CreadoDate,
					PaymenttXml.value('@ExportadoPor','varchar(15)' ) AS ExportadoPor,
					PaymenttXml.value('@Exportado','numeric(1, 0)' ) AS Exportado,
					PaymenttXml.value('@ExportadoUpdate','numeric(1, 0)' ) AS ExportadoUpdate,
					PaymenttXml.value('@ExportadoCandidate','numeric(1, 0) ') AS ExportadoCandidate,
					PaymenttXml.value('@Anulado','BIT') AS Anulado,
					PaymenttXml.value('@EditadoPor','varchar(60)' ) AS EditadoPor
					
                    FROM
                    @xml.nodes('/AbonoCab_InputDto/AbonoAsiento_InputDto/AbonoAsientoDT_InputDto')AS TEMPTABLE(PaymenttXml)

      
					--Clientes deudas
					SELECT
					PaymenttXml.value('@ID','CHAR(10)') AS ID, 
					PaymenttXml.value('@ClienteID','char(10)' ) AS ClienteID,	
					PaymenttXml.value('@DocumentoID','char(10)' ) AS DocumentoID,	
					PaymenttXml.value('@AsientoID','char(10)' ) AS AsientoID,	
					PaymenttXml.value('@Número','varchar(17)' ) AS Número,	
					PaymenttXml.value('@Detalle','varchar(100)' ) AS Detalle,	
					PaymenttXml.value('@Valor','money' ) AS Valor,	     
					PaymenttXml.value('@ValorBase','money' ) AS ValorBase,	
					PaymenttXml.value('@Fecha','datetime' ) AS Fecha,	
					PaymenttXml.value('@Vencimiento','datetime' ) AS Vencimiento,	
					PaymenttXml.value('@RubroID','varchar(50)' ) AS RubroID,	
					PaymenttXml.value('@CtaCxCID','varchar(50)' ) AS CtaCxCID,	
					
					PaymenttXml.value('@DivisaID','char(10)' ) AS DivisaID,	
					PaymenttXml.value('@Cambio','decimal(12, 6)' ) AS Cambio,	
					PaymenttXml.value('@Saldo','money' ) AS Saldo,	
					PaymenttXml.value('@Tipo','varchar(10)	' ) AS Tipo,	
					PaymenttXml.value('@Crédito','bit' ) AS Crédito,	
					PaymenttXml.value('@Facturado','bit' ) AS Facturado,	
					PaymenttXml.value('@DeudaID','varchar(10)' ) AS DeudaID,	
					PaymenttXml.value('@CambioDeuda','decimal(12, 6)' ) AS CambioDeuda,	
					PaymenttXml.value('@VendedorID','varchar(10)	' ) AS VendedorID,
					PaymenttXml.value('@Anulado','bit' ) AS Facturado,
					--PaymenttXml.value('@ExportadoDate', 'datetime') AS ExportadoDate,  
					NULL AS ExportadoDate,             
					PaymenttXml.value('@CreadoPor','VARCHAR(15)') AS CreadoPor,          
					PaymenttXml.value('@CreadoDate','DateTime') AS CreadoDate,
					PaymenttXml.value('@EditadoPor','VARCHAR(15)') AS EditadoPor, 
					--PaymenttXml.value('@EditadoDate','DateTime') AS EditadoDate,
					NULL AS EditadoDate, 
					PaymenttXml.value('@AnuladoPor','VARCHAR(15)') AS AnuladoPor, 
					--PaymenttXml.value('@AnuladoDate','DateTime') AS AnuladoDate,
					NULL AS AnuladoDate, 
					PaymenttXml.value('@AnuladoNota','varchar(1024)') AS AnuladoNota,
					PaymenttXml.value('@SucursalID', 'CHAR(2)') AS SucursalID,  
					PaymenttXml.value('@PCID','VARCHAR(50)') AS PCID, 
					PaymenttXml.value('@DivisiónID', 'CHAR(10)') AS DivisiónID,
					--PaymenttXml.value('@Secuencia', 'timestamp') AS Secuencia, --Cannot insert an explicit value into a timestamp column. Use INSERT with a column list to exclude the timestamp column, or insert a DEFAULT into the timestamp column.
					PaymenttXml.value('@Retenido', 'bit') AS Retenido,
					PaymenttXml.value('@ExportadoPor','varchar(15)' ) AS ExportadoPor,
					PaymenttXml.value('@Exportado','numeric(1, 0)' ) AS Exportado,
					PaymenttXml.value('@ExportadoUpdate','numeric(1, 0)' ) AS ExportadoUpdate,
					PaymenttXml.value('@ExportadoCandidate','numeric(1, 0) ') AS ExportadoCandidate,
					PaymenttXml.value('@MesArriendo','varchar(2)' ) AS MesArriendo,
					PaymenttXml.value('@ContratoID','varchar(10)' ) AS ContratoID,
					PaymenttXml.value('@Inmobiliaria','bit' ) AS Inmobiliaria
					
					FROM
                    @xml.nodes('/AbonoCab_InputDto/AbonoClienteDeudas_InputDto')AS TEMPTABLE(PaymenttXml)


					-- ACTUALIZAR SALDO
					-- Distribuir el valor del saldo  -- @codCliente: No es el RUC
					

					--Bancos kardex
					--[ID] [char](10)	
					SELECT
					PaymenttXml.value('@ID','CHAR(10)') AS ID, 
					PaymenttXml.value('@BancoID','char(10)' ) AS BancoID,	
					PaymenttXml.value('@DocumentoID','char(10)' ) AS DocumentoID,
					PaymenttXml.value('@AsientoID','char(10)' ) AS AsientoID,
					PaymenttXml.value('@SucursalID','char(2)' ) AS SucursalID,
					PaymenttXml.value('@PcID','varchar(50)' ) AS PcID,
					
					PaymenttXml.value('@DivisaID','char(10)' ) AS DivisaID,
					PaymenttXml.value('@Cambio','decimal(12, 6)' ) AS Cambio,
					PaymenttXml.value('@Fecha','datetime' ) AS Fecha,
					PaymenttXml.value('@Fecha_Banco','datetime' ) AS Fecha_Banco,
					PaymenttXml.value('@Tipo','varchar(10)' ) AS Tipo,
					PaymenttXml.value('@Detalle','varchar(100)' ) AS Detalle,    
					PaymenttXml.value('@Débito','bit' ) AS Débito,  
					PaymenttXml.value('@Valor','money' ) AS Valor,
					PaymenttXml.value('@Valor_Base','money' ) AS Valor_Base,
					PaymenttXml.value('@Conciliado','bit' ) AS Conciliado,
					--PaymenttXml.value('@ExportadoDate','datetime' ) AS ExportadoDate,
					NULL AS ExportadoDate,
					PaymenttXml.value('@CreadoPor','char(15) ' ) AS CreadoPor,
					PaymenttXml.value('@CreadoDate','datetime' ) AS CreadoDate,
					PaymenttXml.value('@Número','char(10)' ) AS Número,
					PaymenttXml.value('@Cheque','varchar(10)' ) AS Cheque,
					PaymenttXml.value('@Fecha_Cheque','datetime' ) AS Fecha_Cheque,
					PaymenttXml.value('@Beneficiario','varchar(50)' ) AS Beneficiario,
					PaymenttXml.value('@DivisiónID','char(10) ' ) AS DivisiónID,					
					PaymenttXml.value('@Anulado', 'bit') AS Anulado,
					PaymenttXml.value('@AnuladoPor','VARCHAR(15)') AS AnuladoPor, 
					--PaymenttXml.value('@AnuladoDate','DateTime') AS AnuladoDate,
					NULL AS AnuladoDate,
					PaymenttXml.value('@AnuladoNota','varchar(1024)') AS AnuladoNota,
					
					--PaymenttXml.value('@Secuencia', 'timestamp') AS Secuencia, --Cannot insert an explicit value into a timestamp column. Use INSERT with a column list to exclude the timestamp column, or insert a DEFAULT into the timestamp column.
					PaymenttXml.value('@ExportadoPor','varchar(15)' ) AS ExportadoPor,
					PaymenttXml.value('@Exportado','numeric(1, 0)' ) AS Exportado,
					PaymenttXml.value('@ExportadoUpdate','numeric(1, 0)' ) AS ExportadoUpdate,
					PaymenttXml.value('@ExportadoCandidate','numeric(1, 0) ') AS ExportadoCandidate
					
	
					FROM
                    @xml.nodes('/AbonoCab_InputDto/AbonoBanco_Kardex_InputDto')AS TEMPTABLE(PaymenttXml)
		