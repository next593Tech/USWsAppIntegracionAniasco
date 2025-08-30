using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models 
{
    public class ConsultaDocumentoDto
    {
        public string RemoteFileName { get; set; }
        public string ContentType { get; set; }

        public string TenancyName { get; set; }

        public string TenancyFtpHost { get; set; }

        public string TenancyFtpUserName { get; set; }

        public string TenancyFtpPassword { get; set; }


 
    }
}
