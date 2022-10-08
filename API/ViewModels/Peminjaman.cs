using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class Peminjaman
    {
        public int Barang_Id { get; set; }
        public int Karyawan_Id { get; set; }
        public DateTime Tanggal_Pinjam { get; set; }
        public DateTime Tanggal_Kembali { get; set; }
        public string Status { get; set; }
        public int Jumlah { get; set; }
    }
}
