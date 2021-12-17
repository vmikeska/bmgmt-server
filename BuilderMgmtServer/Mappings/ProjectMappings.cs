using builder_mgmt_server.Controllers;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Utils;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Mappings
{
    public class TaskMappings
    {
        public static TaskResponse FromEntityToRes(TaskEntity e)
        {
            var res = new TaskResponse()
            {
                id = e.id.ToString(),
                ownerId = ObjectIdUtils.SafeString(e.owner_id),                
                name = e.name,
                desc = e.desc,
                type = e.type,
                dateFrom = e.dateFrom.HasValue ? e.dateFrom.Value.ToIsoDateString() : null,
                dateTo = e.dateTo.HasValue ? e.dateTo.Value.ToIsoDateString() : null,
                manDays = e.manDays,
                manHours = e.manHours,
                month = e.month,
                week = e.week,
                year = e.year
            };
            return res;
        }
    }
}
