using builder_mgmt_server.Database;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Models.Account;
using builder_mgmt_server.Models.ChatMessages;
using builder_mgmt_server.Models.Tasks;
using builder_mgmt_server.Models.TasksBusyness;
using builder_mgmt_server.Models.User;
using builder_mgmt_server.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models
{

    public class ProjectChatMessagesModel :
        ChatMessagesModelBase<ProjectChatMessagesEntity>,
        IProjectChatMessagesModel
    {        
        public ProjectChatMessagesModel(IDbOperations db, IUserModel userModel) : base(db, userModel)
        {
        }
    }
}
