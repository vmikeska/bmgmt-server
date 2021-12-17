using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    public class UserIdService : IUserIdService
    {
        private string idStr;

        public string IdStr
        {
            get
            {
                return idStr;
            }

            set
            {
                idStr = value;
            }
        }

        public ObjectId IdObj
        {
            get
            {
                return new ObjectId(IdStr);
            }
        }

        
    }

    public interface IUserIdService
    {
        public string IdStr { get; set; }

        public ObjectId IdObj { get; }

    }
}