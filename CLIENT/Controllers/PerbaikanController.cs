using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLIENT.Controllers
{
    public class PerbaikanController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != null && role.Equals("Staff"))
            {
                return View();

            }
            return RedirectToAction("Unauthorized", "Error");
        }
        
        public IActionResult TambahPerbaikan()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != null && role.Equals("Staff"))
            {
                return View();

            }
            return RedirectToAction("Unauthorized", "Error");
        }
        public IActionResult Admin()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != null && role.Equals("Admin"))
            {
                return View();

            }
            return RedirectToAction("Unauthorized", "Error");
        }
    }
}
