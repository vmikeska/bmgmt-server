using builder_mgmt_server.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    public class AuthorizeApiAttribute : ActionFilterAttribute
    {
        public bool ExplicitAuth { get; set; }

        public AuthorizeApiAttribute(bool explicitAuth = false)
        {
            ExplicitAuth = explicitAuth;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string authorizationToken = context.HttpContext.Request.Cookies["session"];


            string decodedStr = string.Empty;

            try
            {
                decodedStr = JsonWebToken.Decode(authorizationToken, AppConfig.AppSecret, true);
            }
            catch
            {                
                if (!ExplicitAuth)
                {
                    context.Result = new StatusCodeResult(401);
                    //new HttpStatusCodeResult(401);
                }
                return;
            }

            var tokenObj = JsonConvert.DeserializeObject<UserToken>(decodedStr);

            var userId = tokenObj.UserId;

            ((BaseApiController)context.Controller).UserId = userId;

            var userIdSvc = (IUserIdService)context.HttpContext.RequestServices.GetService(typeof(IUserIdService));

            userIdSvc.IdStr = userId;

            base.OnActionExecuting(context);
        }

        
    }
}
