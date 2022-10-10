using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLIENT.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != null)
            {
                return View();

            }
            return RedirectToAction("Unauthorized", "Error");
            
        }
    }
}
