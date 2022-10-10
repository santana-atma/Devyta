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
        ResponseDetailAset GetDetails(int id);
        Barang Get(int id);
        int Post(BarangVM barang);
        int Put(int id, BarangVM barang);

        int Delete(int id);
    }
}
