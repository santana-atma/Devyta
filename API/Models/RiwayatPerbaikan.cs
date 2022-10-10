using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class RiwayatPerbaikan
    {
        [Key]
        public int Id { get; set; }
        public Barang Barang { get; set; }
        [ForeignKey("Barang")]
        public int Barang_Id { get; set; }

        public Karyawan Karyawan { get; set; }
        [ForeignKey("Karyawan")]
        public int Karyawan_Id { get; set; }

        public string Keterangan { get; set; }
        public double Biaya { get; set; }
        public int Jumlah { get; set; }
        public string Status { get; set; }
        public DateTime Tanggal_Terima { get; set; }
        public DateTime Tanggal_Selesai { get; set; }
    }

}
