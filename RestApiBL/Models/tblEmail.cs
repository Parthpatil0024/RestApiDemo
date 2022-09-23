namespace RestApiBL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblEmail")]
    public partial class tblEmail
    {
        public int Id { get; set; }

        [Required]
        public string ToEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public int IsEmailSent { get; set; }
    }
}
