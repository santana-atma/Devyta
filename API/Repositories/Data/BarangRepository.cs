using API.Context;
using API.Models;
using API.Repositories.Interface;
using API.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class BarangRepository : IBarangRepository
    {
        MyContext myContext;

        public BarangRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public int Delete(int Id)
        {
            var data = Get(Id);
            if (data != null && data.Stok == 0)
            {
                myContext.Barang.Remove(data);
                var result = myContext.SaveChanges();
                return result;
            }   
            return -1;
        }

        public List<Barang> Get()
        {
            var data = myContext.Barang.ToList();
            return data;
        }
        public Barang Get(int id)
        {
            var data = myContext.Barang.Find(id);
            return data;
        }

        public ResponseDetailAset GetDetails(int id)
        {
            var barang = myContext.Barang.Find(id);
            var listRiwayat = myContext.RiwayatPengadaan.Where(x => x.Barang_Id == barang.Id).ToList();
            ResponseDetailAset result = new ResponseDetailAset { Barang = barang, Riwayat_Pengadaan = listRiwayat };
            return result;
        }

        public int Post(BarangVM barang)
        {
            var listAset = myContext.Barang.ToList();
            foreach(Barang aset in listAset)
            {
                if (aset.Nama.ToLower().Equals(barang.Nama.ToLower()))
                    return -1;
            }
            myContext.Barang.Add(new Barang { Nama = barang.Nama, Satuan = barang.Satuan });
            var result = myContext.SaveChanges();
            return result;
        }

        public int Put(int id, BarangVM barang)
        {
            var data = Get(id);
            if (data == null)
                return -1;
            data.Nama = barang.Nama;
            data.Satuan = barang.Satuan;
            myContext.Barang.Update(data);
            var result = myContext.SaveChanges();
            return result;
        }
      
    }
}
