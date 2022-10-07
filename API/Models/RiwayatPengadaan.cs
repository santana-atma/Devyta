using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class RiwayatPengadaan
    {
        [Key]
        public int Id { get; set; }
        public Barang Barang { get; set; }
        [ForeignKey("Barang")]
        public int Barang_Id { get; set; }
        public DateTime Tanggal { get; set; }
        public int Jumlah { get; set; }
        public double Harga { get; set; }
        public string Supplier { get; set; }

    }
}
