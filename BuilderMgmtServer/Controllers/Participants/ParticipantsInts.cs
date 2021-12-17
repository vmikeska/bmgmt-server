using builder_mgmt_server.DOs;
using builder_mgmt_server.Enums;
using System.Collections.Generic;

namespace builder_mgmt_server.Controllers
{
    public class TopicNewParticipantRequest
    {
        public string topicId { get; set; }

        public string userId { get; set; }

        public TopicParticipantEnum role { get; set; }
    }

    public class TopicParticipantResponse
    {
        public string bindingId { get; set; }

        public TopicParticipantEnum role { get; set; }

        public string userId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }


    }

}