using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Interface
{
    public interface IPengadaanRepository
    {
        List<RiwayatPengadaan> Get();
        RiwayatPengadaan Get(int id);
        int Post(PengadaanVM pengadaan);
        int Put(RiwayatPengadaan pengadaan);
        int Delete(int id);
    }
}
