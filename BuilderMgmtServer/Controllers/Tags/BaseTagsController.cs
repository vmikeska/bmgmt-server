using builder_mgmt_server.Controllers.ChatMessages;
using builder_mgmt_server.Controllers.Tags;
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
    public class BaseTagsController<TBinding, TTag> : BaseApiController
        where TBinding : TagBindingBaseEntity, new()
        where TTag : TagBaseEntity, new()
    {

        public BaseTagsController(IDbOperations db) : base(db)
        {

        }

        private bool HasPermission(string entityId)
        {
            var has = UserId == entityId;
            return has;

        }

        [HttpGet("list")]
        [AuthorizeApi]
        public ApiResult SearchTags(
            //SearchTagResponse req
            string str, string entityId
            )
        {
            if (string.IsNullOrEmpty(str))
            {
                return ResponseHelper.EmptyArray();
            }

            var eid = new ObjectId(entityId);

            var tags = DB.List<TTag>(i => i.name.ToLower().Contains(str.ToLower()));

            var bindings = DB.List<TBinding>(i => i.entity_id == eid);
            var usedIds = bindings.Select(i => i.tag_id.ToString()).ToList();

            var unusedTags = tags.Where(t => !usedIds.Contains(t.id.ToString())).ToList();

            var res = unusedTags.Select(i =>
            {
                var item = new TagResponse()
                {
                    id = i.id.ToString(),
                    name = i.name
                };
                return item;
            });

            return ResponseHelper.Successful(res);
        }

        [HttpGet]
        [AuthorizeApi]
        public ApiResult GetSaved(string entityId)
        {
            //if (!HasPermission(entityId))
            //{                
            //    return new StatusCodeResult(401);
            //}

            var eid = new ObjectId(entityId);

            var bindings = DB.List<TBinding>(i => i.entity_id == eid);

            var tagIds = bindings.Select(i => i.tag_id);

            var tags = DB.List<TTag>(i => tagIds.Contains(i.id));

            var res = bindings.Select(i =>
            {
                var tag = tags.FirstOrDefault(t => t.id == i.tag_id);

                var item = new TagBindingResponse()
                {
                    bindingId = i.id.ToString(),
                    name = tag.name,
                    tagId = tag.id.ToString()
                };
                return item;
            }).ToList();

            return ResponseHelper.Successful(res);
        }

        [HttpPost]
        [AuthorizeApi]
        public async Task<ApiResult> Add([FromBody] NewTagBindingResponse req)
        {
            var eid = new ObjectId(req.entityId);
            var tid = new ObjectId(req.tagId);

            var ne = new TBinding()
            {
                id = ObjectId.GenerateNewId(),
                entity_id = eid,
                tag_id = tid
            };

            var res = await DB.SaveAsync(ne);

            return ResponseHelper.Successful(ne.id);
        }

        [HttpDelete]
        [AuthorizeApi]
        public async Task<ApiResult> Remove(string bindingId)
        {
            var bid = new ObjectId(bindingId);

            await DB.DeleteAsync<TBinding>(i => i.id == bid);

            return ResponseHelper.Successful(true);
        }


    }

}
