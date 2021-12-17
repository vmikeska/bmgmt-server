using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Auth
{
    public class TokenUtils
    {
        //public string IssueNewToken()
        //{
        //    var token = new UserToken
        //    {                
        //        UserId = ObjectId.GenerateNewId().ToString()
        //    };

        //    string encodedToken = CreateTokenString(token);
        //    return encodedToken;
        //}

        public static string CreateTokenString(string id)
        {

            var ut = new UserToken()
            {
                UserId = id
            };

            var tokenStr = JsonConvert.SerializeObject(ut);
            var tokenJson = JObject.Parse(tokenStr);

            string encodedToken = JsonWebToken.Encode(tokenJson, AppConfig.AppSecret, JwtHashAlgorithm.RS256);
            return encodedToken;
        }

        //public UserToken ReadToken(HttpContext context)
        //{
        //    var tokenStr = context.Request.Cookies[TokenConstants.CookieKeyName].ToString();
        //    if (string.IsNullOrEmpty(tokenStr))
        //    {
        //        return null;
        //    }

        //    try
        //    {
        //        string decodedStr = JsonWebToken.Decode(tokenStr, GloobsterConfig.AppSecret, true);

        //        var tokenObj = JsonConvert.DeserializeObject<UserToken>(decodedStr);
        //        return tokenObj;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
    }
}
