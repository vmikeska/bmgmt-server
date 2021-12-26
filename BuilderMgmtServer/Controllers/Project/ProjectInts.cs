using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Enums;
using System.Collections.Generic;

namespace builder_mgmt_server.Controllers
{

    public class ProjectDetailResponse
    {
        public ProjectResponse project { get; set; }

        public List<ParticipantsOverviewReponse> participants { get; set; }
    }

    public class ProjectResponse
    {
        public string id { get; set; }

        public string name { get; set; }

        public string desc { get; set; }

        public PlaceLocationResponse location { get; set; }

    }

    public class ProjectTaskBindingRequest
    {
        public string projId { get; set; }

        public string taskId { get; set; }
    }

}