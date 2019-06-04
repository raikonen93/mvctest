using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcTest.Models
{
    [Table("UserRole")]
    public class UserRole
    { 
        [Key]
        public long Id { get; set; }
        [Required]
        public string Role { get; set; }        
        public virtual ICollection<User> Users { get; set; }

        public UserRole()
        {
            Users = new List<User>();
        }
    }
}