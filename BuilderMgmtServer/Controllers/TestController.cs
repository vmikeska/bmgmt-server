using builder_mgmt_server.Controllers;
using builder_mgmt_server.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_server_test.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : BaseApiController
    {
        public TestController(IDbOperations db) : base(db)
        {            
        }

        [HttpGet]
        public string Get()
        {
            return "test je ok 444";
        }
    }
}
