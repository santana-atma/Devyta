using API.Context;
using API.Models;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
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
            return _context.RiwayatPerbaikan.Include(x=>x.Barang).Include(x => x.Karyawan).ToList();
        }

        //READ/GET By Id Perbaikan
        public RiwayatPerbaikan Get(int Id)
        {
            return _context.RiwayatPerbaikan.Include(x => x.Barang).Include(x => x.Barang).Where(x=>x.Id==Id).FirstOrDefault();
        }


        //  CREATE/POST Perbaikan
        public int Post(Perbaikan perbaikan)
        {

            using var transaction = _context.Database.BeginTransaction();
            var result = 0;
            try
            {
                var barang = _context.RiwayatPeminjaman.Where(x => x.Barang_Id == perbaikan.Barang_Id && x.Karyawan_Id == perbaikan.Karyawan_Id).FirstOrDefault();
                //Validasi barang
                if (barang == null)
                {
                    return 0;
                }
                else
                {
                    
                    if(perbaikan.Jumlah > barang.Jumlah)
                    {
                        return 0;
                    }
                }

                //DateTime defaultTglSelesai = DateTime.Parse("0000-00-00");
                var data = new RiwayatPerbaikan()
                {
                    Barang_Id = perbaikan.Barang_Id,
                    Karyawan_Id = perbaikan.Karyawan_Id,
                    Keterangan = perbaikan.Keterangan,
                    Biaya = 0, //dari FrontEnd harus set ke 0 saat create
                    Jumlah = perbaikan.Jumlah,
                    Status = "DIPERIKSA",
                    Tanggal_Terima = perbaikan.Tanggal_Terima,
                    Tanggal_Selesai = perbaikan.Tanggal_Selesai
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
                var riwayatPeminjaman = _context.RiwayatPeminjaman.Where(x => x.Barang_Id == perbaikan.Barang_Id && x.Karyawan_Id == perbaikan.Karyawan_Id).FirstOrDefault();
                //Cek apakah ada pinjaman dengan barang_id dan peminjam_id yang sama, jika Not Null maka bisa Update
                if (riwayatPerbaikan == null)
                {
                    return 0;
                }
                else
                {
                    if (perbaikan.Jumlah > riwayatPeminjaman.Jumlah)
                    {
                        return 0;
                    }
                    riwayatPerbaikan.Barang_Id = perbaikan.Barang_Id;
                    riwayatPerbaikan.Karyawan_Id = perbaikan.Karyawan_Id;
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
                var riwayatPerbaikan = _context.RiwayatPerbaikan.Find(Id);

                //Cek apakah ada pinjaman dengan barang_id dan peminjam_id yang sama, jika Not Null maka bisa Update
                if (riwayatPerbaikan == null)
                {
                    return 0;
                }
                else
                {
                    

                    _context.RiwayatPerbaikan.Remove(riwayatPerbaikan);
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
