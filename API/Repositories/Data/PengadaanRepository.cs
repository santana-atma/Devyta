using API.Context;
using API.Models;
using API.Repositories.Interface;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class PengadaanRepository : IPengadaanRepository
    {
        MyContext myContext;

        public PengadaanRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public int Delete(int id)
        {
            var data = myContext.RiwayatPengadaan.Find(id);
            var barang = myContext.Barang.FirstOrDefault(x => x.Id == data.Barang_Id);
            barang.Stok -= data.Jumlah;
            myContext.Barang.Update(barang);
            myContext.SaveChanges();
            myContext.RiwayatPengadaan.Remove(data);
            var result = myContext.SaveChanges();
            return result;
        }

        public List<RiwayatPengadaan> Get()
        {
            var data = myContext.RiwayatPengadaan.Include(x => x.Barang).ToList();
            return data;
        }

        public RiwayatPengadaan Get(int id)
        {
            var data = myContext.RiwayatPengadaan.Include(x => x.Barang).FirstOrDefault(x=> x.Id == id);
            return data;
        }

        public int Post(PengadaanVM pengadaan)
        {
            using var transaction = myContext.Database.BeginTransaction();
            var result = 0;
            int id;
            try
            {
                var data = myContext.UserRole.Where(x => x.Id == pengadaan.PetugasId).FirstOrDefault();     
                if (data != null && data.Role_Id == 1 )
                {
                    var isExist = myContext.Barang.Where(x => x.Nama == pengadaan.Nama).FirstOrDefault();
                    if (isExist != null)
                    {
                        isExist.Stok = isExist.Stok + pengadaan.Jumlah;
                        myContext.Barang.Update(isExist);
                        result += myContext.SaveChanges();
                        id = isExist.Id;
                    }
                    else
                    {
                        myContext.Barang.Add(new Barang { Nama = pengadaan.Nama, Satuan = pengadaan.Satuan, Stok = pengadaan.Jumlah });
                        result += myContext.SaveChanges();
                        id = myContext.Barang.OrderByDescending(x => x.Id).First().Id;

                    }
                    myContext.RiwayatPengadaan.Add(new RiwayatPengadaan { Barang_Id = id, Supplier = pengadaan.Supplier, Tanggal = pengadaan.Tanggal, Jumlah = pengadaan.Jumlah, Harga = pengadaan.Harga });
                    result += myContext.SaveChanges();
                    transaction.Commit();
                }
                else
                {
                    return 0;
                }


            }
            catch (Exception)
            {
                transaction.Rollback();
                result = 0;
            }
            return result;
        }

        public int Put(int Id,PengadaanVM pengadaan)
        {
            using var transaction = myContext.Database.BeginTransaction();
            var result = 0;
            int id;
            try
            {
                var data = myContext.UserRole.Where(x => x.Id == pengadaan.PetugasId).FirstOrDefault();
                if(data != null && data.Role_Id == 1)
                {
                    var isExist = myContext.RiwayatPengadaan.Find(Id);
                    if (isExist != null)
                    {
                        var barang = myContext.Barang.FirstOrDefault(x => x.Id == data.Barang_Id);
                        isExist.Tanggal = pengadaan.Tanggal;
                        barang.Stok = barang.Stok - isExist.Jumlah + pengadaan.Jumlah;
                        isExist.Jumlah = pengadaan.Jumlah;
                        isExist.Harga = pengadaan.Harga;
                        isExist.Supplier = pengadaan.Supplier;
                        myContext.Barang.Update(barang);
                        myContext.SaveChanges();
                        myContext.RiwayatPengadaan.Update(isExist);
                        result += myContext.SaveChanges();
                        transaction.Commit();
                    }
                }          
                else
                {
                    return 0;
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
