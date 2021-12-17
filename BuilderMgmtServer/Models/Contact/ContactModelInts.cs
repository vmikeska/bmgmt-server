using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.Contact
{
    public interface IContactModel
    {
    }

    public class ContactResultsDO
    {
        public List<ContactResultDO> Native;
        public List<ContactResultDO> Custom;

    }

    public class ContactResultDO
    {
        public string Id;
        public ContactTypeEnum Type;
        public string Name;
        public bool AlreadyAdded;
        public bool Stared;
    }

    public enum ContactTypeEnum { Native, Custom }

    public class NewContactResultDO
    {
        public ObjectId UserId;

        public string FirstName;
        public string LastName;
    }
}
