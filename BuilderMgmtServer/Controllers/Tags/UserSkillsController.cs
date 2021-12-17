using builder_mgmt_server.Database;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Models;
using builder_mgmt_server.Models.ChatMessages;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    [Route("api/user-skills")]
    public class UserSkillsController :
        BaseTagsController<UserSkillsBindingEntity, UserSkillsTagEntity>
    {

        public UserSkillsController(IDbOperations db) : base(db)
        {
            
        }

        [HttpGet("fill")]
        [AuthorizeApi]
        public async Task<ApiResult> GetSaved(string entityId)
        {
            var items = new List<UserSkillsTagEntity>()
            {
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Mason"
                },
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Tiler"
                },
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Builder"
                },
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Carpenter"
                },
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Service man"
                },
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Walls"
                },
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Bricks"
                },
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Fences"
                },
                new UserSkillsTagEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    name = "Roofs"
                }
            };

            await DB.SaveManyAsync(items);

            return ResponseHelper.Successful(true);
        }




    }

}
