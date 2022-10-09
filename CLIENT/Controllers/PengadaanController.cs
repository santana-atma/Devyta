using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLIENT.Controllers
{
    public class PengadaanController : Controller
    {
        public IActionResult Index()
        {
            var role =HttpContext.Session.GetString("Role");
            var UserId = HttpContext.Session.GetString("UserId");
            ViewData["sessionRole"] = role;
            ViewData["sessionUserId"] = UserId;
            return View();
        }
    }
}
