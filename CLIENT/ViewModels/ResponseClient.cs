using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLIENT.ViewModels
{
    public class ResponseClient
    {
        public string message { get; set; }
        public int status { get; set; }
        public ResponseLogin data { get; set; }

    }
}
