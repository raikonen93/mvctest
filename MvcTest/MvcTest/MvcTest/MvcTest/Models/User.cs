using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.IO;

namespace MvcTest.Models
{
    [Table("User")]
    public class User
    {       
        public long Id { get; set; }

        [Required(ErrorMessage ="Please enter the name!")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Characters are not allowed.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the e-mail!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Skype { get; set; }

        [StringLength(50)]
        public string Signature { get; set; }

        [DefaultValue(true)]
        public bool IsEnabled { get; set; }

        [DefaultValue(1)]
        public long UserRoleId { get; set; }


        [Column(TypeName = "image")]
        public byte[] Avatar { get; set; } 

        public virtual UserRole UserRole { get; set; }
    }
}