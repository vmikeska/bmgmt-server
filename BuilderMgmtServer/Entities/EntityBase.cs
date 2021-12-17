using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Database
{
    public class EntityBase
    {
        public ObjectId id { get; set; }
    }
}
