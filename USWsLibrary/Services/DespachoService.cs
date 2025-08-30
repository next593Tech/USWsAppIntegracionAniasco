using System;
using System.Data.SqlClient;
using System.Linq;
using USWsLibrary.Common;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{

    public class DespachoService
    {
        private DataModel _DB;

        public DespachoService()
        {
            _DB = new DataModel();
        }
        public PagedList<VEN_ORDENESDESPACHO_DTO> GetAllOrdenesAsync(GenericFilterInp filter)
        {
            PagedList<VEN_ORDENESDESPACHO_DTO> retorno = new PagedList<VEN_ORDENESDESPACHO_DTO>();
            string sp = "dbo.OFFLINE_VEN_ORD_spGetAllOrdenes @PageNumber,@PageSize,@UserName, @Count out";
            SqlParameter pPageNumber = new SqlParameter();
            pPageNumber.ParameterName = "PageNumber";
            pPageNumber.DbType = System.Data.DbType.Int32;
            pPageNumber.Direction = System.Data.ParameterDirection.Input;
            pPageNumber.Size = 4;
            pPageNumber.Value = filter.PageNumber;

            SqlParameter pPageSize = new SqlParameter();
            pPageSize.ParameterName = "PageSize";
            pPageSize.DbType = System.Data.DbType.Int32;
            pPageSize.Direction = System.Data.ParameterDirection.Input;
            pPageSize.Size = 4;
            pPageSize.Value = filter.PageSize;

            SqlParameter pUserName = new SqlParameter();
            pUserName.ParameterName = "UserName";
            pUserName.DbType = System.Data.DbType.String;
            pUserName.Size = 13;
            pUserName.IsNullable = true;
            pUserName.Direction = System.Data.ParameterDirection.Input;
            if (filter.genericstring == null)
                pUserName.Value = DBNull.Value;
            else
                pUserName.Value = filter.genericstring;


            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "Count";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;
            object[] parameters = new object[] { pPageNumber, pPageSize, pUserName, pTotalSalida };
            try
            {
                retorno.Results = _DB.Database.SqlQuery<VEN_ORDENESDESPACHO_DTO>(sp, parameters).ToList();
                retorno.Count = Convert.ToInt32(pTotalSalida.Value);
            }
            catch (SqlException ee)
            {

            }
            catch (System.Exception eee)
            {

            }
            return retorno;

        }

        public PagedList<VEN_ENTREGAS_MIN_DTO> GetAllEntregasAsync(GenericFilterInp filter)
        {
            PagedList<VEN_ENTREGAS_MIN_DTO> retorno = new PagedList<VEN_ENTREGAS_MIN_DTO>();
            string sp = "dbo.OFFLINE_VEN_ENT_GetAllEntregas @PageNumber,@PageSize,@UserName, @Count out";
            SqlParameter pPageNumber = new SqlParameter();
            pPageNumber.ParameterName = "PageNumber";
            pPageNumber.DbType = System.Data.DbType.Int32;
            pPageNumber.Direction = System.Data.ParameterDirection.Input;
            pPageNumber.Size = 4;
            pPageNumber.Value = filter.PageNumber;

            SqlParameter pPageSize = new SqlParameter();
            pPageSize.ParameterName = "PageSize";
            pPageSize.DbType = System.Data.DbType.Int32;
            pPageSize.Direction = System.Data.ParameterDirection.Input;
            pPageSize.Size = 4;
            pPageSize.Value = filter.PageSize;


            SqlParameter pUserName = new SqlParameter();
            pUserName.ParameterName = "UserName";
            pUserName.DbType = System.Data.DbType.String;
            pUserName.Size = 13;
            pUserName.IsNullable = true;
            pUserName.Direction = System.Data.ParameterDirection.Input;
            if (filter.genericstring == null)
                pUserName.Value = DBNull.Value;
            else
                pUserName.Value = filter.genericstring;


            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "Count";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;
            object[] parameters = new object[] { pPageNumber, pPageSize, pUserName, pTotalSalida };
            try
            {
                retorno.Results = _DB.Database.SqlQuery<VEN_ENTREGAS_MIN_DTO>(sp, parameters).ToList();
                retorno.Count = Convert.ToInt32(pTotalSalida.Value);
            }
            catch (SqlException ee)
            {

            }
            catch (System.Exception eee)
            {

            }
            return retorno;

        }

        public PagedList<VEN_ENTREGAS_MIN_DT_DTO> GetAllEntregasDtAsync(GenericFilterInp filter)
        {
            PagedList<VEN_ENTREGAS_MIN_DT_DTO> retorno = new PagedList<VEN_ENTREGAS_MIN_DT_DTO>();
            string sp = "dbo.OFFLINE_VEN_ENT_spGetAllEntregasDT @PageNumber,@PageSize,@UserName, @Count out";
            SqlParameter pPageNumber = new SqlParameter();
            pPageNumber.ParameterName = "PageNumber";
            pPageNumber.DbType = System.Data.DbType.Int32;
            pPageNumber.Direction = System.Data.ParameterDirection.Input;
            pPageNumber.Size = 4;
            pPageNumber.Value = filter.PageNumber;


            SqlParameter pPageSize = new SqlParameter();
            pPageSize.ParameterName = "PageSize";
            pPageSize.DbType = System.Data.DbType.Int32;
            pPageSize.Direction = System.Data.ParameterDirection.Input;
            pPageSize.Size = 4;
            pPageSize.Value = filter.PageSize;


            SqlParameter pUserName = new SqlParameter();
            pUserName.ParameterName = "UserName";
            pUserName.DbType = System.Data.DbType.String;
            pUserName.Size = 13;
            pUserName.IsNullable = true;
            pUserName.Direction = System.Data.ParameterDirection.Input;
            if (filter.genericstring == null)
                pUserName.Value = DBNull.Value;
            else
                pUserName.Value = filter.genericstring;



            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "Count";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;
            object[] parameters = new object[] { pPageNumber, pPageSize, pUserName, pTotalSalida };
            try
            {
                retorno.Results = _DB.Database.SqlQuery<VEN_ENTREGAS_MIN_DT_DTO>(sp, parameters).ToList();
                retorno.Count = Convert.ToInt32(pTotalSalida.Value);
            }
            catch (SqlException ee)
            {

            }
            catch (System.Exception eee)
            {

            }
            return retorno;

        }

        public PagedList<VEN_ORDENESDESPACHO_DT_DTO> GetAllOrdenesDtAsync(GenericFilterInp filter)
        {
            PagedList<VEN_ORDENESDESPACHO_DT_DTO> retorno = new PagedList<VEN_ORDENESDESPACHO_DT_DTO>();
            string sp = "dbo.OFFLINE_VEN_ORD_spGetAllOrdenesDT @PageNumber,@PageSize,@UserName,@Count out";
            SqlParameter pPageNumber = new SqlParameter();
            pPageNumber.ParameterName = "PageNumber";
            pPageNumber.DbType = System.Data.DbType.Int32;
            pPageNumber.Direction = System.Data.ParameterDirection.Input;
            pPageNumber.Size = 4;
            pPageNumber.Value = filter.PageNumber;

            SqlParameter pPageSize = new SqlParameter();
            pPageSize.ParameterName = "PageSize";
            pPageSize.DbType = System.Data.DbType.Int32;
            pPageSize.Direction = System.Data.ParameterDirection.Input;
            pPageSize.Size = 4;
            pPageSize.Value = filter.PageSize;


            SqlParameter pUserName = new SqlParameter();
            pUserName.ParameterName = "UserName";
            pUserName.DbType = System.Data.DbType.String;
            pUserName.Size = 13;
            pUserName.IsNullable = true;
            pUserName.Direction = System.Data.ParameterDirection.Input;
            if (filter.genericstring == null)
                pUserName.Value = DBNull.Value;
            else
                pUserName.Value = filter.genericstring;


            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "Count";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;


            object[] parameters = new object[] { pPageNumber, pPageSize, pUserName, pTotalSalida };
            try
            {
                retorno.Results = _DB.Database.SqlQuery<VEN_ORDENESDESPACHO_DT_DTO>(sp, parameters).ToList();
                retorno.Count = Convert.ToInt32(pTotalSalida.Value);
            }
            catch (SqlException ee)
            {

            }
            catch (System.Exception eee)
            {

            }
            return retorno;

        }
        public SimpleReturn InsertEntregasOrdenes(VEN_ORD_ENT_DTO input)
        {
            SimpleReturn retorno = new SimpleReturn();



            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<VEN_ORD_ENT_DTO>.E2Xml(input);

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
                retorno.ErrorIfExist = _DB.Database.SqlQuery<ErrorItem>
                    ("dbo.OFFLINE_VEN_SpInsertEntrega  @infoXml,@IDRetorno out ", parameters).FirstOrDefault();

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

        public SimpleReturn UpdateDeliveryStatus(VEN_ENTREGAS_UPDATE_DTO input)
        {
            SimpleReturn retorno = new SimpleReturn();

            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<VEN_ENTREGAS_UPDATE_DTO>.E2Xml(input);

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
                retorno.ErrorIfExist = _DB.Database.SqlQuery<ErrorItem>
                    ("dbo.OFFLINE_VEN_ENT_UPDATEDELIVERYSTATE  @infoXml,@IDRetorno out ", parameters).FirstOrDefault();
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



        public SimpleReturn UpdateDateDelivery(VEN_ORDENESDESPACHO_UPDATE_DTO input)
        {
            SimpleReturn retorno = new SimpleReturn();

            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<VEN_ORDENESDESPACHO_UPDATE_DTO>.E2Xml(input);

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
                retorno.ErrorIfExist = _DB.Database.SqlQuery<ErrorItem>
                    ("dbo.[OFFLINE_VEN_ENT_UPDATEDELIVERYSTATEDATE]  @infoXml,@IDRetorno out ", parameters).FirstOrDefault();
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

        public PagedList<VEN_ORDENES_DESPACHO2> getOrdersPending(){

            PagedList<VEN_ORDENES_DESPACHO2> retorno = new PagedList<VEN_ORDENES_DESPACHO2>();
            string sp = "dbo.OFFLINE_VEN_ORD_GETPENDING @TotalSalida out";
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;

            object[] parameters = new object[] { pTotalSalida };
            try
            {
                retorno.Results = _DB.Database.SqlQuery<VEN_ORDENES_DESPACHO2>(sp, parameters).ToList();
                retorno.Count = Convert.ToInt32(pTotalSalida.Value);
            }
            catch (SqlException ee)
            {

            }
            catch (System.Exception eee)
            {

            }
            return retorno;
        }

        public PagedList<VEN_ORDENES_DESPACHO2DT> getOrdersPendingDT(GenericFilterInp input)
		{
            PagedList<VEN_ORDENES_DESPACHO2DT> retorno = new PagedList<VEN_ORDENES_DESPACHO2DT>();
            string sp = "OFFLINE_VEN_ORD_GETPENDINGDT @TotalSalida out, @OrderID";
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;

            SqlParameter pOrderID = new SqlParameter();
            pOrderID.ParameterName = "OrderID";
            pOrderID.DbType = System.Data.DbType.String;
            pOrderID.Direction = System.Data.ParameterDirection.Input;
            pOrderID.Size = 10;
            pOrderID.Value = input.genericstring;
            object[] parameters = new object[] { pTotalSalida,pOrderID };
            try
            {
                retorno.Results = _DB.Database.SqlQuery<VEN_ORDENES_DESPACHO2DT>(sp, parameters).ToList();
                retorno.Count = Convert.ToInt32(pTotalSalida.Value);
            }
            catch (SqlException ee)
            {

            }
            catch (System.Exception eee)
            {

            }
            return retorno;

        }
    }
}