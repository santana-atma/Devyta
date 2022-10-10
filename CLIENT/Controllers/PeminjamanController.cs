using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLIENT.Controllers
{
    public class PeminjamanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult RiwayatPeminjaman()
        {
            return View();
        }
    }
}
