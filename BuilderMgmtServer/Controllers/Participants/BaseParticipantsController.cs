using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Mappings;

using builder_mgmt_server.Models.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    [ApiController]
    public class BaseParticipantsController<T> : BaseApiController
        where T : TopicParticipantEntity, new()
    {        
        public BaseParticipantsController(IDbOperations db) : base(db)
        {
            
        }

        [HttpGet]
        [AuthorizeApi]
        public ApiResult GetParticipants(string topicId)
        {
            var tid = new ObjectId(topicId);

            var participantEntities = DB.List<T>(t => t.topic_id == tid).ToList();

            var userIds = participantEntities.Select(i => i.user_id);

            var userEntities = DB.List<UserEntity>(u => userIds.Contains(u.id)).ToList();

            var response = participantEntities.Select((i) =>
            {
                var uid = i.user_id.ToString();

                var user = userEntities.First(u => u.id.ToString() == uid);

                //todo: user exist check

                var res = new TopicParticipantResponse()
                {
                    bindingId = i.id.ToString(),
                    role = i.role,
                    userId = uid,
                    firstName = user.firstName,
                    lastName = user.lastName
                };

                return res;
            }).ToList();

            return ResponseHelper.Successful(response);
        }


        [HttpPost]
        [AuthorizeApi]
        public async Task<ApiResult> Add([FromBody] TopicNewParticipantRequest req)
        {
            var taskId = new ObjectId(req.topicId);
            var userId = new ObjectId(req.userId);
            var e = new T()
            {
                id = ObjectId.GenerateNewId(),
                topic_id = taskId,
                user_id = userId,
                role = req.role
            };

            var res = await DB.SaveAsync(e);

            return ResponseHelper.Successful(e.id.ToString());
        }

        [HttpDelete]
        [AuthorizeApi]
        public async Task<ApiResult> Delete(string bindingId)
        {
            var bid = new ObjectId(bindingId);
            await DB.DeleteAsync<T>(bid);

            return ResponseHelper.Successful(true);
        }

    }


}
