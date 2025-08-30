using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USWsLibrary.Common;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{
    public class ParametrosServices
    {
        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public ParametrosServices()
        {
            _db = new DataModel();

        }

        #endregion

        #region Operaciones

        public List<ParametroOutputDto> TiposAbono()
        {

            return Lista(ConstGeneralCode.PAYMENT_TYPE);
        }


        public List<ParametroOutputDto> Bancos()
        {

            return Lista(ConstGeneralCode.BANK_PadreID);
        }

        public List<ParametroOutputDto> Terminos()
        {
            return Lista(ConstGeneralCode.CLI_TERMINOS);
        }


        public List<ParametroOutputDto> List(string ParamCode)
        {
            return Lista(ParamCode);
        }

        public List<ParametroOutputDto> TiposIdentificacion()
        {
            List<ParametroOutputDto> retorno = new List<ParametroOutputDto>();

            try
            {
                retorno = _db.Database.SqlQuery<ParametroOutputDto>
                    ("dbo.SRI_TIPOS_IDENTICACION_VENTA_RECUPERAR ").ToList();

            }
            catch (SqlException ee)
            {

            }
            catch (Exception ee)
            {

            }

            return retorno;
        }


        //Obtener  id de divisa *
        public List<ParametroOutputDto> TiposCambio()
        {
            List<ParametroOutputDto> retorno = new List<ParametroOutputDto>();

            try
            {
                retorno = _db.Database.SqlQuery<ParametroOutputDto>
                    ("dbo.SIS_DIVISAS_RECUPERAR ").ToList();

            }
            catch (SqlException ee)
            {

            }
            catch (Exception ee)
            {

            }

            return retorno;
        }



        private List<ParametroOutputDto> Lista(string CodigoPadre)
        {
            SqlParameter pCodigo = new SqlParameter();
            pCodigo.ParameterName = "Codigo";
            pCodigo.DbType = System.Data.DbType.String;
            pCodigo.Direction = System.Data.ParameterDirection.Input;
            pCodigo.Size = 50;
            pCodigo.IsNullable = true;
            pCodigo.Value = CodigoPadre;

            object[] parameters = new object[] { pCodigo };

            List<ParametroOutputDto> retorno = new List<ParametroOutputDto>();

            try
            {
                retorno = _db.Database.SqlQuery<ParametroOutputDto>
                    ("dbo.SIS_LISTA_PARAMETROS  @Codigo", parameters).ToList();
            }
            catch (SqlException ee)
            {

            }
            catch (Exception ee)
            {

            }

            return retorno;
        }
        #endregion




        #region Integracion Movil

        public PagedList<SIS_PARAMETRO> AllParameters(GenericFilterInp Filtro)
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


            PagedList<SIS_PARAMETRO> retorno = new PagedList<SIS_PARAMETRO>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<SIS_PARAMETRO>
                  ("dbo.OFFLINE_SIS_Parametros_ListadoPagina @PageNumber,@PageSize, @TotalSalida out", parameters).ToList();

                retorno.Count = Convert.ToInt32(pTotalSalida.Value);

            }
            catch (SqlException ee)
            {

            }
            catch (Exception ee)
            {

            }

            return retorno;
        }

        public PagedList<SIS_PARAMETRO> ParametersFilter(GenericFilterInp Filtro)
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

            SqlParameter pCodigoPadre = new SqlParameter();
            pCodigoPadre.ParameterName = "CodigoPadre";
            pCodigoPadre.DbType = System.Data.DbType.String;
            pCodigoPadre.Direction = System.Data.ParameterDirection.Input;
            pCodigoPadre.Size = 50;
            pCodigoPadre.Value = Filtro.genericstring;


            object[] parameters = new object[] { pPageNumber, pPageSize, pTotalSalida, pCodigoPadre };


            PagedList<SIS_PARAMETRO> retorno = new PagedList<SIS_PARAMETRO>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<SIS_PARAMETRO>
                  ("dbo.OFFLINE_SIS_Parametros_ListadoPagina_Codigo @PageNumber,@PageSize, @TotalSalida out,@CodigoPadre", parameters).ToList();

                retorno.Count = Convert.ToInt32(pTotalSalida.Value);

            }
            catch (SqlException ee)
            {

            }
            catch (Exception ee)
            {

            }

            return retorno;
        }




        #endregion

    }
}