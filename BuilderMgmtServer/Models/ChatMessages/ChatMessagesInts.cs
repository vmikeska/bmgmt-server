using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.ChatMessages
{
    public interface ITaskChatMessagesModel
    {
    }

    public interface IProjectChatMessagesModel
    {
    }

    

    public class NewChatMessageDO
    {
        public ObjectId Id;        
        public string Text;
        public ObjectId AuthorId;
        public ObjectId TopicId;
    }

    public class ChatMessageDO
    {
        public string Id;
        public string Posted;
        public string Text;
        public string AuthorId;
        public string FullName;
    }
}
