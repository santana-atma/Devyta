using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Interface
{
    interface IPerbaikanRepository
    {
        List<RiwayatPerbaikan> Get();
        RiwayatPeminjaman Get(int Id);
        int Post(Perbaikan perbaikan);
        int Put(int Id, Perbaikan perbaikan);
        int Delete(int Id);
    }
}
