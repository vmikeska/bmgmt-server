using builder_mgmt_server.Controllers.ChatMessages;
using builder_mgmt_server.Database;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Models;
using builder_mgmt_server.Models.ChatMessages;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    [ApiController] 
    public class BaseChatMessagesController<TM, TE> : BaseApiController
        where TM : ChatMessagesModelBase<TE>
        where TE : BaseChatMessagesEntity, new()
    {
        public TM ChatModel;

        public BaseChatMessagesController(IDbOperations db) : base(db)
        {
            
        }

        [HttpGet]
        [AuthorizeApi]
        public ApiResult GetMessages(string id)
        {
            var topicId = new ObjectId(id);            
            var res = GetMessagesById(topicId);

            return ResponseHelper.Successful(res);
        }

        [HttpPost]
        [AuthorizeApi]
        public async Task<ApiResult> AddMessage([FromBody] NewChatMessageRequest req)
        {
            var topicId = new ObjectId(req.topicId);
            var msg = new NewChatMessageDO()
            {
                Id = ObjectId.GenerateNewId(),
                AuthorId = UserIdObj,
                TopicId = topicId,
                Text = req.text
            };

            var e = await ChatModel.AddMessage(msg);

            return ResponseHelper.Successful(e.id);
        }

        [HttpDelete]
        [AuthorizeApi]        
        public async Task<ApiResult> DeleteMessage(string id, string topicId)
        {
            var tid = new ObjectId(topicId);
            var mid = new ObjectId(id);

            await ChatModel.DeleteMessage(tid, mid);

            return ResponseHelper.Successful(true);
        }

        private List<ChatMessageResponse> GetMessagesById(ObjectId topicId)
        {
            var messages = ChatModel.GetMessages(topicId);

            var res = messages.Select(i =>
            {
                var r = new ChatMessageResponse()
                {
                    id = i.Id,
                    authorId = i.AuthorId,
                    fullName = i.FullName,
                    posted = i.Posted,
                    text = i.Text
                };

                return r;
            }).ToList();

            return res;
        }

    }

}
