using NexoMobile;
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
    public class ReportsServices
    {
        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public ReportsServices()
        {
            _db = new DataModel();

        }

        #endregion

  
        public CompositeReturn<List<VEN_CUADRE_DE_CAJADT>> ObtenerCuadreCaja(FAC_Filter filtro)
        {

            //Llenar Parametros
            SqlParameter pFechaDesde = new SqlParameter();
            pFechaDesde.ParameterName = "FechaDesde";
            pFechaDesde.DbType = System.Data.DbType.DateTime;
            pFechaDesde.IsNullable = true;
            pFechaDesde.Direction = System.Data.ParameterDirection.Input;

            SqlParameter pFechaHasta = new SqlParameter();
            pFechaHasta.ParameterName = "FechaHasta";
            pFechaHasta.DbType = System.Data.DbType.DateTime;
            pFechaHasta.IsNullable = true;
            pFechaHasta.Direction = System.Data.ParameterDirection.Input;

            SqlParameter pSucursal = new SqlParameter();
            pSucursal.ParameterName = "SucursalID";
            pSucursal.DbType = System.Data.DbType.String;
            pSucursal.Size = 2;
            pSucursal.IsNullable = true;
            pSucursal.Direction = System.Data.ParameterDirection.Input;

            if (filtro.FechaDesde == null)
                pFechaDesde.Value = DBNull.Value;
            else
                pFechaDesde.Value = filtro.FechaDesde;

            if (filtro.FechaHasta == null)
                pFechaHasta.Value = DBNull.Value;
            else
                pFechaHasta.Value = filtro.FechaHasta;

            if (filtro.SucursalID == null)
                pSucursal.Value = DBNull.Value;
            else
                pSucursal.Value = filtro.SucursalID;

            //collecion de objetos de parametros
            object[] parameters = new object[] { pFechaDesde, pFechaHasta, pSucursal };


            //inicializacion del retorno
            CompositeReturn<List<VEN_CUADRE_DE_CAJADT>> retorno = new CompositeReturn<List<VEN_CUADRE_DE_CAJADT>>();

            //creacion de la llamada
            try
            {
                var ret = _db.Database.SqlQuery<VEN_CUADRE_DE_CAJADT>
                  ("dbo.ONLINE_VEN_FAC_DT_SpObtenerTotalesYMD  @FechaDesde, @FechaHasta, @SucursalID ", parameters).ToList();

                retorno.CompositeObjet = ret;
                retorno.ExistError = false;

            }
            catch (SqlException ee) // Manejo del error
            {
                retorno.CompositeObjet = new List<VEN_CUADRE_DE_CAJADT>();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = ee.ErrorCode;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
            }
            catch (Exception ee)
            {
                retorno.CompositeObjet = new List<VEN_CUADRE_DE_CAJADT>();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 999999;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
            }

            return retorno;
        }



        public CompositeReturn<List<VEN_REPORTE_VENTAS>> Obtener_Ventas(FAC_Filter filtro)
        {
            //Llenar Parametros
            SqlParameter pFechaDesde = new SqlParameter();
            pFechaDesde.ParameterName = "FechaDesde";
            pFechaDesde.DbType = System.Data.DbType.DateTime;
            pFechaDesde.IsNullable = true;
            pFechaDesde.Direction = System.Data.ParameterDirection.Input;

            SqlParameter pFechaHasta = new SqlParameter();
            pFechaHasta.ParameterName = "FechaHasta";
            pFechaHasta.DbType = System.Data.DbType.DateTime;
            pFechaHasta.IsNullable = true;
            pFechaHasta.Direction = System.Data.ParameterDirection.Input;


            SqlParameter pCliente = new SqlParameter();
            pCliente.ParameterName = "ClienteID";
            pCliente.DbType = System.Data.DbType.String;
            pCliente.Size = 10;
            pCliente.IsNullable = true;
            pCliente.Direction = System.Data.ParameterDirection.Input;


            SqlParameter pSucursal = new SqlParameter();
            pSucursal.ParameterName = "SucursalID";
            pSucursal.DbType = System.Data.DbType.String;
            pSucursal.Size = 2;
            pSucursal.IsNullable = true;
            pSucursal.Direction = System.Data.ParameterDirection.Input;

            SqlParameter pCategoria = new SqlParameter();
            pCategoria.ParameterName = "Categoria";
            pCategoria.DbType = System.Data.DbType.String;
            pCategoria.Size = 50;
            pCategoria.IsNullable = true;
            pCategoria.Direction = System.Data.ParameterDirection.Input;

            SqlParameter pProducto = new SqlParameter();
            pProducto.ParameterName = "Producto";
            pProducto.DbType = System.Data.DbType.String;
            pProducto.Size = 50;
            pProducto.IsNullable = true;
            pProducto.Direction = System.Data.ParameterDirection.Input;


            if (filtro.FechaDesde == null)
                pFechaDesde.Value = DBNull.Value;
            else
                pFechaDesde.Value = filtro.FechaDesde;

            if (filtro.FechaHasta == null)
                pFechaHasta.Value = DBNull.Value;
            else
                pFechaHasta.Value = filtro.FechaHasta;

            if (filtro.ClienteID == null)
                pCliente.Value = DBNull.Value;
            else
                pCliente.Value = filtro.ClienteID;

            if (filtro.SucursalID == null)
                pSucursal.Value = DBNull.Value;
            else
                pSucursal.Value = filtro.SucursalID;

            if (filtro.Categoria == null)
                pCategoria.Value = DBNull.Value;
            else
                pCategoria.Value = filtro.Categoria;




            //collecion de objetos de parametros
            object[] parameters = new object[] { pFechaDesde, pFechaHasta, pCliente, pSucursal, pCategoria };


            //inicializacion del retorno
            CompositeReturn<List<VEN_REPORTE_VENTAS>> retorno = new CompositeReturn<List<VEN_REPORTE_VENTAS>>();

            //creacion de la llamada
            try
            {
                var ret = _db.Database.SqlQuery<VEN_REPORTE_VENTAS>
                  ("dbo.ONLINE_VEN_TotalesProductosYM  @FechaDesde, @FechaHasta, @ClienteID,  @SucursalID, @Categoria  ", parameters).ToList();

                retorno.CompositeObjet = ret;
                retorno.ExistError = false;

            }
            catch (SqlException ee) // Manejo del error
            {
                retorno.CompositeObjet = new List<VEN_REPORTE_VENTAS>();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = ee.ErrorCode;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
            }
            catch (Exception ee)
            {
                retorno.CompositeObjet = new List<VEN_REPORTE_VENTAS>();
                retorno.ExistError = true;
                retorno.ErrorIfExist = new ErrorItem();
                retorno.ErrorIfExist.ErrorNumber = 999999;
                retorno.ErrorIfExist.ErrorMessage = ee.Message;
            }

            return retorno;
        }



    }
}
