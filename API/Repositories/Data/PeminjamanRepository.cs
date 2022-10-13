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
    public class PeminjamanRepository
    {
        private readonly MyContext _context;

        public PeminjamanRepository(MyContext context)
        {
            _context = context;
        }

        #region Peminjaman
        //READ/GET All Peminjaman
        public List<RiwayatPeminjaman> Get()
        {
            return _context.RiwayatPeminjaman.Include(x => x.Barang).Include(x=>x.Karyawan).ToList();
        }

        //READ/GET By Id Peminjaman
        public RiwayatPeminjaman Get(int Id)
        {
            return _context.RiwayatPeminjaman.Include(x => x.Barang).Include(x => x.Karyawan).Where(x=>x.Id==Id).FirstOrDefault();
        }


        //  CREATE/POST Peminjaman
        public int Post(Peminjaman peminjaman)
        {

            using var transaction = _context.Database.BeginTransaction();
            var result = 0;
            try
            {
                //Cek apakah masih ada pinjaman dengan barang dan peminjam yang sama dengan status belum kembali, jika True maka belum bisa pinjam
                if (_context.RiwayatPeminjaman.Where(x => x.Karyawan_Id == peminjaman.Karyawan_Id && x.Status == "PINJAM" && x.Barang_Id == peminjaman.Barang_Id).FirstOrDefault() != null)
                {
                    return 0;
                }
                else
                {
                    var isExist = _context.Barang.Find(peminjaman.Barang_Id);
                    if (isExist != null)
                    {
                        if (isExist.Stok < peminjaman.Jumlah)
                        {
                            return 0;
                        }
                        isExist.Stok = isExist.Stok - peminjaman.Jumlah;
                        _context.Barang.Update(isExist);
                        result += _context.SaveChanges();

                    }
                    else
                    {
                        return 0;

                    }

                    var Peminjaman = new RiwayatPeminjaman()
                    {
                        Barang_Id = peminjaman.Barang_Id,
                        Karyawan_Id = peminjaman.Karyawan_Id,
                        Jumlah = peminjaman.Jumlah,
                        Status = "PINJAM",
                        Tanggal_Pinjam = peminjaman.Tanggal_Pinjam,
                        Tanggal_Kembali = peminjaman.Tanggal_Kembali
                    };
                    _context.RiwayatPeminjaman.Add(Peminjaman);
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

        //  UPDATE/PUT Peminjaman
        public int Put(int Id, Peminjaman peminjaman)
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
                    //Update barang_id atau karyawan_id
                    if (riwayatPeminjaman.Karyawan_Id != peminjaman.Karyawan_Id)
                    {
                        riwayatPeminjaman.Karyawan_Id = peminjaman.Karyawan_Id;
                    }

                    //Jika mengubah barang yg dipinjam, tetapi jumlahnya tetap
                    if (riwayatPeminjaman.Barang_Id != peminjaman.Barang_Id && riwayatPeminjaman.Jumlah == peminjaman.Jumlah)
                    {
                        riwayatPeminjaman.Barang_Id = peminjaman.Barang_Id;

                        var barang = _context.Barang.Find(riwayatPeminjaman.Barang_Id);

                        //Kembalikan stok di Tb Barang sesuai jumlah yg dipinjam sebelumnya
                        barang.Stok += riwayatPeminjaman.Jumlah;
                        _context.Barang.Update(barang);
                        result += _context.SaveChanges(); result += _context.SaveChanges();
                        //Ubah stok di barang lainnya sesuai barang_id di request body
                        barang = _context.Barang.Find(peminjaman.Barang_Id);
                        barang.Stok += riwayatPeminjaman.Jumlah;
                        _context.Barang.Update(barang);
                        result += _context.SaveChanges();
                    }
                    //Jika mengubah barang dan jumlah yg dipinjam
                    else if (riwayatPeminjaman.Barang_Id != peminjaman.Barang_Id && riwayatPeminjaman.Jumlah != peminjaman.Jumlah)
                    {
                        riwayatPeminjaman.Barang_Id = peminjaman.Barang_Id;

                        var barang = _context.Barang.Find(peminjaman.Barang_Id);

                        //Kembalikan stok di Tb Barang sesuai jumlah yg dipinjam sebelumnya
                        barang.Stok += riwayatPeminjaman.Jumlah;
                        _context.Barang.Update(barang);
                        result += _context.SaveChanges();
                        //Ubah stok di barang lainnya sesuai barang_id di request body
                        barang = _context.Barang.Find(peminjaman.Barang_Id);
                        barang.Stok += peminjaman.Jumlah;
                        _context.Barang.Update(barang);
                        result += _context.SaveChanges();
                        //Ubah jumlah di riwayatPeminjaman sesuai jumlah terbaru di request body
                        riwayatPeminjaman.Jumlah = peminjaman.Jumlah;
                    }
                    else if (peminjaman.Barang_Id == peminjaman.Barang_Id && riwayatPeminjaman.Jumlah != peminjaman.Jumlah)
                    {
                        //Ubah jumlah di riwayatPeminjaman sesuai jumlah terbaru di request body
                        riwayatPeminjaman.Jumlah = peminjaman.Jumlah;
                    }
                    
                    if(peminjaman.Status=="KEMBALI" && peminjaman.Status != riwayatPeminjaman.Status)
                    {
                        riwayatPeminjaman.Status = peminjaman.Status;
                    }

                    riwayatPeminjaman.Tanggal_Pinjam = peminjaman.Tanggal_Pinjam;
                    riwayatPeminjaman.Tanggal_Kembali = peminjaman.Tanggal_Kembali;

                    _context.RiwayatPeminjaman.Update(riwayatPeminjaman);
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

            #endregion
       
    }
}
