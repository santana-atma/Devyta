using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class Perbaikan
    {
        public int Barang_Id { get; set; }
        public string Keterangan { get; set; }
        public double Biaya { get; set; }
        public int Jumlah { get; set; }
        public string Status { get; set; }
        public DateTime Tanggal_Terima { get; set; }
        public DateTime Tanggal_Selesai { get; set; }
    }
}
