using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers.ChatMessages
{
    public class ChatMessageResponse
    {
        public string id { get; set; }        
        public string posted { get; set; }
        public string text { get; set; }
        public string authorId { get; set; }
        public string fullName { get; set; }
    }

    public class NewChatMessageRequest
    {
        public string text { get; set; }
        public string topicId { get; set; }
    }

    public class DeleteChatMessageRequest
    {
        public string id { get; set; }
        public string topicId { get; set; }
    }
}
