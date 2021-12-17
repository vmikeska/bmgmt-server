using builder_mgmt_server.Database;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Models;
using builder_mgmt_server.Models.ChatMessages;
using Microsoft.AspNetCore.Mvc;

namespace builder_mgmt_server.Controllers
{
    [Route("api/project-chat")]
    public class ProjectChatMessagesController : 
        BaseChatMessagesController<ProjectChatMessagesModel, ProjectChatMessagesEntity>
    {

        public ProjectChatMessagesController(IProjectChatMessagesModel chatModel, IDbOperations db) : base(db)
        {
            ChatModel = (ProjectChatMessagesModel)chatModel;
        }

    }

}
