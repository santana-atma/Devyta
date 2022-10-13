using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class UpdateAccount
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Alamat { get; set; }
        public string Telp { get; set; }
        public string Departemen { get; set; }
        public int Role { get; set; }

    }
}
