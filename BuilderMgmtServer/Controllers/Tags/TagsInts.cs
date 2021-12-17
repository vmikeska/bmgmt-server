using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers.Tags
{
    public class TagResponse
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class TagBindingResponse
    {
        public string tagId { get; set; }
        public string bindingId { get; set; }
        public string name { get; set; }
    }

    public class NewTagBindingResponse
    {
        public string tagId { get; set; }
        public string entityId { get; set; }
    }

    public class SearchTagResponse
    {
        public string str { get; set; }
        public string entityId { get; set; }
    }
}
