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
    public class TomaFisicaServices
    {
        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public TomaFisicaServices()
        {
            _db = new DataModel();

        }


        #endregion
         
        #region Operaciones
        
          
        public PagedList<INV_FISICO_DTO> ListadoOrdenes(GenericFilterInp filtro)
        {

            #region Parametros


            SqlParameter pPageNumber = new SqlParameter();
            pPageNumber.ParameterName = "PageNumber";
            pPageNumber.DbType = System.Data.DbType.Int32;
            pPageNumber.Direction = System.Data.ParameterDirection.Input;
            pPageNumber.Size = 4;
            pPageNumber.Value = filtro.PageNumber;

            SqlParameter pPageSize = new SqlParameter();
            pPageSize.ParameterName = "PageSize";
            pPageSize.DbType = System.Data.DbType.Int32;
            pPageSize.Direction = System.Data.ParameterDirection.Input;
            pPageSize.Size = 4;
            pPageSize.Value = filtro.PageSize;

            SqlParameter pbusqueda = new SqlParameter();
            pbusqueda.ParameterName = "UserName";
            pbusqueda.DbType = System.Data.DbType.String;
            pbusqueda.Size = 200;
            pbusqueda.IsNullable = true;
            pbusqueda.Direction = System.Data.ParameterDirection.Input;
            if (filtro.genericstring == null)
                pbusqueda.Value = DBNull.Value;
            else
                pbusqueda.Value = filtro.genericstring;


            //@Total parametro de Salida
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;
            #endregion

            object[] parameters = new object[] { pPageNumber, pPageSize, pbusqueda, pTotalSalida };


            PagedList<INV_FISICO_DTO > retorno = new PagedList<INV_FISICO_DTO>();
         
            try
            {
                retorno.Results = _db.Database.SqlQuery<INV_FISICO_DTO>
                    ("dbo.OFFLINE_INV_Listado_OrdenesTomaFisica  @PageNumber,@PageSize,@UserName, @TotalSalida out", parameters).ToList();

                retorno.Count = Convert.ToInt32(pTotalSalida.Value);

            }
            catch (SqlException ee)
            {
                //throw ee;
            }
            catch (Exception ee)
            {
                throw ee;
            }

            return retorno;

        }

         

        public SimpleReturn Grabar(INV_CONTEO_INPUT Orden)
        {
            SimpleReturn retorno = new SimpleReturn();
             


            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<INV_CONTEO_INPUT>.E2Xml(Orden);

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
                    ("dbo.OFFLINE_INV_TomaFisica_Grabar  @infoXml,@IDRetorno out ", parameters).FirstOrDefault();

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

        #endregion


        #region Validaciones



        #endregion
    }
}