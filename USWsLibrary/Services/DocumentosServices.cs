using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{
    public class DocumentosServices
    {
        #region Propiedades y campos
        private DataModel _db;
        #endregion

        #region Constructores

        public DocumentosServices()
        {
            _db = new DataModel();

        }

        #endregion

        #region Operaciones
        private DataModel db = new DataModel();

 
        public List<RegistroDocumentoDto> ListaOld(FiltroDto filtro)
        {

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


            SqlParameter pFechaDesde = new SqlParameter();
            pFechaDesde.ParameterName = "FechaDesde";
            pFechaDesde.DbType = System.Data.DbType.String;
            pFechaDesde.Size = 10;
            pFechaDesde.IsNullable = true;
            pFechaDesde.Direction = System.Data.ParameterDirection.Input;
            if (filtro.FechaDesde == null)
                pFechaDesde.Value = DBNull.Value;
            else
                pFechaDesde.Value = filtro.FechaDesde;


            SqlParameter pFechaHasta = new SqlParameter();
            pFechaHasta.ParameterName = "FechaHasta";
            pFechaHasta.DbType = System.Data.DbType.String;
            pFechaHasta.Size = 10;
            pFechaHasta.IsNullable = true;
            pFechaHasta.Direction = System.Data.ParameterDirection.Input;
            if (filtro.FechaHasta == null)
                pFechaHasta.Value = DBNull.Value;
            else
                pFechaHasta.Value = filtro.FechaHasta;


            SqlParameter pTipoDoc = new SqlParameter();
            pTipoDoc.ParameterName = "TipoDoc";
            pTipoDoc.DbType = System.Data.DbType.String;
            pTipoDoc.Size = 10;
            pTipoDoc.IsNullable = true;
            pTipoDoc.Direction = System.Data.ParameterDirection.Input;
            if (filtro.TipoDoc == null)
                pTipoDoc.Value = DBNull.Value;
            else
                pTipoDoc.Value = filtro.TipoDoc;


            SqlParameter pCodCliente = new SqlParameter();
            pCodCliente.ParameterName = "CodCliente";
            pCodCliente.DbType = System.Data.DbType.String;
            pCodCliente.Size = 15;
            pCodCliente.IsNullable = true;
            pCodCliente.Direction = System.Data.ParameterDirection.Input;
            if (filtro.CodCliente == null)
                pCodCliente.Value = DBNull.Value;
            else
                pCodCliente.Value = filtro.CodCliente;


            SqlParameter pCodDocumento = new SqlParameter();
            pCodDocumento.ParameterName = "CodDocumento";
            pCodDocumento.DbType = System.Data.DbType.String;
            pCodDocumento.Size = 100;
            pCodDocumento.IsNullable = true;
            pCodDocumento.Direction = System.Data.ParameterDirection.Input;
            if (filtro.CodDocumento == null)
                pCodDocumento.Value = DBNull.Value;
            else
                pCodDocumento.Value = filtro.CodDocumento;


            SqlParameter pTenantId = new SqlParameter();
            pTenantId.ParameterName = "TenantId";
            pTenantId.DbType = System.Data.DbType.Int32;
            pTenantId.Direction = System.Data.ParameterDirection.Input;
            pTenantId.Size = 4;
            pTenantId.Value = filtro.TenantId;


            object[] parameters = new object[] { pPageNumber, pPageSize, pFechaDesde, pFechaHasta, pTipoDoc, pCodCliente, pCodDocumento, pTenantId };


            List<RegistroDocumentoDto> retorno = new List<RegistroDocumentoDto>();

            try
            {
                retorno = db.Database.SqlQuery<RegistroDocumentoDto>
                    ("dbo.FEL_CONSULTADOCS @PageNumber,@PageSize,@FechaDesde,@FechaHasta,@TipoDoc,@CodCliente,@CodDocumento,@TenantId", parameters).ToList();

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

         

        public PagedList<RegistroDocumentoDto> Lista(FiltroDto filtro)
        {

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


            SqlParameter pFechaDesde = new SqlParameter();
            pFechaDesde.ParameterName = "FechaDesde";
            pFechaDesde.DbType = System.Data.DbType.String;
            pFechaDesde.Size = 10;
            pFechaDesde.IsNullable = true;
            pFechaDesde.Direction = System.Data.ParameterDirection.Input;
            if (filtro.FechaDesde == null)
                pFechaDesde.Value = DBNull.Value;
            else
                pFechaDesde.Value = filtro.FechaDesde;


            SqlParameter pFechaHasta = new SqlParameter();
            pFechaHasta.ParameterName = "FechaHasta";
            pFechaHasta.DbType = System.Data.DbType.String;
            pFechaHasta.Size = 10;
            pFechaHasta.IsNullable = true;
            pFechaHasta.Direction = System.Data.ParameterDirection.Input;
            if (filtro.FechaHasta == null)
                pFechaHasta.Value = DBNull.Value;
            else
                pFechaHasta.Value = filtro.FechaHasta;


            SqlParameter pTipoDoc = new SqlParameter();
            pTipoDoc.ParameterName = "TipoDoc";
            pTipoDoc.DbType = System.Data.DbType.String;
            pTipoDoc.Size = 10;
            pTipoDoc.IsNullable = true;
            pTipoDoc.Direction = System.Data.ParameterDirection.Input;
            if (filtro.TipoDoc == null)
                pTipoDoc.Value = DBNull.Value;
            else
                pTipoDoc.Value = filtro.TipoDoc;


            SqlParameter pCodCliente = new SqlParameter();
            pCodCliente.ParameterName = "CodCliente";
            pCodCliente.DbType = System.Data.DbType.String;
            pCodCliente.Size = 15;
            pCodCliente.IsNullable = true;
            pCodCliente.Direction = System.Data.ParameterDirection.Input;
            if (filtro.CodCliente == null)
                pCodCliente.Value = DBNull.Value;
            else
                pCodCliente.Value = filtro.CodCliente;


            SqlParameter pCodDocumento = new SqlParameter();
            pCodDocumento.ParameterName = "CodDocumento";
            pCodDocumento.DbType = System.Data.DbType.String;
            pCodDocumento.Size = 100;
            pCodDocumento.IsNullable = true;
            pCodDocumento.Direction = System.Data.ParameterDirection.Input;
            if (filtro.CodDocumento == null)
                pCodDocumento.Value = DBNull.Value;
            else
                pCodDocumento.Value = filtro.CodDocumento;


            SqlParameter pTenantId = new SqlParameter();
            pTenantId.ParameterName = "TenantId";
            pTenantId.DbType = System.Data.DbType.Int32;
            pTenantId.Direction = System.Data.ParameterDirection.Input;
            pTenantId.Size = 4;
            pTenantId.Value = filtro.TenantId;

            //@Total parametro de Salida
            SqlParameter pCantidadRegistros = new SqlParameter();
            pCantidadRegistros.ParameterName = "CantidadRegistros";
            pCantidadRegistros.DbType = System.Data.DbType.Int32;
            pCantidadRegistros.Direction = System.Data.ParameterDirection.Output;
            pCantidadRegistros.Size = 4;
            pCantidadRegistros.Value = 0;

            object[] parameters = new object[] { pPageNumber, pPageSize, pFechaDesde, pFechaHasta, pTipoDoc, pCodCliente, pCodDocumento, pTenantId, pCantidadRegistros };


            PagedList<RegistroDocumentoDto> retorno = new PagedList<RegistroDocumentoDto>();

            try
            {
                retorno.Results = db.Database.SqlQuery<RegistroDocumentoDto>
                    ("dbo.FEL_CONSULTADOCS @PageNumber,@PageSize,@FechaDesde,@FechaHasta,@TipoDoc,@CodCliente,@CodDocumento,@TenantId,@CantidadRegistros out", parameters).ToList();


                retorno.Count = Convert.ToInt32(pCantidadRegistros.Value);
                retorno.Total = 0;

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