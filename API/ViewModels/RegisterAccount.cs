using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class RegisterAccount
    {
        public int Id { get; set; }
        [StringLength(30,ErrorMessage = "Input nama minimal terdiri dari 3 karakter", MinimumLength = 3)]
        public string Fullname { get; set; }
        [EmailAddress(ErrorMessage = "Harus memasukkan input data email !")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password harus diisi !")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Input alamat tidak boleh kosong !")]
        public string Alamat { get; set; }
        [StringLength(20, ErrorMessage = "Masukkan input nomor yang valid !", MinimumLength = 3)]
        public string Telp { get; set; }
        [Required]
        public string Departemen { get; set; }
        [Required]
        public int Role { get; set; }
    }
}
