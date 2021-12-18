using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Mappings;

using builder_mgmt_server.Models.Tasks;
using builder_mgmt_server.Models.User;
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
    [Route("api/user")]
    public class UserController : BaseApiController
    {
        UserModel UModel;

        public UserController(IUserModel uModel, IDbOperations db) : base(db)
        {
            UModel = (UserModel)uModel;
        }

        [HttpGet]
        [AuthorizeApi]
        public async Task<ApiResult> Get(string id)
        {
            var uid = new ObjectId(id);

            var e = DB.FOD<UserEntity>(u => u.id == uid);

            var res = UserMappings.FromEntityToRes(e);

            return ResponseHelper.Successful(res);
        }

        [HttpGet("me")]
        [AuthorizeApi]
        public async Task<ApiResult> GetMe()
        {
            var e = DB.FOD<UserEntity>(u => u.id == UserIdObj);

            var res = UserMappings.FromEntityToRes(e);

            return ResponseHelper.Successful(res);
        }

        private Object GetPropUpdateValue(string item, string value)
        {
            if (item == "location")
            {
                var prms = value.Split("||");
                var c1 = double.Parse(prms[1]);
                var c2 = double.Parse(prms[2]);

                return new LocationSE()
                {
                    text = prms[0],
                    coords = new List<double>() { c1, c2 }
                };
            }

            return value;
        }


        [HttpPut("prop")]
        [AuthorizeApi]
        public async Task<ApiResult> UpdateItem([FromBody] UpdatePropRequest req)
        {
            var value = GetPropUpdateValue(req.item, req.value);

            var filter = DB.F<UserEntity>().Eq(p => p.id, UserIdObj);
            var update = DB.U<UserEntity>().Set(req.item, value);
            var res = await DB.UpdateAsync(filter, update);
            var successful = res.MatchedCount == 1;
            
            return ResponseHelper.Successful(successful);
        }

        [HttpPut]
        [AuthorizeApi]
        public async Task<ApiResult> Update([FromBody] UserRequest req)
        {
            var newUserDO = new UpdateUserDO()
            {
                Id = UserIdObj,
                FirstName = req.firstName,
                LastName = req.lastName,
                Desc = req.desc,
                Mail = req.mail,
                Phone = req.phone,
                Website = req.website,
            };

            if (req.location != null)
            {
                newUserDO.Location = new LocationDO()
                {
                    Text = req.location.text,
                    Coords = req.location.coords
                };
            }

            var id = await UModel.UpdateUserAsync(newUserDO);
            return ResponseHelper.Successful(true);
        }

        [HttpGet("find")]
        [AuthorizeApi]
        public async Task<ApiResult> FindUser(string s)
        {
            var ls = s != null ? s.ToLowerInvariant() : "";

            var users = DB.List<UserEntity>(u =>
                u.firstName.ToLowerInvariant().Contains(ls)
                ||
                u.lastName.ToLowerInvariant().Contains(ls)
            );

            var res = users.Select(u => UserMappings.FromEntityToRes(u)).ToList();

            return ResponseHelper.Successful(res);
        }





    }


}
