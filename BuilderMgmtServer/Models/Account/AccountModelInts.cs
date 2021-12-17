using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.Account
{
    public class NewAccountDO
    {
        public string Mail;
        public string Password;

        //currently for dummy accounts creation
        public string FirstName;
        public string LastName;
    }

    public interface IAccountModel
    {

    }



}

