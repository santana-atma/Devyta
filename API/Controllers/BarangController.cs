using API.Models;
using API.Repositories.Data;
using API.ViewModels;
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
    public class BarangController : ControllerBase
    {
        BarangRepository barangRepository;

        public BarangController(BarangRepository barangRepository)
        {
            this.barangRepository = barangRepository;
        }

        // GET: api/Barang
        [HttpGet]
        public IActionResult Get()
        {
            var data = barangRepository.Get();
            return Ok(new { statusCode = 200, message = "List semua aset", data = data });
        }

        // GET: api/Barangs/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = barangRepository.Get(id);
            if(data != null)
                return Ok(new { statusCode = 200, message = "Detail aset", data = data });
            return NotFound(new { statusCode = 404, message = "Aset tidak ditemukan" });
        }
      
        [HttpPost]
        public IActionResult Post(BarangVM barang)
        {
            var result = barangRepository.Post(barang);
            if (result > 0)
                return Ok(new { statusCode = 200, message = "Barang berhasil ditambah" });
            return BadRequest(new { statusCode = 400, message = "Barang gagal ditambah" });
        }
        // Put: api/Barangs/5
        [HttpPut("{id}")]
        public IActionResult Put(Barang barang)
        {
            var result = barangRepository.Put(barang);
            if (result > 0)
                return Ok(new { statusCode = 200, message = "Barang berhasil diubah" });
            return BadRequest(new { statusCode = 400, message = "Barang gagal diubah" });
        }
        // Delete: api/Barangs/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Barang barang)
        {
            var result = barangRepository.Delete(barang.Id);
            if (result > 0)
                return Ok(new { statusCode = 200, message = "Barang berhasil dihapus" });
            return BadRequest(new { statusCode = 400, message = "Barang gagal dihapus" });
        }
    }
}
