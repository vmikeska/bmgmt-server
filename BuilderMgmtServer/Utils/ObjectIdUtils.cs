using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Utils
{
    public static class ObjectIdUtils
    {
        public static string SafeString(ObjectId oid)
        {
            if (oid == null)
            {
                return null;
            }

            var isEmpty = ObjectId.Empty == oid;
            if (isEmpty)
            {
                return null;
            }

            return oid.ToString();
        }
    }
}
