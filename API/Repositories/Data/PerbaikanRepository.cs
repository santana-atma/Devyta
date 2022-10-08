using API.Context;
using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class PerbaikanRepository
    {
        private readonly MyContext _context;

        public PerbaikanRepository(MyContext context)
        {
            _context = context;
        }

        #region Perbaikan
        //READ/GET All Perbaikan
        public List<RiwayatPerbaikan> Get()
        {
            return _context.RiwayatPerbaikan.ToList();
        }

        //READ/GET By Id Perbaikan
        public RiwayatPerbaikan Get(int Id)
        {
            return _context.RiwayatPerbaikan.Find(Id);
        }


        //  CREATE/POST Perbaikan
        public int Post(Perbaikan perbaikan)
        {

            using var transaction = _context.Database.BeginTransaction();
            var result = 0;
            try
            {
                DateTime defaultTglSelesai = DateTime.Parse("0000-00-00");
                var data = new RiwayatPerbaikan()
                {
                    Barang_Id = perbaikan.Barang_Id,
                    Keterangan = perbaikan.Keterangan,
                    Biaya = perbaikan.Biaya, //dari FrontEnd harus set ke 0 saat create
                    Jumlah = perbaikan.Jumlah,
                    Status = "DIPERIKSA",
                    Tanggal_Terima = perbaikan.Tanggal_Terima,
                    Tanggal_Selesai = perbaikan.Tanggal_Selesai == null ? defaultTglSelesai : perbaikan.Tanggal_Selesai
                };
                _context.RiwayatPerbaikan.Add(data);
                result += _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                result = 0;
            }
            return result;
        }

        //  UPDATE/PUT Perbaikan
        public int Put(int Id, Perbaikan perbaikan)
        {

            using var transaction = _context.Database.BeginTransaction();
            var result = 0;
            try
            {
                var riwayatPerbaikan = _context.RiwayatPerbaikan.Find(Id);
                //Cek apakah ada pinjaman dengan barang_id dan peminjam_id yang sama, jika Not Null maka bisa Update
                if (riwayatPerbaikan == null)
                {
                    return 0;
                }
                else
                {
                    riwayatPerbaikan.Barang_Id = perbaikan.Barang_Id;
                    riwayatPerbaikan.Biaya = perbaikan.Biaya;
                    riwayatPerbaikan.Status = perbaikan.Status;
                    riwayatPerbaikan.Tanggal_Terima = perbaikan.Tanggal_Terima;
                    riwayatPerbaikan.Tanggal_Selesai = perbaikan.Tanggal_Selesai;

                    _context.RiwayatPerbaikan.Update(riwayatPerbaikan);
                    result += _context.SaveChanges();
                    transaction.Commit();
                }

            }
            catch (Exception)
            {
                transaction.Rollback();
                result = 0;
            }
            return result;
        }

        //  DELETE Peminjaman
        public int Delete(int Id)
        {

            using var transaction = _context.Database.BeginTransaction();
            var result = 0;
            try
            {
                var riwayatPeminjaman = _context.RiwayatPeminjaman.Find(Id);

                //Cek apakah ada pinjaman dengan barang_id dan peminjam_id yang sama, jika Not Null maka bisa Update
                if (riwayatPeminjaman == null)
                {
                    return 0;
                }
                else
                {
                    var barang = _context.Barang.Find(riwayatPeminjaman.Barang_Id);

                    //Kembalikan stok di Tb Barang sesuai jumlah yg dipinjam sebelumnya
                    barang.Stok += riwayatPeminjaman.Jumlah;
                    _context.Barang.Update(barang);
                    result += _context.SaveChanges();

                    _context.RiwayatPeminjaman.Remove(riwayatPeminjaman);
                    result += _context.SaveChanges();
                    transaction.Commit();
                }

            }
            catch (Exception)
            {
                transaction.Rollback();
                result = 0;
            }
            return result;
        }

        
        #endregion Perbaikan
    }
}
