using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Interface
{
    interface IPeminjamanRepository
    {
        List<RiwayatPeminjaman> Get();
        RiwayatPeminjaman Get(int Id);
        int Post(Peminjaman peminjaman);
        int Put(int Id, Peminjaman peminjaman);
        int Delete(int Id);

    }
}
