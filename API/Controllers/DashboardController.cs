using API.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        DashboardRepository dashboardRepository;
        public DashboardController(DashboardRepository dashboardRepository)
        {
            this.dashboardRepository = dashboardRepository;
        }
        [HttpGet]
        public IActionResult GetAllData()
        {
            var data = dashboardRepository.GetAllData();
            return Ok(new { StatusCode = 200, message = "Total info data aset", data = data });
        }

        [HttpGet]
        [Route("TotalData")]
        public IActionResult GetTotal()
        {
            var data = dashboardRepository.GetTotal();
            return Ok(new { StatusCode = 200, message = "Total data :", data = data });
        }
    }
}
