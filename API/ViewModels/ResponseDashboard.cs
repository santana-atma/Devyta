using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class ResponseDashboard
    {
        public string NamaAset { get; set; }
        public int Total_Keseluruhan { get; set; }
        public int Total_Peminjaman { get; set; }
        public int Total_Perbaikan { get; set; }
    }
}
