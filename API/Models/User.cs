using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Karyawan Karyawan { get; set; }
        [ForeignKey("Karyawan")]
        //[Key, ForeignKey("Karyawan")]
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Password minimal 8 karakter", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
