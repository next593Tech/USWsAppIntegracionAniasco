namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Code_DeudorID_OutPutDto//
    {
        public string DeudorID { get; set; }

        public string ErrorCode { get; set; }
    }
}

