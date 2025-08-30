using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USWsLibrary.Common;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{
    public class CajasServices
    {
        private DataModel _db = new DataModel();

        public SimpleReturn GrabarOffline(CIERRECAJA_DTO caja)
        {

            SimpleReturn retorno = new SimpleReturn();
            //xml
            SqlParameter pxml = new SqlParameter();
            pxml.ParameterName = "xml";
            pxml.DbType = System.Data.DbType.Xml;
            pxml.Direction = System.Data.ParameterDirection.Input;
            pxml.Value = HelperConvert<CIERRECAJA_DTO>.E2Xml(caja);



            object[] parameters = new object[] { pxml };



            try
            {
                retorno.ExistError = false;
                retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                    ("dbo.OFFLINE_CierreCaja_Grabar  @xml", parameters).FirstOrDefault();

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