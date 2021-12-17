using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    public class ApiResult
    {
        public ApiResultCode result { get; set; }
        public string responseMessage { get; set; }
        public object data { get; set; }
    }

    public enum ApiResultCode { OK, WARNING, ERROR }
}
