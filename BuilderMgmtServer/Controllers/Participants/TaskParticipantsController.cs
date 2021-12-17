using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Mappings;

using builder_mgmt_server.Models.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    [ApiController]
    [Route("api/task-participant")]
    public class TaskParticipantsController : BaseParticipantsController<TaskParticipantEntity>
    {
        
        public TaskParticipantsController(IDbOperations db) : base(db)
        {
            
        }

        

    }


}
