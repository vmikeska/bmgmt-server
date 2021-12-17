using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Mappings;
using builder_mgmt_server.Models.Contact;
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
    [Route("api/contact")]
    public class ContactController : BaseApiController
    {
        //UserModel UModel;
        ContactModel CModel;

        public ContactController(
            //IUserModel uModel,
            IDbOperations db,
            IContactModel cm
            ) : base(db)
        {
            //UModel = (UserModel)uModel;
            CModel = (ContactModel)cm;
        }

        [HttpGet("contacts")]
        [AuthorizeApi]
        public ApiResult GetAllContacts()
        {
            var results = CModel.GetInitContacts(UserIdObj);

            var res = results.Select(i =>
            {
                var r = new NewContactResponse()
                {
                    id = i.Id,
                    name = i.Name,
                    stared = i.Stared,
                    alreadyAdded = true                    
                };
                return r;
            }).ToList();

            return ResponseHelper.Successful(res);
        }

        [HttpGet("find-saved")]
        [AuthorizeApi]
        public ApiResult GetSavedContacts(string str)
        {
            var results = CModel.FindContactsByStr(UserIdObj, str);

            var nativeItems = results.Native.Select(i =>
            {
              var r = new NewContactResponse()
                {
                    id = i.Id,
                    name = i.Name,
                    stared = i.Stared,
                    alreadyAdded = true
                };
                return r;
            }).ToList();

            var customItems = results.Custom.Select(i =>
            {
                var r = new NewContactResponse()
                {
                    id = i.Id,
                    name = i.Name,
                    stared = i.Stared,
                    alreadyAdded = i.AlreadyAdded
                };
                return r;
            }).ToList();

            var res = nativeItems.Concat(customItems);
            return ResponseHelper.Successful(res);
        }

        [HttpGet("find-new")]
        [AuthorizeApi]
        public ApiResult GetNewContacts(string str)
        {
            var results = CModel.FindNewContactsByStr(UserIdObj, str);

            var res = results.Select(i =>
            {
                var r = new NewContactResponse()
                {
                    id = i.Id,
                    name = i.Name,
                    stared = i.Stared,
                    alreadyAdded = i.AlreadyAdded
                };
                return r;
            }).ToList();

            return ResponseHelper.Successful(res);
        }

        [HttpPost("new-native")]
        [AuthorizeApi]
        public async Task<ApiResult> AddNewNativeContact([FromBody] BindingChangeRequest req)
        {
            var contactId = new ObjectId(req.contactId);

            await CModel.AddNewNativeContactAsync(UserIdObj, contactId);
            return ResponseHelper.Successful(true);
        }

        [HttpDelete("remove-native")]
        [AuthorizeApi]
        public async Task<ApiResult> RemoveNativeContact(string contactId)
        {
            var contactIdObj = new ObjectId(contactId);

            await CModel.RemoveNativeContactAsync(UserIdObj, contactIdObj);
            return ResponseHelper.Successful(true);
        }

     
    }


}
