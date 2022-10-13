using API.Context;
using API.Models;
using API.Repositories.Interface;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class DashboardRepository : IDashboardRepository
    {
        MyContext myContext;

        public DashboardRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public List<ResponseDashboard> GetAllData()
        {
            List<ResponseDashboard> responseDashboards = new List<ResponseDashboard>();
            var Top5Aset = myContext.Barang.Take(5).ToList();
            foreach(Barang aset in Top5Aset)
            {
                var DataPerbaikan = myContext.RiwayatPerbaikan
                                        .Where(x => x.Barang_Id == aset.Id && x.Status == "DIPERIKSA").ToList();
                var Total_Perbaikan = 0;
                foreach (RiwayatPerbaikan riwayatPerbaikan in DataPerbaikan)
                {
                    Total_Perbaikan += riwayatPerbaikan.Jumlah;
                }

                var DataPeminjam = myContext.RiwayatPeminjaman
                                        .Where(x => x.Barang_Id == aset.Id && x.Status == "PINJAM").ToList();
                int Total_Peminjaman = 0;
                foreach(RiwayatPeminjaman riwayatPeminjaman in DataPeminjam)
                {
                    Total_Peminjaman += riwayatPeminjaman.Jumlah;
                }
                ResponseDashboard responseDashboard = new ResponseDashboard()
                {
                    NamaAset = aset.Nama,
                    Total_Keseluruhan = aset.Stok,
                    Total_Peminjaman = Total_Peminjaman,
                    Total_Perbaikan = Total_Perbaikan
                };
                responseDashboards.Add(responseDashboard);
            }
            
            return responseDashboards;
        }

        public ResponseTotalAngka GetTotal()
        {
            var totalAset = myContext.Barang.ToList().Count();
            var totalPeminjaman = myContext.RiwayatPeminjaman.ToList().Count();
            var totalAdmin = myContext.UserRole.Where(x => x.Role_Id == 1).Count();
            var totalStaff = myContext.UserRole.Where(x => x.Role_Id == 2).Count();
            ResponseTotalAngka responseTotalAngka = new ResponseTotalAngka()
            {
                TotalAdmin = totalAdmin,
                TotalStaff = totalStaff,
                TotalPeminjaman = totalPeminjaman,
                TotalAset = totalAset
            };
            return responseTotalAngka;
        }
    }
}
