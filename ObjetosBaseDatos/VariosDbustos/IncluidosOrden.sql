use MINIMARKET --solo hay normal en clientes
select cupo, * from CLI_CLIENTES where id = '0000000099' --la tabla cliente tiene que clase de cliente es normal, mayorista, etc
select * from VEN_ORDENES where id = '0000000028'
select * from VEN_ORDENES_DT where ordenid = '0000000028'
select * from VEN_ORDENES_DT where ProductoID = '0000000337'


select * from INV_PRODUCTOS where id = '0000000337'
select * from INV_PRODUCTOS_EMPAQUES where ProductoID = '0000000337'
select * from INV_PRODUCTOS_precios where ProductoID = '0000000337'

--select 
select * from sis_parametros where código like 'precio%'
select * from sis_parametros where Nombre like '%contado%'
select * from sis_parametros where PadreID ='0000000014'

--select distinct clase from INV_PRODUCTOS 
--select distinct clase from CLI_CLIENTES 
--select * from inv_precios
--select * from INV_PRODUCTOS

-- select DISTINCt DivisiónID from VEN_ORDENES 
-- select DISTINCt ESTADO from VEN_ORDENES 
-- select DISTINCt VendedorID from VEN_ORDENES 


