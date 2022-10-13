using CLIENT.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CLIENT.Controllers
{
    public class AccountController : Controller
    {
        string address;
        HttpClient HttpClient;
        public AccountController()
        {
            this.address = "https://localhost:44307/api/Login";
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(address)
            };
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var objLogin = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(objLogin, Encoding.UTF8, "application/json");
            var result = HttpClient.PostAsync(address, content).Result;
            if (result.IsSuccessStatusCode)
            {
                var resultContent = await result.Content.ReadAsStringAsync();
                var data = new ResponseClient();
                data = JsonConvert.DeserializeObject<ResponseClient>(resultContent);
                HttpContext.Session.SetString("Role", data.data.Role);
                HttpContext.Session.SetString("User", data.data.FullName);
                HttpContext.Session.SetString("UserId", data.data.Id.ToString());
                if (HttpContext.Session.GetString("Role").Equals("Admin")){
                    return (RedirectToAction("Index", "Dashboard"));
                }
                else
                {
                    return (RedirectToAction("Index", "Dashboard"));
                }
                
            }
            ViewBag.Message = "Wrong email or password";
            return View("Login", "Account");
        }

        public IActionResult UserPanel()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != null)
            {
                return View("UserPanel","Account");

            }
            return RedirectToAction("Unauthorized","Error");
        }



    }
}
