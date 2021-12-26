using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Enums;
using System.Collections.Generic;

namespace builder_mgmt_server.Controllers
{

    public class TaskDetailResponse
    {
        public TaskResponse task { get; set; }

        public TaskProjectResponse projectBinding { get; set; }

        public List<ParticipantsOverviewReponse> participants { get; set; }
    }

    public class ParticipantsOverviewReponse
    {
        public TopicParticipantEnum type { get; set; }
        public int count { get; set; }
    }




    public class TaskProjectResponse
    {
        public bool hasProject { get; set; }
        public string projId { get; set; }
        public string projName { get; set; }
    }

    public class TaskResponse : TaskDateTypeResponse
    {        
        public string ownerId { get; set; }
        public string desc { get; set; }
        public PlaceLocationResponse location { get; set; }
    }

    public class TaskDateTypeResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public TaskTypeEnum @type { get; set; }
        public int week { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int manHours { get; set; }
        public int manDays { get; set; }
    }

    public class TaskGroupsResponse
    {
        public List<TaskResponse> dates { get; set; }
        public List<TaskResponse> months { get; set; }
        public List<TaskResponse> weeks { get; set; }
    }


}