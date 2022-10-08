using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class Hashing
    {
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(5);
        } 
        public static string PasswordHashing(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());

        }
        public static bool PasswordVerify(string password, string hashPassword)
        {
            //Console.WriteLine($"plain password : {password}");
            //Console.WriteLine($"hash  password : {PasswordHashing(password)}");
            //Console.WriteLine($"db    password : {hashPassword}");
            //Console.WriteLine();

            return BCrypt.Net.BCrypt.Verify(password, hashPassword);

        }
    }
}
