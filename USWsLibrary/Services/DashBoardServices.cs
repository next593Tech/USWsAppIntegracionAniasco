using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USWsLibrary.Models;
using USWsLibrary.Models.DashBoard;

namespace USWsApp.Services
{
    public class DashBoardServices
    {
        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores
        public DashBoardServices()
        {
            _db = new DataModel();

        }
        #endregion
        #region Operaciones


        public SalesTopValues SalesTopValues(DateTime ldFecha) 
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                pFecha.Value = DBNull.Value;
            else
                pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            SalesTopValues retorno = new SalesTopValues();
            try
            {
                retorno = _db.Database.SqlQuery<SalesTopValues>
                  ("dbo.DSH_ValuesOfTheMonth @Fecha", parameters).SingleOrDefault();
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



        // Lista de ventas por hora de una fecha determinada
        public List<SalesByHourDto> SalesByHour(DateTime ldFecha )
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                  pFecha.Value = DBNull.Value;
            else
                  pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            List<SalesByHourDto> retorno = new List<SalesByHourDto>();
            try
            {
                retorno = _db.Database.SqlQuery<SalesByHourDto>
                  ("dbo.DSH_SalesByHour @Fecha", parameters).ToList();
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

        // Lista de ventas diarias de un mes determinado
        public List<SalesByDayDto> SalesByDay(DateTime ldFecha)
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                pFecha.Value = DBNull.Value;
            else
                pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            List<SalesByDayDto> retorno = new List<SalesByDayDto>();
            try
            {
                retorno = _db.Database.SqlQuery<SalesByDayDto>
                  ("dbo.DSH_SalesByDay @Fecha", parameters).ToList();
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

        // Lista de ventas mensuales de un anio determinado
        public List<SalesByMonthDto> SalesByMonth(DateTime ldFecha)
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                pFecha.Value = DBNull.Value;
            else
                pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            List<SalesByMonthDto> retorno = new List<SalesByMonthDto>();
            try
            {
                retorno = _db.Database.SqlQuery<SalesByMonthDto>
                  ("dbo.DSH_SalesByMonth @Fecha", parameters).ToList();
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

        // Lista de ventas por clientes
        public List<SalesByCustomerDto> SalesByCustomer(DateTime ldFecha)
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                pFecha.Value = DBNull.Value;
            else
                pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            List<SalesByCustomerDto> retorno = new List<SalesByCustomerDto>();
            try
            {
                retorno = _db.Database.SqlQuery<SalesByCustomerDto>
                  ("dbo.DSH_SalesByCustomer @Fecha", parameters).ToList();
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

        // Lista de ventas mensuales por cliente
        public List<SalesByCustomerMonthlyDto> SalesByCustomerMonthly(DateTime ldFecha)
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                pFecha.Value = DBNull.Value;
            else
                pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            List<SalesByCustomerMonthlyDto> retorno = new List<SalesByCustomerMonthlyDto>();
            try
            {
                retorno = _db.Database.SqlQuery<SalesByCustomerMonthlyDto>
                  ("dbo.DSH_SalesByCustomerMonthly @Fecha", parameters).ToList();
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

        // Lista de ventas por Productos
        public List<SalesByProductDto> SalesByProduct(DateTime ldFecha)
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                pFecha.Value = DBNull.Value;
            else
                pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            List<SalesByProductDto> retorno = new List<SalesByProductDto>();
            try
            {
                retorno = _db.Database.SqlQuery<SalesByProductDto>
                  ("dbo.DSH_SalesByProduct @Fecha", parameters).ToList();
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


        // By Line
        public List<SalesRankedDto> SalesByLine(DateTime ldFecha)
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                pFecha.Value = DBNull.Value;
            else
                pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            List<SalesRankedDto> retorno = new List<SalesRankedDto>();
            try
            {
                retorno = _db.Database.SqlQuery<SalesRankedDto>
                  ("dbo.DSH_SalesByLine @Fecha", parameters).ToList();
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


        // By Brand
        public List<SalesRankedDto> SalesByBrand(DateTime ldFecha)
        {
            SqlParameter pFecha = new SqlParameter();
            pFecha.ParameterName = "Fecha";
            pFecha.DbType = System.Data.DbType.DateTime;
            pFecha.Size = 10;
            pFecha.IsNullable = true;
            pFecha.Direction = System.Data.ParameterDirection.Input;
            if (ldFecha == null)
                pFecha.Value = DBNull.Value;
            else
                pFecha.Value = ldFecha;

            object[] parameters = new object[] { pFecha };

            List<SalesRankedDto> retorno = new List<SalesRankedDto>();
            try
            {
                retorno = _db.Database.SqlQuery<SalesRankedDto>
                  ("dbo.DSH_SalesByBrand @Fecha", parameters).ToList();
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

    }
}