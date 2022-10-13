using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Karyawan
    {
        [Key]
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Alamat { get; set; }
        public string Email { get; set; }
        public string Telp { get; set; }
        public string Departemen { get; set; }
        public string Divisi { get; set; }
    }
}
