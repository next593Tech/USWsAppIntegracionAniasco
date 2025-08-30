using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using USWsLibrary.Common;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{
    public class OrdenesServices
    {
        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public OrdenesServices()
        {
            _db = new DataModel();

        }
         
        #endregion

        #region Operaciones
        //Obtener el next id para la orden


        public NextIDOutputDto NextID()
        {
            SqlParameter pContador = new SqlParameter();
            pContador.ParameterName = "Contador";
            pContador.DbType = System.Data.DbType.String;
            pContador.Size = 50;
            pContador.IsNullable = true;
            pContador.Direction = System.Data.ParameterDirection.Input;
            pContador.Value = ConstGeneralCode.OrdenID;

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


        public SimpleReturn Grabar(VEN_ORDENES_DTO Orden)
        {
            SimpleReturn retorno = new SimpleReturn();


            var totaldetalles = TotalizaDetalles(Orden);


            //if (!ValidaTotales(Orden))


            //if (totaldetalles != Orden.Total)
            //{
            //    retorno.ExistError = true;
            //    retorno.ErrorIfExist = new ErrorItem();
            //    retorno.ErrorIfExist.ErrorNumber = 99999;
            //    retorno.ErrorIfExist.ErrorMessage = "Hay inconsistencias en la operación: Total=" + Orden.Total.ToString() + " " + " SumDet= " + totaldetalles.ToString();
            //    return retorno;
            //}


            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<VEN_ORDENES_DTO>.E2Xml(Orden);


            object[] parameters = new object[] { pXml };

            try
            {
                retorno.ExistError = false;
                retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                    ("dbo.FAC_Grabar  @infoXml  ", parameters).FirstOrDefault();

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


        public SimpleReturn GrabarOff(VEN_ORDENES_DTO Orden)
        {
            SimpleReturn retorno = new SimpleReturn();

            var totaldetalles = TotalizaDetalles(Orden);

            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<VEN_ORDENES_DTO>.E2Xml(Orden);

            SqlParameter pIdRet = new SqlParameter();
            pIdRet.ParameterName = "IDRetorno";
            pIdRet.DbType = System.Data.DbType.String;
            pIdRet.Direction = System.Data.ParameterDirection.Output;
            pIdRet.Size = 10;
            pIdRet.Value = string.Empty;


            object[] parameters = new object[] { pXml, pIdRet };

            try
            {
                retorno.ExistError = false;
                retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                    ("dbo.OFFLINE_VEN_Ordenes_Grabar  @infoXml,@IDRetorno out ", parameters).FirstOrDefault();

                if (pIdRet.Value != null)
                    retorno.DocumentId = pIdRet.Value.ToString();

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


        public VEN_ORDENES_DTO Recuperar(String OrdenId, int OptionId)
        {
            VEN_ORDENES_DTO retorno = new VEN_ORDENES_DTO();
            ProductosServices objprodsrv = new Services.ProductosServices();


            SqlParameter pOrdenId = new SqlParameter();
            pOrdenId.ParameterName = "OrdenId";
            pOrdenId.DbType = System.Data.DbType.String;
            pOrdenId.Direction = System.Data.ParameterDirection.Input;
            pOrdenId.Value = OrdenId;

            var parameters = new[] { pOrdenId };

            string query = "";

            if (OptionId == 1)
                query = "EXEC [dbo].[VEN_ORDENES_RECUPERAR] @OrdenId";
            else
                query = "EXEC [dbo].[VEN_Ordenes_Clonar] @OrdenId";

            try
            {

                using (var multiResultSet = _db.MultiResultSetSqlQuery(query, parameters))
                {
                    retorno = multiResultSet.ResultSetFor<VEN_ORDENES_DTO>().FirstOrDefault();

                    retorno.LIST_VEN_ORDENES_DT = multiResultSet.ResultSetFor<VEN_ORDENES_DT_DTO>().ToList();

                    foreach (var item in retorno.LIST_VEN_ORDENES_DT)
                    {
                       // item.RANGO_PRECIOS = objprodsrv.ListaPrecios(new Models.FiltroProdPrecioDto() { ProductoID = item.ProductoID, ClienteId = retorno.ClienteRUC, Empaque = item.Empaque });

                    }

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



        public List<VEN_ORDENES_DT_DTO_PRINT> RecuperarImprimir(String OrdenId)
        {
            List<VEN_ORDENES_DT_DTO_PRINT> retorno = new List<VEN_ORDENES_DT_DTO_PRINT>();
            ProductosServices objprodsrv = new Services.ProductosServices();


            SqlParameter pOrdenId = new SqlParameter();
            pOrdenId.ParameterName = "OrdenId";
            pOrdenId.DbType = System.Data.DbType.String;
            pOrdenId.Direction = System.Data.ParameterDirection.Input;
            pOrdenId.Value = OrdenId;

            var parameters = new[] { pOrdenId };

            var query = "EXEC [dbo].[Ven_Ordenes_Imprimir] @OrdenId";

            try
            {

                using (var multiResultSet = _db.MultiResultSetSqlQuery(query, parameters))
                {
                    retorno = multiResultSet.ResultSetFor<VEN_ORDENES_DT_DTO_PRINT>().ToList();


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



        public ErrorItem Eliminar(String OrdenId)
        {
            ErrorItem retorno = new ErrorItem();


            SqlParameter pOrdenId = new SqlParameter();
            pOrdenId.ParameterName = "OrdenId";
            pOrdenId.DbType = System.Data.DbType.String;
            pOrdenId.Direction = System.Data.ParameterDirection.Input;
            pOrdenId.Value = OrdenId;

            object[] parameters = new object[] { pOrdenId };

            try
            {
                retorno = _db.Database.SqlQuery<ErrorItem>
                    ("dbo.VEN_ORDENES_ELIMINAR  @OrdenId  ", parameters).FirstOrDefault();

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


        public PagedList<VEN_ORDENES_MIN_DTO> ListaXFiltro(FiltroPedido Filtro)
        {
            PagedList<VEN_ORDENES_MIN_DTO> retorno = new PagedList<VEN_ORDENES_MIN_DTO>();
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


            SqlParameter pEstado = new SqlParameter();
            pEstado.ParameterName = "EstadoId";
            pEstado.DbType = System.Data.DbType.String;
            pEstado.Size = 10;
            pEstado.Direction = System.Data.ParameterDirection.Input;
            pEstado.Value = Filtro.EstadoId;


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
            pVendedorUserName.Value = Filtro.VendedorUserName;


            SqlParameter pClienteCedRuc = new SqlParameter();
            pClienteCedRuc.ParameterName = "ClienteCedulaRuc";
            pClienteCedRuc.DbType = System.Data.DbType.String;
            pClienteCedRuc.Size = 25;
            pClienteCedRuc.Direction = System.Data.ParameterDirection.Input;

            if (Filtro.ClienteId == null)
                pClienteCedRuc.Value = DBNull.Value;
            else
                pClienteCedRuc.Value = Filtro.ClienteId;



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


            object[] parameters = new object[] { pPageNumber, pPageSize, pEstado, pClienteNombre, pClienteCedRuc, pVendedorUserName, pDias, pCntRegistros };

            try
            {
                retorno.Results = _db.Database.SqlQuery<VEN_ORDENES_MIN_DTO>
                    ("dbo.VEN_ORDENES_RECUPERAR_LISTA @PageNumber,@PageSize,@EstadoId,@ClienteNombre,@ClienteCedulaRuc,@VendedorUserName, @Dias,  @CntRegistros out ", parameters).ToList();

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







        // VEN_OPCIONES


        public PagedList<VEN_OPCIONES> ObtenerVenOpciones(GenericFilterInp Filtro)
        {
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



            //@Total parametro de Salida
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;

            object[] parameters = new object[] { pPageNumber, pPageSize, pTotalSalida };

            PagedList<VEN_OPCIONES> retorno = new PagedList<VEN_OPCIONES>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<VEN_OPCIONES>
                  ("dbo.VEN_Opciones_Listado @PageNumber,@PageSize, @TotalSalida out", parameters).ToList();

                retorno.Count = Convert.ToInt32(pTotalSalida.Value);

            }
            catch (SqlException ee)
            {
                //throw ee;
            }
            catch (Exception ee)
            {
                string mensaje = ee.Message;
            }

            return retorno;



        }
































        #endregion


        #region Validaciones

        private bool ValidaTotales(VEN_ORDENES_DTO Orden)
        {

            var totaldetalles = TotalizaDetalles(Orden);

            if (totaldetalles != Orden.Total) return false;

            return true;
        }

        private decimal TotalizaDetalles(VEN_ORDENES_DTO orden)
        {
            decimal retorno = 0M;
            foreach (var item in orden.LIST_VEN_ORDENES_DT)
            {


                retorno = retorno + item.Cantidad * item.Precio * item.Factor * (1 - item.TasaDescuento / 100) + item.Impuesto + item.ImpuestoIce + item.ImpuestoVerde;



            }


            return retorno;
        }

        #endregion
    }
}