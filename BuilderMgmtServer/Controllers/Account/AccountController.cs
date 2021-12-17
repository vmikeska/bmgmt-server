using builder_mgmt_server.Auth;
using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Mappings;
using builder_mgmt_server.Models;
using builder_mgmt_server.Models.Account;
using builder_mgmt_server.Models.Tasks;
using builder_mgmt_server.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    [Route("api/account")]
    public class AccountController : BaseApiController
    {
        AccountModel AModel;

        public AccountController(IAccountModel aModel, IDbOperations db) : base(db)
        {
            AModel = (AccountModel)aModel;
        }

        [HttpGet("test-accounts")]
        public async Task<ApiResult> CreateTestAccounts()
        {
            var items = new List<NewAccountDO>()
            {
                new NewAccountDO()
                {
                    Mail = "aa@test.com",
                    Password = "pass",
                    FirstName = "John",
                    LastName = "Malkovic"
                },
                new NewAccountDO()
                {
                    Mail = "bb@test.com",
                    Password = "pass",
                    FirstName = "Karel",
                    LastName = "Borec"
                },
                new NewAccountDO()
                {
                    Mail = "cc@test.com",
                    Password = "pass",
                    FirstName = "Jaroslav",
                    LastName = "Newton"
                },
                new NewAccountDO()
                {
                    Mail = "dd@test.com",
                    Password = "pass",
                    FirstName = "John",
                    LastName = "Borek"
                },
                new NewAccountDO()
                {
                    Mail = "ee@test.com",
                    Password = "pass",
                    FirstName = "Uma",
                    LastName = "Thurman"
                },
                new NewAccountDO()
                {
                    Mail = "ff@test.com",
                    Password = "pass",
                    FirstName = "Jaroslavovic",
                    LastName = "Bretnezovskijicieam"
                },
                new NewAccountDO()
                {
                    Mail = "uu@test.com",
                    Password = "pass",
                    FirstName = "Thomas",
                    LastName = "Poll"
                },
                new NewAccountDO()
                {
                    Mail = "kk@test.com",
                    Password = "pass",
                    FirstName = "Johnny",
                    LastName = "Doe"
                },
                new NewAccountDO()
                {
                    Mail = "ll@test.com",
                    Password = "pass",
                    FirstName = "Miroslav",
                    LastName = "Řezníček"
                },
                new NewAccountDO()
                {
                    Mail = "vv@test.com",
                    Password = "pass",
                    FirstName = "Pavel",
                    LastName = "Boťka"
                },
            };

            foreach (var u in items)
            {
                await AModel.AddNewAccountAsync(u);
            }

            return ResponseHelper.Successful(true);
        }


        [HttpGet("test")]
        public string Get()
        {
            return "test je ok 333";
        }

        [HttpGet("info")]
        [AuthorizeApi]
        public async Task<ApiResult> GetInfoConfig()
        {
            var ue = DB.FOD<UserEntity>(u => u.id == UserIdObj);
            var ae = DB.FOD<AccountEntity>(u => u.user_id == UserIdObj);

            var res = new InfoConfigResponse()
            {
                firstName = ue.firstName,
                lastName = ue.lastName,
                mail = ae.mail,
                dayWorkingHours = 8
            };

            return ResponseHelper.Successful(res);
        }

        [HttpGet("test-db")]
        public async Task<ApiResult> Test()
        {
            try
            {
                var test = DB.List<AccountEntity>();
            }
            catch (Exception exc)
            {
                return ResponseHelper.Successful(exc.Message);
            }

            return ResponseHelper.Successful(true);
        }

        [HttpPost]
        public async Task<ApiResult> Create([FromBody] NewAccountRequest req)
        {
            var newAccountDO = new NewAccountDO()
            {
                Mail = req.mail,
                Password = req.password
            };

            var userId = await AModel.AddNewAccountAsync(newAccountDO);

            SetSessionCookie(userId);

            return ResponseHelper.Successful(true);
        }

        [HttpPost("login")]
        public async Task<ApiResult> Login([FromBody] LoginRequest req)
        {
            var e = DB.FOD<AccountEntity>(u => u.mail == req.mail && u.password == req.password);

            var userExists = e != null;

            if (userExists)
            {
                SetSessionCookie(e.user_id.ToString());
            }

            return ResponseHelper.Successful(true);
        }

        private void SetSessionCookie(string userId)
        {
            var cookieOptions = new CookieOptions()
            {
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(365),
                IsEssential = true,
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None     
                
            };

            var tokenStr = TokenUtils.CreateTokenString(userId);

            Response.Cookies.Append("session", tokenStr, cookieOptions);
        }
    }
}
