using API.Context;
using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class BarangRepository
    {
        private readonly MyContext _context;

        public BarangRepository(MyContext context)
        {
            _context = context;
        }

        //READ/GET All Peminjaman
        public List<RiwayatPeminjaman> Get()
        {
            return _context.RiwayatPeminjaman.ToList();
        }

        //READ/GET By Id Peminjaman
        public RiwayatPeminjaman Get(int Barang_Id, int Karyawan_Id)
        {
            return _context.RiwayatPeminjaman.Where(x => x.Karyawan_Id == Karyawan_Id && x.Barang_Id == Barang_Id).FirstOrDefault();
        }


        //  CREATE/POST Peminjaman
        public int Create(Peminjaman peminjaman)
        {

            using var transaction = _context.Database.BeginTransaction();
            var result = 0;
            try
            {
                //Cek apakah masih ada pinjaman dengan barang dan peminjam yang sama dengan status belum kembali, jika True maka belum bisa pinjam
                if (_context.RiwayatPeminjaman.Where(x => x.Karyawan_Id == peminjaman.Karyawan_Id && x.Status == "Pinjam" && x.Barang_Id == peminjaman.Barang_Id).FirstOrDefault() != null)
                {
                    return 0;
                }
                else
                {
                    var isExist = _context.Barang.Find(peminjaman.Barang_Id);
                    if (isExist != null)
                    {
                        isExist.Stok = isExist.Stok - peminjaman.Jumlah;
                        _context.Barang.Update(isExist);
                        result += _context.SaveChanges();

                    }
                    else
                    {
                        return 0;

                    }

                    DateTime defaultTglKembali = DateTime.Parse("0000-00-00");
                    var Peminjaman = new RiwayatPeminjaman()
                    {
                        Barang_Id = peminjaman.Barang_Id,
                        Karyawan_Id = peminjaman.Karyawan_Id,
                        Jumlah = peminjaman.Jumlah,
                        Status = "Pinjam",
                        Tanggal_Pinjam = peminjaman.Tanggal_Pinjam,
                        Tanggal_Kembali = (DateTime)(peminjaman.Tanggal_Kembali == null ? defaultTglKembali : peminjaman.Tanggal_Kembali)
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

        //  UPDATE Peminjaman
        public int Update(int Barang_Id, int Karyawan_Id, Peminjaman peminjaman)
        {

            using var transaction = _context.Database.BeginTransaction();
            var result = 0;
            try
            {
                var riwayatPeminjaman = _context.RiwayatPeminjaman.Where(x => x.Karyawan_Id == Karyawan_Id && x.Barang_Id == Barang_Id).FirstOrDefault();
                //Cek apakah ada pinjaman dengan barang_id dan peminjam_id yang sama, jika Not Null maka bisa Update
                if (riwayatPeminjaman == null)
                {
                    return 0;
                }
                else
                {
                    //Update barang_id atau karyawan_id
                    if (Karyawan_Id != peminjaman.Karyawan_Id)
                    {
                        riwayatPeminjaman.Karyawan_Id = peminjaman.Karyawan_Id;                            
                    }

                    //Jika mengubah barang yg dipinjam, tetapi jumlahnya tetap
                    if (Barang_Id != peminjaman.Barang_Id && riwayatPeminjaman.Jumlah == peminjaman.Jumlah)
                    {
                        riwayatPeminjaman.Barang_Id = peminjaman.Barang_Id;

                        var barang = _context.Barang.Find(Barang_Id);

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
                    else if (Barang_Id != peminjaman.Barang_Id && riwayatPeminjaman.Jumlah != peminjaman.Jumlah)
                    {
                        riwayatPeminjaman.Barang_Id = peminjaman.Barang_Id;

                        var barang = _context.Barang.Find(Barang_Id);

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
                    else if(Barang_Id == peminjaman.Barang_Id && riwayatPeminjaman.Jumlah != peminjaman.Jumlah)
                    {
                        //Ubah jumlah di riwayatPeminjaman sesuai jumlah terbaru di request body
                        riwayatPeminjaman.Jumlah = peminjaman.Jumlah;
                    }

                    riwayatPeminjaman.Tanggal_Pinjam = peminjaman.Tanggal_Pinjam;
                    riwayatPeminjaman.Tanggal_Kembali = ((DateTime)(peminjaman.Tanggal_Kembali != null ? peminjaman.Tanggal_Kembali : riwayatPeminjaman.Tanggal_Kembali));

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
        public int Delete(int Barang_Id, int Karyawan_Id)
        {

            using var transaction = _context.Database.BeginTransaction();
            var result = 0;
            try
            {
                var riwayatPeminjaman = _context.RiwayatPeminjaman.Where(x => x.Karyawan_Id == Karyawan_Id && x.Barang_Id == Barang_Id).FirstOrDefault();
                
                //Cek apakah ada pinjaman dengan barang_id dan peminjam_id yang sama, jika Not Null maka bisa Update
                if (riwayatPeminjaman == null)
                {
                    return 0;
                }
                else
                {
                    var barang = _context.Barang.Find(Barang_Id);

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





    }
}
