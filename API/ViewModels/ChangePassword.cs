using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class ChangePassword
    {
        public Login login { get; set; }
        public string newPassword { get; set; }
    }
}
