using builder_mgmt_server.Database;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{

    [Route("api/[controller]")]    
    public class BaseApiController : Controller
    {
        public IDbOperations DB { get; set; }

        public BaseApiController(IDbOperations db)
        {
            DB = db;
        }

        public string UserId { get; set; }

        public ObjectId UserIdObj
        {
            get
            {
                return new ObjectId(UserId);
            }
        }
    }

    public static class ResponseHelper
    {
        public static ApiResult Successful(object data)
        {
            var res = new ApiResult { data = data, result = ApiResultCode.OK };
            return res;
        }

        public static ApiResult Error(string data)
        {
            var res = new ApiResult { data = data, result = ApiResultCode.ERROR };
            return res;
        }

        public static ApiResult EmptyArray()
        {
            var res = new ApiResult { data = new List<Object>(), result = ApiResultCode.OK };
            return res;
        }
    }
}
