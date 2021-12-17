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

    public class ChatMessagesModelBase<T> 
        where T : BaseChatMessagesEntity, new()
    {
        public IDbOperations DB;
        public UserModel UModel;

        public ChatMessagesModelBase(
            IDbOperations db,
            IUserModel userModel
            )
        {
            DB = db;
            UModel = (UserModel)userModel;
        }

        public List<ChatMessageDO> GetMessages(ObjectId topicId)
        {
            var e = DB.FOD<T>(i => i.topic_id == topicId);

            if (e == null)
            {
                return new List<ChatMessageDO>();
            }

            var authorsIds = e.messages.Select(i => i.author_id).Distinct();

            var users = DB.List<UserEntity>(u => authorsIds.Contains(u.id)).ToList();

            var res = e.messages.Select(i =>
            {
                var user = users.Find(u => u.id == i.author_id);

                var fullName = "removed user";
                if (user != null)
                {
                    fullName = $"{user.firstName} {user.lastName}";
                }

                var itemDO = new ChatMessageDO()
                {
                    Id = i.id.ToString(),
                    AuthorId = i.author_id.ToString(),
                    FullName = fullName,
                    Posted = i.posted.ToIsoDateTimeString(),
                    Text = i.text
                };

                return itemDO;
            }).ToList();

            res = res.OrderByDescending(i => i.Posted).ToList();

            return res;
        }

        public async Task<ChatMessageSE> AddMessage(NewChatMessageDO message)
        {
            var se = new ChatMessageSE()
            {
                id = ObjectId.GenerateNewId(),
                author_id = message.AuthorId,
                posted = DateTime.UtcNow,
                text = message.Text,
            };

            var e = DB.FOD<T>(i => i.topic_id == message.TopicId);

            if (e != null)
            {
                var filter = DB.F<T>().Eq(p => p.topic_id, message.TopicId);
                var update = DB.U<T>().Push(p => p.messages, se);
                UpdateResult result = await DB.UpdateAsync(filter, update);
            }
            else
            {
                var ne = new T()
                {
                    id = ObjectId.GenerateNewId(),
                    messages = new List<ChatMessageSE>() { se },
                    topic_id = message.TopicId
                };
                await DB.SaveAsync(ne);
            }

            return se;
        }

        public async Task DeleteMessage(ObjectId topicId, ObjectId messageId)
        {
            var t = DB.FOD<T>(p => p.topic_id == topicId);
            var m = t.messages.First(i => i.id == messageId);

            var filter = DB.F<T>().Eq(p => p.topic_id, topicId);
            var update = DB.U<T>().Pull(p => p.messages, m);
            UpdateResult result = await DB.UpdateAsync(filter, update);
        }

    }
}
