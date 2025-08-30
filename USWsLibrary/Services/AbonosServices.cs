using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USWsLibrary.Models;
using USWsLibrary.Common;
using USWsLibrary.Services;

namespace USWsLibrary.Services
{
    public class AbonosServices
    {
        private DataModel _db = new DataModel();


        public AbonoReporteOut RecuperarImprimir(string AbonoId)
        {
            AbonoReporteOut retorno = new AbonoReporteOut();
            ProductosServices objprodsrv = new Services.ProductosServices();


            SqlParameter pAbonoId = new SqlParameter();
            pAbonoId.ParameterName = "AbonoId";
            pAbonoId.SqlDbType = System.Data.SqlDbType.VarChar;
            pAbonoId.Size = 10;
            pAbonoId.Direction = System.Data.ParameterDirection.Input;
            pAbonoId.Value = AbonoId.ToString();



            var parameters = new[] { pAbonoId };

            var query = "EXEC [dbo].[Ban_Abonos_Imprimir] @AbonoId";

            try
            {

                using (var multiResultSet = _db.MultiResultSetSqlQuery(query, parameters))
                {
                    retorno.DataSet01 = multiResultSet.ResultSetFor<AbonoReporte01Out>().ToList();
                    retorno.DataSet02 = multiResultSet.ResultSetFor<AbonoReporte02Out>().ToList();

                }


            }
            catch (SqlException ee)
            {
                throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;

        }

        //Obtener el Detalle y Número para abonos //erase not use
        public ClienteDetalleNumero_OutputDto ConsultaDetalleNumero(ClienteDetalleNumero_InputDto detalle) //Ban_kardex
        {
            //@codCliente varchar(13),
            //@IdClienteDeuda char(10)

            SqlParameter pClienteRUC = new SqlParameter();
            pClienteRUC.ParameterName = "CodCliente";
            pClienteRUC.DbType = System.Data.DbType.String;
            pClienteRUC.Size = 13;
            pClienteRUC.IsNullable = true;
            pClienteRUC.Direction = System.Data.ParameterDirection.Input;
            if (detalle.CodCliente == null)
                pClienteRUC.Value = DBNull.Value;
            else
                pClienteRUC.Value = detalle.CodCliente;



            SqlParameter pIdClienteDeuda = new SqlParameter();
            pIdClienteDeuda.ParameterName = "IdClienteDeuda";
            pIdClienteDeuda.DbType = System.Data.DbType.String;
            pIdClienteDeuda.Size = 10;
            pIdClienteDeuda.IsNullable = true;
            pIdClienteDeuda.Direction = System.Data.ParameterDirection.Input;
            if (detalle.IdClienteDeuda == null)
                pIdClienteDeuda.Value = DBNull.Value;
            else
                pIdClienteDeuda.Value = detalle.IdClienteDeuda;


            object[] parameters = new object[] { pClienteRUC, pIdClienteDeuda };


            ClienteDetalleNumero_OutputDto retorno = new ClienteDetalleNumero_OutputDto();

            try
            {
                retorno = _db.Database.SqlQuery<ClienteDetalleNumero_OutputDto>
                   ("dbo.CLI_Clientes_Select_DetalleNumero   @codCliente,  @IdClienteDeuda ", parameters).SingleOrDefault();

            }
            catch (SqlException ee)
            {
                throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;
        }


        //Obtener el next id para Bancos Kardex *
        public NextIDOutputDto NextID_Bancokardex() //Ban_kardex
        {

            SqlParameter pContador = new SqlParameter();
            pContador.ParameterName = "Contador";
            pContador.DbType = System.Data.DbType.String;
            pContador.Size = 50;
            pContador.IsNullable = true;
            pContador.Direction = System.Data.ParameterDirection.Input;
            pContador.Value = ConstGeneralCode.BAN_BANCOS_CARDEX_ID_00;

            SqlParameter pErrorCode = new SqlParameter();
            pErrorCode.ParameterName = "ErrorCode";
            pErrorCode.DbType = System.Data.DbType.String;
            pErrorCode.Direction = System.Data.ParameterDirection.Output;
            pErrorCode.Size = 256;
            pErrorCode.Value = string.Empty;

            object[] parameters = new object[] { pContador, pErrorCode };


            NextIDOutputDto retorno = new NextIDOutputDto();

            try
            {
                retorno = _db.Database.SqlQuery<NextIDOutputDto>
                   ("dbo.SIS_GetNextID   @Contador, @ErrorCode out", parameters).SingleOrDefault();

                retorno.ErrorCode = Convert.ToString(pErrorCode.Value);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;
        }


        //Obtener el next id para clientes deudas *
        public AbonoNextIDOutputDto NextID_DebtsCustomer() //debt
        {

            SqlParameter pContador = new SqlParameter();
            pContador.ParameterName = "Contador";
            pContador.DbType = System.Data.DbType.String;
            pContador.Size = 50;
            pContador.IsNullable = true;
            pContador.Direction = System.Data.ParameterDirection.Input;
            pContador.Value = ConstGeneralCode.CLI_CLIENTES_DEUDAS_ID_00;

            SqlParameter pErrorCode = new SqlParameter();
            pErrorCode.ParameterName = "ErrorCode";
            pErrorCode.DbType = System.Data.DbType.String;
            pErrorCode.Direction = System.Data.ParameterDirection.Output;
            pErrorCode.Size = 256;
            pErrorCode.Value = string.Empty;

            object[] parameters = new object[] { pContador, pErrorCode };


            AbonoNextIDOutputDto retorno = new AbonoNextIDOutputDto();

            try
            {
                retorno = _db.Database.SqlQuery<AbonoNextIDOutputDto>
                   ("dbo.SIS_GetNextID   @Contador, @ErrorCode out", parameters).SingleOrDefault();

                retorno.ErrorCode = Convert.ToString(pErrorCode.Value);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;
        }



        public AbonoCargaOut Recuperar(String PaymentId)
        {
            AbonoCargaOut retorno = new AbonoCargaOut();
            ProductosServices objprodsrv = new Services.ProductosServices();


            SqlParameter pPaymentId = new SqlParameter();
            pPaymentId.ParameterName = "PaymentId";
            pPaymentId.DbType = System.Data.DbType.String;
            pPaymentId.Direction = System.Data.ParameterDirection.Input;
            pPaymentId.Value = PaymentId;

            var parameters = new[] { pPaymentId };

            string query = "";


            query = "EXEC [dbo].[Ban_Abonos_Recuperar] @PaymentId";


            try
            {

                using (var multiResultSet = _db.MultiResultSetSqlQuery(query, parameters))
                {
                    retorno.Cab = multiResultSet.ResultSetFor<AbonoPagoCabOut>().FirstOrDefault();

                    retorno.Pagos = multiResultSet.ResultSetFor<AbonoPagoOut>().ToList();

                    retorno.Distribucion = multiResultSet.ResultSetFor<AbonoDistribucionOut>().ToList();


                }


            }
            catch (SqlException ee)
            {
                throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;

        }





        //Obtener el de linea que es ID de BAN_INGRESOS_DT *
        public NextIDOutputDto NextPaymentID()
        {

            SqlParameter pContador = new SqlParameter();
            pContador.ParameterName = "Contador";
            pContador.DbType = System.Data.DbType.String;
            pContador.Size = 50;
            pContador.IsNullable = true;
            pContador.Direction = System.Data.ParameterDirection.Input;
            pContador.Value = ConstGeneralCode.BAN_INGRESOS_DT_ID_00;

            SqlParameter pErrorCode = new SqlParameter();
            pErrorCode.ParameterName = "ErrorCode";
            pErrorCode.DbType = System.Data.DbType.String;
            pErrorCode.Direction = System.Data.ParameterDirection.Output;
            pErrorCode.Size = 256;
            pErrorCode.Value = string.Empty;

            object[] parameters = new object[] { pContador, pErrorCode };


            NextIDOutputDto retorno = new NextIDOutputDto();

            try
            {
                retorno = _db.Database.SqlQuery<NextIDOutputDto>
                   ("dbo.SIS_GetNextID   @Contador, @ErrorCode out", parameters).SingleOrDefault();

                retorno.ErrorCode = Convert.ToString(pErrorCode.Value);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;
        }


        //Obtener los ids de deudorID *
        public Code_DeudorID_OutPutDto ConsultarCodigoDeudorID(string DeudorID) //
        {
            SqlParameter pDeudorID = new SqlParameter();
            pDeudorID.ParameterName = "DeudorID";
            pDeudorID.DbType = System.Data.DbType.String;
            pDeudorID.Size = 10;
            pDeudorID.IsNullable = true;
            pDeudorID.Direction = System.Data.ParameterDirection.Input;
            pDeudorID.Value = DeudorID;


            SqlParameter pErrorCode = new SqlParameter();
            pErrorCode.ParameterName = "ErrorCode";
            pErrorCode.DbType = System.Data.DbType.String;
            pErrorCode.Direction = System.Data.ParameterDirection.Output;
            pErrorCode.Size = 256;
            pErrorCode.Value = string.Empty;

            object[] parameters = new object[] { pDeudorID, pErrorCode };


            Code_DeudorID_OutPutDto retorno = new Code_DeudorID_OutPutDto();

            try
            {
                retorno = _db.Database.SqlQuery<Code_DeudorID_OutPutDto>
                   ("dbo.BAN_GetCode_DeudorID   @DeudorID,  @ErrorCode out", parameters).SingleOrDefault();

                retorno.ErrorCode = Convert.ToString(pErrorCode.Value);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;
        }




        public SimpleReturn Grabar(AbonoCabDto abono)
        {


            //xml
            SqlParameter pxml = new SqlParameter();
            pxml.ParameterName = "xml";
            pxml.DbType = System.Data.DbType.Xml;
            pxml.Direction = System.Data.ParameterDirection.Input;
            pxml.Value = HelperConvert<AbonoCabDto>.E2Xml(abono);

            object[] parameters = new object[] { pxml };

            SimpleReturn retorno = new SimpleReturn();

            try
            {
                retorno.ExistError = false;
                retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                    ("dbo.BAN_ABONOS_GRABAR_GRAL  @xml ", parameters).FirstOrDefault();

                if (retorno.ErrorIfExist.ErrorNumber != 0)
                    retorno.ExistError = true;


            }
            catch (SqlException ee)
            {
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 99999;
                retorno.ErrorIfExist.ErrorMessage = "Hubo un error al grabar el documento";
                return retorno;
            }
            catch (Exception ee)
            {
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 99999;
                retorno.ErrorIfExist.ErrorMessage = "Hubo un error al grabar el documento";
                return retorno;
            }

            return retorno;
        }

        public SimpleReturn GrabarOffline(AbonoCabDto abono)
        {

            SimpleReturn retorno = new SimpleReturn();
            //xml
            SqlParameter pxml = new SqlParameter();
            pxml.ParameterName = "xml";
            pxml.DbType = System.Data.DbType.Xml;
            pxml.Direction = System.Data.ParameterDirection.Input;
            pxml.Value = HelperConvert<AbonoCabDto>.E2Xml(abono);


            SqlParameter pretorno = new SqlParameter();
            pretorno.ParameterName = "IDRetorno";
            pretorno.DbType = System.Data.DbType.String;
            pretorno.Direction = System.Data.ParameterDirection.Output;
            pretorno.Size = 10;
            pretorno.Value = string.Empty;

            object[] parameters = new object[] { pxml, pretorno };



            try
            {
                retorno.ExistError = false;

                if (!abono.EsPosfechado)
                    retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                        ("dbo.OFFLINE_BAN_Abonos_Grabar_Gral  @xml, @IDRetorno out", parameters).FirstOrDefault();
                else
                    retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                            ("dbo.OFFLINE_CLI_Recibos_Grabar_Gral  @xml, @IDRetorno out", parameters).FirstOrDefault();


                if (pretorno.Value != null)
                    retorno.DocumentId = pretorno.Value.ToString();

                if (retorno.ErrorIfExist.ErrorNumber != 0)                
                    retorno.ExistError = true;


            }
            catch (SqlException ee)
            {
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 99999;
                retorno.ErrorIfExist.ErrorMessage = "Hubo un error al grabar el documento";
                return retorno;
            }
            catch (Exception ee)
            {
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 99999;
                retorno.ErrorIfExist.ErrorMessage = "Hubo un error al grabar el documento";
                return retorno;
            }

            return retorno;
        }



        public PagedList<AbonoMinOut> ListaXFiltro(AbonoFiltroInp Filtro)
        {
            PagedList<AbonoMinOut> retorno = new PagedList<AbonoMinOut>();
            ProductosServices objprodsrv = new Services.ProductosServices();

            SqlParameter pPageNumber = new SqlParameter();
            pPageNumber.ParameterName = "PageNumber";
            pPageNumber.DbType = System.Data.DbType.Int32;
            pPageNumber.Direction = System.Data.ParameterDirection.Input;
            pPageNumber.Size = 4;
            pPageNumber.Value = Filtro.PageNumber;

            SqlParameter pPageSize = new SqlParameter();
            pPageSize.ParameterName = "PageSize";
            pPageSize.DbType = System.Data.DbType.Int32;
            pPageSize.Direction = System.Data.ParameterDirection.Input;
            pPageSize.Size = 4;
            pPageSize.Value = Filtro.PageSize;


            SqlParameter pClienteNombre = new SqlParameter();
            pClienteNombre.ParameterName = "ClienteNombre";
            pClienteNombre.DbType = System.Data.DbType.String;
            pClienteNombre.Size = 50;
            pClienteNombre.Direction = System.Data.ParameterDirection.Input;


            if (Filtro.ClienteNombre == null)
                pClienteNombre.Value = DBNull.Value;
            else
                pClienteNombre.Value = Filtro.ClienteNombre;


            SqlParameter pVendedorUserName = new SqlParameter();
            pVendedorUserName.ParameterName = "VendedorUserName";
            pVendedorUserName.DbType = System.Data.DbType.String;
            pVendedorUserName.Size = 25;
            pVendedorUserName.Direction = System.Data.ParameterDirection.Input;
            pVendedorUserName.Value = Filtro.VendedorUsername;


            SqlParameter pDias = new SqlParameter();
            pDias.ParameterName = "Dias";
            pDias.DbType = System.Data.DbType.Int32;
            pDias.Size = 4;
            pDias.Direction = System.Data.ParameterDirection.Input;
            pDias.Value = Filtro.Dias;


            SqlParameter pCntRegistros = new SqlParameter();
            pCntRegistros.ParameterName = "CntRegistros";
            pCntRegistros.DbType = System.Data.DbType.Int32;
            pCntRegistros.Direction = System.Data.ParameterDirection.Output;
            pCntRegistros.Size = 4;
            pCntRegistros.Value = 0;


            object[] parameters = new object[] { pPageNumber, pPageSize, pClienteNombre, pVendedorUserName, pDias, pCntRegistros };

            try
            {
                retorno.Results = _db.Database.SqlQuery<AbonoMinOut>
                    ("dbo.BAN_Abonos_Recuperar_Lista @PageNumber,@PageSize,@ClienteNombre,@VendedorUserName, @Dias,  @CntRegistros out ", parameters).ToList();

                retorno.Count = Convert.ToInt32(pCntRegistros.Value);

            }
            catch (SqlException ee)
            {
                throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;

        }




    }
}