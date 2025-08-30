namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NextIDOutputDto
    {        
        public string NextID { get; set; } 
        public string ErrorCode { get; set; }
    }
}
