using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USWsLibrary.Common;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{
    public class DevolucionesServices
    {
        private DataModel _db;

        public DevolucionesServices()
        {
            _db = new DataModel();
        }


        public CompositeReturn<List<FAC_DEV_PRODUCTO_DT>> GetDevProductsDet(DevProductsFilter filter)
        {
            SqlParameter pSecuencia = new SqlParameter();
            pSecuencia.ParameterName = "Secuencia";
            pSecuencia.DbType = System.Data.DbType.String;
            pSecuencia.Direction = System.Data.ParameterDirection.Input;
            pSecuencia.Size = 20;
            pSecuencia.IsNullable = true;

            if (filter.Secuencia == null)
                pSecuencia.Value = DBNull.Value;
            else
                pSecuencia.Value = filter.Secuencia;

            //COLLECCIÓN DE OBJETOS DE PARAMETROS
            object[] parameters = new object[] { pSecuencia };

            //INICIALIZACIÓN DEL RETORNO
            CompositeReturn<List<FAC_DEV_PRODUCTO_DT>> retorno = new CompositeReturn<List<FAC_DEV_PRODUCTO_DT>>();

            try
            {
                var ret = _db.Database.SqlQuery<FAC_DEV_PRODUCTO_DT>("[dbo].[ONLINE_FAC_GetDevProductsDt] @Secuencia", parameters).ToList();

                retorno.CompositeObjet = ret;
                retorno.ExistError = false;
            }
            catch (SqlException ee)
            {
                retorno.CompositeObjet = new List<FAC_DEV_PRODUCTO_DT>();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 99999;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
            }
            catch (Exception ee)
            {
                retorno.CompositeObjet = new List<FAC_DEV_PRODUCTO_DT>();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 99999;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
                return retorno;
            }

            return retorno;
        }

        public CompositeReturn<FAC_DEV_PRODUCTO_CB> GetDevProductsCab(DevProductsFilter filter)
        {
            SqlParameter pSecuencia = new SqlParameter();
            pSecuencia.ParameterName = "Secuencia";
            pSecuencia.DbType = System.Data.DbType.String;
            pSecuencia.Direction = System.Data.ParameterDirection.Input;
            pSecuencia.Size = 20;
            pSecuencia.IsNullable = true;

            if (filter.Secuencia == null)
                pSecuencia.Value = DBNull.Value;
            else
                pSecuencia.Value = filter.Secuencia;

            //COLLECCIÓN DE OBJETOS DE PARAMETROS
            object[] parameters = new object[] { pSecuencia };

            //INICIALIZACIÓN DEL RETORNO
            CompositeReturn<FAC_DEV_PRODUCTO_CB> retorno = new CompositeReturn<FAC_DEV_PRODUCTO_CB>();

            try
            {
                var ret = _db.Database.SqlQuery<FAC_DEV_PRODUCTO_CB>("[dbo].[ONLINE_FAC_GetDevProductsCab] @Secuencia", parameters).FirstOrDefault();

                retorno.CompositeObjet = ret;
                retorno.ExistError = false;
            }
            catch (SqlException ee)
            {
                retorno.CompositeObjet = new FAC_DEV_PRODUCTO_CB();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 99999;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
            }
            catch (Exception ee)
            {
                retorno.CompositeObjet = new FAC_DEV_PRODUCTO_CB();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 99999;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
                return retorno;
            }

            return retorno;
        }


        public SimpleReturn Grabar_CliCreditos(CLI_CREDITOS_DTO filter)
        {
            SimpleReturn retorno = new SimpleReturn();

            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<CLI_CREDITOS_DTO>.E2Xml(filter);

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
                                          ("dbo.OFFLINE_CLI_Creditos_Grabar_Gral  @infoXml, @IDRetorno out ", parameters).FirstOrDefault();

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

        public CompositeReturn<FAC_DEV_PRODUCTO_DTO> UpdateFacturaDevuelto(UpdateDevueltoFilter filter)
        {
            CompositeReturn<FAC_DEV_PRODUCTO_DTO> retorno = new CompositeReturn<FAC_DEV_PRODUCTO_DTO>();

            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<UpdateDevueltoFilter>.E2Xml(filter);

            object[] parameters = new object[] { pXml };

            try
            {
                retorno.ExistError = false;

                //retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>("dbo.ONLINE_FAC_UpdateDevuelto  @infoXml ", parameters).FirstOrDefault();

                var ret = _db.Database.SqlQuery<FAC_DEV_PRODUCTO_DTO>("dbo.ONLINE_FAC_UpdateDevuelto  @infoXml ", parameters).FirstOrDefault();

                retorno.CompositeObjet = ret;

                if (retorno.ErrorIfExist.ErrorNumber != 0)
                    retorno.ExistError = true;

            }
            catch (SqlException ee)
            {
               
                retorno.CompositeObjet = new FAC_DEV_PRODUCTO_DTO();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = ee.ErrorCode;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
            }
            catch (Exception ee)
            { 
                retorno.CompositeObjet = new FAC_DEV_PRODUCTO_DTO(); ;
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 999999;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
            }

            return retorno;
        }

        public PagedList<Croquis> GetCroquis()
        {
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 10;
            pTotalSalida.Value = 0;

            PagedList<Croquis> retorno = new PagedList<Croquis>();

            object[] parameters = new object[] { pTotalSalida};

            try
            {
                
                retorno.Results = _db.Database.SqlQuery<Croquis>
                    ("dbo.OFFLINE_VEN_Croquis @TotalSalida out", parameters).ToList();

                retorno.Count = Convert.ToInt32(pTotalSalida.Value);

            }catch(SqlException e)
            {

            }catch(Exception ee)
            {

            }
            return retorno;

        }

        public PagedList<VEN_FACTURA> GetFactura(GenericFilterInp filter)
        {
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 10;
            pTotalSalida.Value = 0;



            SqlParameter pCroquisID = new SqlParameter();
            pCroquisID.ParameterName = "CroquisID";
            pCroquisID.DbType = System.Data.DbType.String;
            pCroquisID.Size = 10;
            pCroquisID.Value = filter.genericstring;

            PagedList<VEN_FACTURA> retorno = new PagedList<VEN_FACTURA>();

            object[] parameters = new object[] {pCroquisID, pTotalSalida };

            try { 

                  retorno.Results = _db.Database.SqlQuery<VEN_FACTURA>
                    ("dbo.OFFLINE_VEN_Fact_x_Croquis @CroquisID, @TotalSalida out", parameters).ToList();
                    retorno.Count = Convert.ToInt32(pTotalSalida.Value);
            }catch(SqlException e)
            {

            }catch(Exception ee)
            {

            }

            return retorno;
        }

        public PagedList<VEN_FACTURA_DT>GetFacturaDt(GenericFilterInp filter)
        {
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 10;
            pTotalSalida.Value = 0;


            SqlParameter pFactID = new SqlParameter();
            pFactID.ParameterName = "FactID";
            pFactID.DbType = System.Data.DbType.String;
            pFactID.Size = 10;
            pFactID.Value = filter.genericstring;



            PagedList<VEN_FACTURA_DT> retorno = new PagedList<VEN_FACTURA_DT>();

            object[] parameters = new object[] { pFactID, pTotalSalida };

            try
            {

                retorno.Results = _db.Database.SqlQuery<VEN_FACTURA_DT>
                  ("dbo.OFFLINE_VEN_FactDT_x_FactID @FactID, @TotalSalida out", parameters).ToList();
                retorno.Count = Convert.ToInt32(pTotalSalida.Value);
            }
            catch (SqlException e)
            {

            }
            catch (Exception ee)
            {

            }

            return retorno;



        }


        public SimpleReturn UpdateFacturaDT(VEN_FACTURA_DT_LIST Input)
        {
            SimpleReturn retorno = new SimpleReturn();

            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<VEN_FACTURA_DT_LIST>.E2Xml(Input);




            object[] parameters = new object[] { pXml };



            try
            {
                retorno.ExistError = false;
                retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                    ("dbo.OFFLINE_VEN_FACTURAS_DEVOLUCIONES  @infoXml ", parameters).FirstOrDefault();


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

    }
}