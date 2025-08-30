using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using System.Web.Http.Description;
using USWsLibrary.Models;
using USWsLibrary.Services;

namespace USWsApp.Controllers 
{
    //[Authorize(Roles = "QueryDocs")]   
    public class DocumentosController : ApiController
    {
        #region Propiedades y Campos
        DocumentosServices _objdocsrv;
        #endregion

        #region Constructores 
        public DocumentosController()
        {
            _objdocsrv = new DocumentosServices(); 

        }
        #endregion

        #region Acciones  

        //[HttpPost]
        //[ResponseType(typeof(RegistroDocumentoDto))]
        //public List<RegistroDocumentoDto>  Lista(FiltroDto filtro)  
        //{   
             
        //    List<RegistroDocumentoDto> retorno = new List<RegistroDocumentoDto>();

        //    try
        //    {
        //        retorno = _objdocsrv.Lista(filtro);
        //    }
    
        //    catch (Exception ee)
        //    {
        //        throw ee;
        //    }

        //    return retorno;
              

        //}


        [HttpPost]
        [ResponseType(typeof(RegistroDocumentoDto))]
        public PagedList<RegistroDocumentoDto> Lista(FiltroDto filtro)
        {

            PagedList<RegistroDocumentoDto> retorno = new PagedList<RegistroDocumentoDto>();

            try
            {
                retorno = _objdocsrv.Lista(filtro);
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