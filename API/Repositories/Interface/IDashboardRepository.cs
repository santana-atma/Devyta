using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Interface
{
    public interface IDashboardRepository
    {
         List<ResponseDashboard> GetAllData();
    }
}
