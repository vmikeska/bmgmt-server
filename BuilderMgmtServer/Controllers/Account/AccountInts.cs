using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers.User
{

    public class NewAccountRequest
    {
        public string mail { get; set; }
        public string password { get; set; }
    }

    public class LoginRequest
    {
        public string mail { get; set; }
        public string password { get; set; }
    }

    public class InfoConfigResponse
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mail { get; set; }
        public int dayWorkingHours { get; set; }
    }




}
