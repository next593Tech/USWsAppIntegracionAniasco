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
    public class FacturasServices
    {
        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public FacturasServices()
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

        
         

        public SimpleReturn GrabarOff(FAC_DTO Factura)
        {
            SimpleReturn retorno = new SimpleReturn();


            var totaldetalles = TotalizaDetalles(Factura);


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
            pXml.Value = HelperConvert<FAC_DTO>.E2Xml(Factura);

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
                if (!Factura.EsPasajeFactura)
                    retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                    ("dbo.OFFLINE_FAC_Grabar  @infoXml,@IDRetorno out ", parameters).FirstOrDefault();
                else
                    retorno.ErrorIfExist = _db.Database.SqlQuery<ErrorItem>
                       ("dbo.OFFLINE_FAC_Grabar_Pasaje  @infoXml,@IDRetorno out", parameters).FirstOrDefault();

                
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

        private decimal TotalizaDetalles(FAC_DTO factura)
        {
            decimal retorno = 0M;
            foreach (var item in factura.LIST_FACT_DT)
            {
                retorno = retorno + item.Cantidad * item.Precio * item.Factor * (1 - item.TasaDescuento / 100) + item.Impuesto + item.ImpuestoIce + item.ImpuestoVerde;
            }
            return retorno;
        }

    }
}