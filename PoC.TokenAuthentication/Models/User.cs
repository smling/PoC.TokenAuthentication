using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoC.TokenAuthentication.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public long UserID { get; set; }
        [Required(ErrorMessage ="User name required.")]
        [StringLength(50, MinimumLength =2, ErrorMessage ="User name length shold between 2 and 50.")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Password")]
        [MaxLength(30, ErrorMessage = "Password cannot be Greater than 30 Charaters")]
        [StringLength(31, MinimumLength = 7, ErrorMessage = "Password Must be Minimum 7 Charaters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required Email Address")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter Valid Email address")]
        public string EmailAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}