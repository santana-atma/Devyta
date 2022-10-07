using API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public int User_Id { get; set; }
        public Role Role { get; set; }
        [ForeignKey("Role")]
        public int Role_Id { get; set; }
    }
}
