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
    //[EnableCors("AllowOrigin")]
    public class PeminjamanController : ControllerBase
    {
        PeminjamanRepository peminjamanRepository;

        public PeminjamanController(PeminjamanRepository peminjamanRepository)
        {
            this.peminjamanRepository = peminjamanRepository;
        }

        //READ
        [HttpGet]
        public IActionResult Get()
        {

            var data = peminjamanRepository.Get();

            if (data != null)
            {
                return Ok(new { message = "Sukses get", statusCode = 200, data = data });
            }
            else
            {
                return Ok(new { message = "Sukses get", statusCode = 200, data = "null" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = peminjamanRepository.Get(id);

            if (data != null)
            {
                return Ok(new { message = "Sukses get", statusCode = 200, data = data });
            }
            else
            {
                return Ok(new { message = "Sukses get", statusCode = 200, data = "null" });
            }

        }


        [HttpPost]
        public IActionResult Post(Peminjaman peminjaman)
        {
            var result = peminjamanRepository.Post(peminjaman);
            if (result > 0)
            {
                return Ok(new { message = "Sukses input peminjaman barang", statusCode = 200 });
            }
            else
            {
                return BadRequest(new { message = "Gagal peminjaman barang", statusCode = 400 });
            }

        }

        //UPDATE
        [HttpPut("{id}")]
        public IActionResult Put(int id, Peminjaman peminjaman)
        {
            var result = peminjamanRepository.Put(id, peminjaman);
            if (result > 0)
            {
                return Ok(new { message = "Sukses ubah data peminjaman", statusCode = 200 });
            }
            else
            {
                return BadRequest(new { message = "Gagal ubah data peminjaman", statusCode = 400 });
            }

        }


        //DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = peminjamanRepository.Delete(id);
            if (result > 0)
            {
                return Ok(new { message = "Sukses hapus data peminjaman", statusCode = 200 });
            }
            else
            {
                return BadRequest(new { message = "Gagal hapus data peminjaman", statusCode = 400 });
            }

        }


    }
}
