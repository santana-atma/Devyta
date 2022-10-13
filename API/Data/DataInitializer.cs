using API.Context;
using API.Models;
using API.Repositories.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class DataInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var myContext = serviceScope.ServiceProvider.GetService<MyContext>();
                myContext.Database.EnsureCreated();
                //Role 
                if (!myContext.Role.Any())
                {
                    //hati2 gak bisa insert Id krn INSERT_IDENTITY di migration di set OFF
                    myContext.Role.Add(new Role() 
                    {
                        Nama = "Admin"
                    });
                    myContext.Role.Add(new Role()
                    {
                        Nama = "Staff"
                    });
                    myContext.SaveChanges();
                }
                //Karyawan
                
                if (!myContext.Karyawan.Any() && !myContext.User.Any() && !myContext.UserRole.Any())
                    //Ada yg tau cara panggil fungsi dari repository ??
                    //biar ga perlu ngulang fungsi ini lg
                {
                    Karyawan dataKaryawan = new Karyawan
                    {
                        Fullname = "Admnistrator",
                        Email = "admin@gmail.com",
                        Alamat = "JKT",
                        Telp = "087724011077"
                    };
                    myContext.Karyawan.Add(dataKaryawan);
                    myContext.SaveChanges();

                    User userData = new User
                    {
                        Id = dataKaryawan.Id,
                        Password = Hashing.PasswordHashing("admin"),
                        Karyawan = dataKaryawan

                    };
                    myContext.User.Add(userData);
                    myContext.SaveChanges();

                    UserRole userRoleDataa = new UserRole
                    {
                        User_Id = dataKaryawan.Id,
                        Role_Id = 1,

                    };
                    myContext.UserRole.Add(userRoleDataa);
                    myContext.SaveChanges();
                }
            }
        }
    }
}
