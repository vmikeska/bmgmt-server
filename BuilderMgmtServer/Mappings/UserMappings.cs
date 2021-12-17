using builder_mgmt_server.Controllers;
using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Mappings
{
    public class UserMappings
    {
        public static UserResponse FromEntityToRes(UserEntity e)
        {
            var res = new UserResponse()
            {
                id = e.id.ToString(),
                firstName = e.firstName,
                lastName = e.lastName,
                desc = e.desc,
                mail = e.mail,
                phone = e.phone,
                website = e.website,
                location = new PlaceLocationResponse()
                {
                    text = "",
                    coords = new List<double>()
                }
            };

            if (e.location != null)
            {
                res.location = new PlaceLocationResponse()
                {
                    text = e.location.text,
                    coords = e.location.coords
                };
            }

            return res;
        }
    }
}
