using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class SimpleReturn
    {
        public string DocumentId { get; set; }

        public string MovUniqueId { get; set; }

        public bool ExistError { get; set; }

        public string Text { get; set; }

        public ErrorItem ErrorIfExist { get; set; }


    }
}