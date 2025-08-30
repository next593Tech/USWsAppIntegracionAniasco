using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using USWsApp.Services;
using USWsLibrary.Models;
using USWsLibrary.Models.DashBoard;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
    public class DashboardController : ApiController
    { 
        #region Propiedades y Campos

         DashBoardServices _objsrv;

        #endregion

        #region Constructores 

        public DashboardController()
        {
            _objsrv = new DashBoardServices();

        } 
        #endregion

        #region Acciones  
        [HttpPost]
        public List<SalesByHourDto> SalesByHour(QueryFilterInp filter)
        {
            return _objsrv.SalesByHour(filter.DateFrom);
        }

        [HttpPost]
        public List<SalesByDayDto> SalesByDay(QueryFilterInp filter)
        {
            return _objsrv.SalesByDay(filter.DateFrom);
        }

        [HttpPost]
        public List<SalesByMonthDto> SalesByMonth(QueryFilterInp filter)
        {
            return _objsrv.SalesByMonth(filter.DateFrom);
        }

        [HttpPost]
        public List<SalesByCustomerDto> SalesByCustomer(QueryFilterInp filter)
        {
            return _objsrv.SalesByCustomer(filter.DateFrom);
        }

        [HttpPost]
        public List<SalesByCustomerMonthlyDto> SalesByCustomerMonthly(QueryFilterInp filter)
        {
            return _objsrv.SalesByCustomerMonthly(filter.DateFrom);
        }


        [HttpPost]
        public List<SalesByProductDto> SalesByProduct(QueryFilterInp filter)
        {
            return _objsrv.SalesByProduct(filter.DateFrom);
        }



        [HttpPost]
        public List<SalesRankedDto> SalesByLine(QueryFilterInp filter)
        {
            return _objsrv.SalesByLine(filter.DateFrom);
        }

        [HttpPost]
        public List<SalesRankedDto> SalesByBrand(QueryFilterInp filter)
        {
            return _objsrv.SalesByBrand(filter.DateFrom);
        }

        [HttpPost]
        public SalesTopValues SalesTopValues(QueryFilterInp filter)
        {
            return _objsrv.SalesTopValues(filter.DateFrom);
        }
       

        #endregion



    }
}