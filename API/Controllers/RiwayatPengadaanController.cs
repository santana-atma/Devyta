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
    public class RiwayatPengadaanController : ControllerBase
    {
        PengadaanRepository pengadaanRepository;

        public RiwayatPengadaanController(PengadaanRepository pengadaanRepository)
        {    
            this.pengadaanRepository = pengadaanRepository;
        }


        // GET: api/RiwayatPengadaan
        [HttpGet]
        public IActionResult GetRiwayatPengadaan()
        {
            var data = pengadaanRepository.Get();
            return Ok(new { statusCode = 200, message = "List semua riwayat pengadaan aset", data = data });
        }

        // GET: api/RiwayatPengadaan/5
        [HttpGet("{id}")]
        public IActionResult GetRiwayatPengadaan(int id)
        {
            var data = pengadaanRepository.Get(id);
            if (data != null)
                return Ok(new { statusCode = 200, message = "Detail riwayat pengadaan aset", data = data });
            return NotFound(new { statusCode = 404, message = "Riwayat pengadaan aset tidak ditemukan" });
        }

        [HttpPost]
        public IActionResult Post(PengadaanVM pengadaan)
        {
            var result = pengadaanRepository.Post(pengadaan);
            if (result > 0)
            {
                return Ok(new { message = "Sukses tambah data", statusCode = 200 });
            }
            else
            {
                return BadRequest(new { message = "Gagal tambah data", statusCode = 400 });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(RiwayatPengadaan pengadaan)
        {
            var result = pengadaanRepository.Put(pengadaan);
            if (result > 0)
            {
                return Ok(new { message = "Sukses perbarui data", statusCode = 200 });
            }
            else
            {
                return BadRequest(new { message = "Gagal perbarui data", statusCode = 400 });
            }
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(RiwayatPengadaan pengadaan)
        {
            var result = pengadaanRepository.Delete(pengadaan.Id);
            if (result > 0)
            {
                return Ok(new { message = "Sukses hapus data", statusCode = 200 });
            }
            else
            {
                return BadRequest(new { message = "Gagal hapus data", statusCode = 400 });
            }
        }
    }
}
