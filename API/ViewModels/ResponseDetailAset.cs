using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class ResponseDetailAset
    {
        public Barang Barang { get; set; }
        public List<RiwayatPengadaan> Riwayat_Pengadaan { get; set; }
    }
}
