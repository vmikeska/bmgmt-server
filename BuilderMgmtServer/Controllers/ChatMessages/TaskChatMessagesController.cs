using builder_mgmt_server.Database;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Models;
using builder_mgmt_server.Models.ChatMessages;
using Microsoft.AspNetCore.Mvc;

namespace builder_mgmt_server.Controllers
{
    [Route("api/task-chat")]
    public class TaskChatMessagesController : 
        BaseChatMessagesController<TaskChatMessagesModel, TaskChatMessagesEntity>
    {

        public TaskChatMessagesController(ITaskChatMessagesModel chatModel, IDbOperations db) : base(db)
        {
            ChatModel = (TaskChatMessagesModel)chatModel;
        }

    }

}
