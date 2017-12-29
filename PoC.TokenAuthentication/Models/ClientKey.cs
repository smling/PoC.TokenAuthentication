using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Models
{
    [Table("ClientKey")]
    public class ClientKey
    {
        [Key]
        public long ClientKeyID { get; set; }
        public long CompanyID { get; set; }
        public string ClientKeyName { get; set; }
        public string ClientKeySecret { get; set; }
        public DateTime CreatedAt { get; set; }
        public long UserID { get; set; }
    }
}