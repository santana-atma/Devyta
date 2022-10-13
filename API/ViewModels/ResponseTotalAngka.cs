using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class ResponseTotalAngka
    {
        public int TotalAset { get; set; }
        public int TotalPeminjaman { get; set; }
        public int TotalAdmin { get; set; }
        public int TotalStaff { get; set; }
    }
}
