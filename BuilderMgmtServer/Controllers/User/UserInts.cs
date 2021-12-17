using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers.User
{
    public class UserResponse
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string desc { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string website { get; set; }
        public PlaceLocationResponse location { get; set; }

    }

    public class UpdatePropRequest
    {
        public string item { get; set; }
        public string value { get; set; }
    }

    public class PlaceLocationResponse
    {
        public string text { get; set; }
        public List<double> coords { get; set; }
    }

    public class UserRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string desc { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string website { get; set; }
        public PlaceLocationResponse location { get; set; }
    }


}
