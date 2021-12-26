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
    public class ProjectMappings
    {
        public static ProjectResponse FromEntityToRes(ProjectEntity e)
        {
            var res = new ProjectResponse()
            {
                id = e.id.ToString(),
                name = e.name,
                desc = e.desc,
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
