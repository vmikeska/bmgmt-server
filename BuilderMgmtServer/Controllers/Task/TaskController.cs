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
    [Route("api/task")]
    public class TaskController : BaseApiController
    {
        TaskModel TModel;

        public TaskController(ITaskModel tModel, IDbOperations db) : base(db)
        {
            TModel = (TaskModel)tModel;
        }

        [HttpGet]
        [AuthorizeApi]
        public async Task<ApiResult> Get(string id)
        {
            var tid = new ObjectId(id);
            var e = DB.FOD<TaskEntity>(t => t.id == tid);

            var res = TaskMappings.FromEntityToRes(e);

            return ResponseHelper.Successful(res);
        }

        [HttpDelete]
        [AuthorizeApi]
        public async Task<ApiResult> Delete(string id)
        {
            var tid = new ObjectId(id);
            await DB.DeleteAsync<TaskEntity>(t => t.id == tid);

            return ResponseHelper.Successful(true);
        }

        [HttpGet("detail")]
        [AuthorizeApi]
        public ApiResult GetDetail(string id)
        {
            var tid = new ObjectId(id);

            var res = new TaskDetailResponse();

            var te = DB.FOD<TaskEntity>(t => t.id == tid);
            res.task = TaskMappings.FromEntityToRes(te);

            var tpe = DB.List<TaskParticipantEntity>(t => t.topic_id == tid);
            var tpGroups = tpe.Select(i => i.role).GroupBy(i => i);

            res.participants = tpGroups.Select(i =>
            {
                var r = new ParticipantsOverviewReponse()
                {
                    type = i.First(),
                    count = i.Count()
                };

                return r;
            }).ToList();


            var pte = DB.FOD<ProjectsTaskEntity>(p => p.task_id == tid);

            if (pte != null)
            {
                var pe = DB.FOD<ProjectEntity>(p => p.id == pte.proj_id);

                res.projectBinding = new TaskProjectResponse()
                {
                    hasProject = true,
                    projId = pte.proj_id.ToString(),
                    projName = pe.name
                };
            }
            else
            {
                res.projectBinding = new TaskProjectResponse()
                {
                    hasProject = false
                };
            }

            return ResponseHelper.Successful(res);
        }

        [HttpGet("unassigned")]
        [AuthorizeApi]
        public ApiResult GetUnassignedTasks()
        {
            var tasks = TModel.GetUnassignedTask();

            var res = tasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList();

            return ResponseHelper.Successful(res);
        }

        [HttpGet("dashboard-tasks")]
        [AuthorizeApi]
        public ApiResult GetDashboarTasks()
        {
            var groups = TModel.GetDashboardTasks();

            var res = new TaskGroupsResponse()
            {
                dates = groups.DateTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
                months = groups.MonthTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
                weeks = groups.WeekTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
            };

            return ResponseHelper.Successful(res);
        }

        [HttpPost]
        [AuthorizeApi]
        public async Task<ApiResult> Create([FromBody] TaskResponse req)
        {
            var e = await TModel.CreateFromRequestAsync(req);
            return ResponseHelper.Successful(e.id.ToString());
        }

        //[HttpPut]
        //[AuthorizeApi]
        //public async Task<ApiResult> Update([FromBody] TaskResponse req)
        //{
        //    var e = await TModel.UpdateFromRequestAsync(req);
        //    return ResponseHelper.Successful(e.id.ToString());
        //}

        [HttpPut("type")]
        [AuthorizeApi]
        public async Task<ApiResult> UpdateType([FromBody] TaskDateTypeResponse req)
        {
            var successful = await TModel.UpdateFromTaskTypeRequestAsync(req);
            return ResponseHelper.Successful(successful);
        }

        

    }


}
