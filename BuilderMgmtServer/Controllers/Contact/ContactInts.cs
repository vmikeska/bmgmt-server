using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers.User
{
    public class NewContactResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool alreadyAdded { get; set; }
        public bool stared { get; set; }
    }

    public class BindingChangeRequest
    {
        public string contactId { get; set; }
    }

}
