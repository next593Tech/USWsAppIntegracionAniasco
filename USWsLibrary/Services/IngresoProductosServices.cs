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
    public class IngresoProductosServices
    {
        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public IngresoProductosServices()
        {
            _db = new DataModel();

        }

        #endregion



        public NextIDOutputDto NextID()
        {
            SqlParameter pContador = new SqlParameter();
            pContador.ParameterName = "Contador";
            pContador.DbType = System.Data.DbType.String;
            pContador.Size = 50;
            pContador.IsNullable = true;
            pContador.Direction = System.Data.ParameterDirection.Input;
            pContador.Value = ConstGeneralCode.FacturaID;

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

            }
            catch (Exception ee)
            {

            }

            return retorno;
        }

        public SimpleReturn SaveOffProductos(INV_INGRESOS_DTO producto)
        {
            SimpleReturn retorno = new SimpleReturn();
                              
            
            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<INV_INGRESOS_DTO>.E2Xml(producto);


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
                    ("dbo.OFFLINE_INV_INGRESOS_Grabar_GralProductos @infoXml ,@IDRetorno out", parameters).FirstOrDefault();

                
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
         

    }
}