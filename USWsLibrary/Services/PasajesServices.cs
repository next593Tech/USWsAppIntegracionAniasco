using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USWsLibrary.Common;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{
    public class PasajesServices
    {

        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public PasajesServices()
        {
            _db = new DataModel();

        }

        #endregion









        public PagedList<FrecuenciaDto> ListadoFrecuencias()
        {

            //@Total parametro de Salida
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;


            object[] parameters = new object[] { pTotalSalida };



            PagedList<FrecuenciaDto> retorno = new PagedList<FrecuenciaDto>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<FrecuenciaDto>
                  ("dbo.OFFLINE_TRP_Listado_Frecuencia  @TotalSalida out", parameters).ToList();

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



        public PagedList<BusesDto> ListadoBuses()
        {

            //@Total parametro de Salida
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;


            object[] parameters = new object[] { pTotalSalida };



            PagedList<BusesDto> retorno = new PagedList<BusesDto>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<BusesDto>
                  ("dbo.OFFLINE_TRP_Listado_Buses  @TotalSalida out", parameters).ToList();

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


        public PagedList<TarifarioDto> ListadoTarifario()
        {

            //@Total parametro de Salida
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;


            object[] parameters = new object[] { pTotalSalida };



            PagedList<TarifarioDto> retorno = new PagedList<TarifarioDto>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<TarifarioDto>
                  ("dbo.OFFLINE_TRP_Listado_Tarifario  @TotalSalida out", parameters).ToList();

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

        public PagedList<RutasDto> ListadoRutas()
        {

            //@Total parametro de Salida
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;


            object[] parameters = new object[] { pTotalSalida };



            PagedList<RutasDto> retorno = new PagedList<RutasDto>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<RutasDto>
                  ("dbo.OFFLINE_TRP_Listado_Rutas  @TotalSalida out", parameters).ToList();

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

        public PagedList<RutasDetalleDto> ListadoRutasDetalle()
        {

            //@Total parametro de Salida
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;


            object[] parameters = new object[] { pTotalSalida };



            PagedList<RutasDetalleDto> retorno = new PagedList<RutasDetalleDto>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<RutasDetalleDto>
                  ("dbo.OFFLINE_TRP_Listado_RutasDetalle  @TotalSalida out", parameters).ToList();

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



        public PagedList<ReservaDto> ListadoReservaciones(FilterReservation Filtro)
        {

            //SqlParameter pPageNumber = new SqlParameter();
            //pPageNumber.ParameterName = "PageNumber";
            //pPageNumber.DbType = System.Data.DbType.Int32;
            //pPageNumber.Direction = System.Data.ParameterDirection.Input;
            //pPageNumber.Size = 4;
            //pPageNumber.Value = Filtro.PageNumber;

            //SqlParameter pPageSize = new SqlParameter();
            //pPageSize.ParameterName = "PageSize";
            //pPageSize.DbType = System.Data.DbType.Int32;
            //pPageSize.Direction = System.Data.ParameterDirection.Input;
            //pPageSize.Size = 4;
            //pPageSize.Value = Filtro.PageSize;

            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "Xml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<FilterReservation>.E2Xml(Filtro);


            //@Total parametro de Salida
            SqlParameter pTotalSalida = new SqlParameter();
            pTotalSalida.ParameterName = "TotalSalida";
            pTotalSalida.DbType = System.Data.DbType.Int32;
            pTotalSalida.Direction = System.Data.ParameterDirection.Output;
            pTotalSalida.Size = 4;
            pTotalSalida.Value = 0;


            object[] parameters = new object[] { pXml, pTotalSalida };



            PagedList<ReservaDto> retorno = new PagedList<ReservaDto>();
            try
            {
                retorno.Results = _db.Database.SqlQuery<ReservaDto>
                  ("dbo.OFFLINE_TRP_Listado_Reservaciones  @Xml, @TotalSalida out", parameters).ToList();

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



        public ReturnReservation GrabarReservacion(ReservaDto Reserva)
        {

            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<ReservaDto>.E2Xml(Reserva);

            SqlParameter pErrorExist = new SqlParameter();
            pErrorExist.ParameterName = "ErrorExist";
            pErrorExist.DbType = System.Data.DbType.Boolean;
            pErrorExist.Direction = System.Data.ParameterDirection.Output;

            pErrorExist.Value = false;

            ReturnReservation retorno = new ReturnReservation();


            object[] parameters = new object[] { pXml, pErrorExist };
            bool ResultadoError = false;
            try
            {
                retorno.ExistError = false;
                retorno.ListReservaciones = _db.Database.SqlQuery<ReservaDto>
                    ("dbo.OFFLINE_TRP_GrabarReservacion  @infoXml, @ErrorExist out", parameters).ToList();

                if (pErrorExist.Value != null)
                    ResultadoError = Convert.ToBoolean(pErrorExist.Value);

                if (ResultadoError == true)
                {
                    retorno.ExistError = true;
                    retorno.ErrorIfExist = new ErrorItem() { ErrorLine = 0, ErrorNumber = 0, ErrorMessage = "Error Asiento Reservado" };
                }







            }
            catch (SqlException ee)
            {


            }
            catch (Exception ee)
            {

            }

            return retorno;

        }


        public SimpleReturn ActualizarReservacionOnline(FilterIdReservation ReservaId)
        {
            SimpleReturn retorno = new SimpleReturn();


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
            pXml.ParameterName = "Xml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<FilterIdReservation>.E2Xml(ReservaId);

            SqlParameter pIdRet = new SqlParameter();
            pIdRet.ParameterName = "IDRetorno";
            pIdRet.DbType = System.Data.DbType.String;
            pIdRet.Direction = System.Data.ParameterDirection.Output;
            pIdRet.Size = 10;
            pIdRet.Value = string.Empty;


            object[] parameters = new object[] { pXml };

            try
            {
                retorno.ExistError = false;
                retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                    ("dbo.OFFLINE_TRP_ActualizarReservacion  @Xml", parameters).FirstOrDefault();
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

        public ReturnReservation EliminarReservacion(ReservaDto Reserva)
        {
            SqlParameter pXml = new SqlParameter();
            pXml.ParameterName = "infoXml";
            pXml.DbType = System.Data.DbType.Xml;
            pXml.Direction = System.Data.ParameterDirection.Input;
            pXml.Value = HelperConvert<ReservaDto>.E2Xml(Reserva);

            SqlParameter pErrorExist = new SqlParameter();
            pErrorExist.ParameterName = "ErrorExist";
            pErrorExist.DbType = System.Data.DbType.Boolean;
            pErrorExist.Direction = System.Data.ParameterDirection.Output;

            pErrorExist.Value = false;

            ReturnReservation retorno = new ReturnReservation();


            object[] parameters = new object[] { pXml, pErrorExist };
            bool ResultadoError = false;
            try
            {
                retorno.ExistError = false;
                retorno.ListReservaciones = _db.Database.SqlQuery<ReservaDto>
                    ("dbo.OFFLINE_TRP_EliminarReservacion  @infoXml, @ErrorExist out", parameters).ToList();

                if (pErrorExist.Value != null)
                    ResultadoError = Convert.ToBoolean(pErrorExist.Value);

                if (ResultadoError == true)
                {
                    retorno.ExistError = true;
                    retorno.ErrorIfExist = new ErrorItem() { ErrorLine = 0, ErrorNumber = 0, ErrorMessage = "Error el Asiento no pudo ser eliminado" };
                }







            }
            catch (SqlException ee)
            {


            }
            catch (Exception ee)
            {

            }

            return retorno;

        }


        public SimpleReturn GrabarPasajeros(PasajeroDto pasajero)
        {
            SimpleReturn retorno = new SimpleReturn();





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
            pXml.Value = HelperConvert<PasajeroDto>.E2Xml(pasajero);

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
                    ("dbo.OFFLINE_TRP_GrabarPasajeros  @infoXml,@IDRetorno out ", parameters).FirstOrDefault();
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