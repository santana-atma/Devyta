using API.Repositories.Data;
using API.ViewModels;
using Microsoft.AspNetCore.Cors;
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
    [EnableCors("AllowOrigin")]

    public class AccountController : ControllerBase
    {
        AccountRepository accountRepository;

        public AccountController(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }


        [HttpPost("~/api/register")]
        public IActionResult RegisterAccount(RegisterAccount account)
        {
            var data = accountRepository.Register(account);
            if (data)
            {
                return Ok(new { message = "sukses membuat akun baru !", statusCode = 201});
            }
            return BadRequest(new { message = "gagal membuat akun !", statusCode = 400 });
        }


        [HttpPost("~/api/login")]
        public IActionResult Login(Login login)
        {
            var data = accountRepository.Login(login);
            if (data != null)
            {
                return Ok(new { message = "sukses login !", statusCode = 201, data = data });
            }
            return BadRequest(new { message = "email dan password salah !!", statusCode = 400 });
        }


        [HttpPost("~/api/changepasswd")]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            var data = accountRepository.ChangePassword(changePassword);
            if (data)
            {
                return Ok(new { message = "sukses mengganti password !", statusCode = 201 });
            }
            return BadRequest(new { message = "password atau email tidak cocok !!", statusCode = 400 });
        }


        [HttpGet("~/api/karyawan/")]
        public IActionResult Get()
        {
            var data = accountRepository.Get();
            return Ok(new { message = "sukses mendapatkan akun !", statusCode = 201, data = data });

        }


        [HttpGet("~/api/karyawan/{id:int}")]
        public IActionResult Get(int id)
        {
            var data = accountRepository.Get();
            return Ok(new { message = "sukses mendapatkan akun !", statusCode = 201, data = data });
        }

        [HttpGet("~/api/roles")]
        public IActionResult Role()
        {
            var data = accountRepository.GetRoles();
            return Ok(new { message = "sukses mendapatkan info roles !", statusCode = 201, data = data });
        }


        [HttpDelete("~/api/karyawan/{id:int}")]
        public IActionResult Delete(int id)
        {
            var data = accountRepository.Delete(id);
            if (data)
            {
                return Ok(new { message = "sukses menghapus akun !", statusCode = 201 });
            }
            return BadRequest(new { message = "gagal menghapus akun !", statusCode = 400 });

        }

        [HttpPut("~/api/karyawan/")]
        public IActionResult Put(UpdateAccount updateAccount)
        {
            var data = accountRepository.UpdateAccount(updateAccount);
            if (data)
            {
                return Ok(new { message = "sukses mengupdate akun !", statusCode = 201 });
            }
            return BadRequest(new { message = "gagal mengupdate akun !", statusCode = 400 });

        }
    }
}
