using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using USWsLibrary.Models;

namespace USWsApp.Services
{
    public class EmploymentsServices
    {

        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public EmploymentsServices()
        {
            _db = new DataModel();

        }

        #endregion

        #region Operaciones
        public CompositeReturn<EmploymentMinOut> GetRestrictions(EmploymentFilterInp filter)
        {

            CompositeReturn<EmploymentMinOut> retorno = new CompositeReturn<EmploymentMinOut>();
            retorno.CompositeObjet = new EmploymentMinOut();
            SqlParameter pUserName = new SqlParameter();
            pUserName.ParameterName = "UserName";
            pUserName.DbType = System.Data.DbType.String;
            pUserName.Direction = System.Data.ParameterDirection.Input;
            pUserName.Size = 50;

            if (filter.UserName == null)
                pUserName.Value = DBNull.Value;
            else
                pUserName.Value = filter.UserName;


            object[] parameters = new object[] { pUserName };

            try
            {
                retorno.CompositeObjet = _db.Database.SqlQuery<EmploymentMinOut>
                    ("dbo.EMP_EmpleadoObtenerRestricciones @UserName ", parameters).FirstOrDefault();


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
        #endregion

        #region Integración movil offline

        public PagedList<EMP_EMPLEADO> AllEmployees(GenericFilterInp Filtro)
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


            PagedList<EMP_EMPLEADO> retorno = new PagedList<EMP_EMPLEADO>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<EMP_EMPLEADO>
                  ("dbo.OFFLINE_EMP_Empleados_ListadoPagina @PageNumber,@PageSize, @TotalSalida out", parameters).ToList();

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


        //Obtener Cajeros


        public PagedList<VEN_CAJEROS> ObtenerCajeros(GenericFilterInp Filtro)
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


            PagedList<VEN_CAJEROS> retorno = new PagedList<VEN_CAJEROS>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<VEN_CAJEROS>
                  ("dbo.VEN_Cajeros_Listado @PageNumber,@PageSize, @TotalSalida out", parameters).ToList();

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