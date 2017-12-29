using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Models
{
    [Table("Token")]
    public class Token
    {
        [Key]
        public long TokenID { get; set; }
        public string TokenKey { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public long UserID { get; set; }
        public long CompanyID { get; set; }
    }
}