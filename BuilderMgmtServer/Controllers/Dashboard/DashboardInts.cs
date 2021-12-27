using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Enums;
using System.Collections.Generic;

namespace builder_mgmt_server.Controllers
{
    public class DashTaskResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public TaskTypeEnum @type { get; set; }
        public bool isOwner { get; set; }
    }

    public class DashProjResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool isOwner { get; set; }
    }

    public class DashResultResponse
    {
        public List<DashTaskResponse> tasks { get; set; }
        public List<DashProjResponse> projs { get; set; }
    }

}