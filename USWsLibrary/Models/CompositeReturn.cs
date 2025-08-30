using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class CompositeReturn<T> where T : class
    {
        public T CompositeObjet { get; set; }
         
        public bool ExistError { get; set; }

        public string Text { get; set; }

        public ErrorItem ErrorIfExist { get; set; }
    }
}