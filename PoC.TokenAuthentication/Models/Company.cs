using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Models
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public long CompanyID { get; set; }

        [Required(ErrorMessage = "Required Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Required Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required PersonInCharge")]
        public long InChargeUserID { get; set; }
        
        [Required(ErrorMessage = "Required EmailID")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Required Status")]
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}