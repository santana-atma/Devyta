using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Interface
{
    public interface IBarangRepository
    {
        List<Barang> Get();
        Barang Get(int id);
        int Post(BarangVM barang);
        int Put(Barang barang);
    }
}
