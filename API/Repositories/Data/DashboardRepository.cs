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
                var Total_Perbaikan = myContext.RiwayatPerbaikan
                                        .Where(x => x.Barang_Id == aset.Id && x.Status == "DIPERIKSA").Count();
                var Total_Peminjaman = myContext.RiwayatPeminjaman
                                        .Where(x => x.Barang_Id == aset.Id && x.Status == "PINJAM").Count();
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
    }
}
