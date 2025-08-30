using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace USWsApp.Controllers 
{
   // [Authorize(Roles ="Test")]
    public class ValuesController : ApiController
    {
        // GET api/values
        public string GetTest() 
        {
            var userName = this.RequestContext.Principal.Identity.Name;
            return String.Format("Estimado {0} si esta viendo esto, los servicios estan operativos.", userName);
        }
    }
}
