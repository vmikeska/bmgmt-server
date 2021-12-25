using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Mappings;
using builder_mgmt_server.Models.Project;
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
    [Route("api/project")]
    public class ProjectController : BaseApiController
    {
        ProjectModel ProjModel;
        TaskModel TModel;

        public ProjectController(IProjectModel projModel, ITaskModel tm, IDbOperations db) : base(db)
        {
            ProjModel = (ProjectModel)projModel;
            TModel = (TaskModel)tm;
        }

        [HttpGet("detail")]
        [AuthorizeApi]
        public ApiResult GetDetail(string id)
        {
            var pid = new ObjectId(id);

            var res = new ProjectDetailResponse();

            var pe = DB.FOD<ProjectEntity>(t => t.id == pid);
            res.project = ProjectMappings.FromEntityToRes(pe);

            var ppe = DB.List<ProjectParticipantEntity>(t => t.topic_id == pid);
            var ppGroups = ppe.Select(i => i.role).GroupBy(i => i);

            res.participants = ppGroups.Select(i =>
            {
                var r = new ParticipantsOverviewReponse()
                {
                    type = i.First(),
                    count = i.Count()
                };

                return r;
            }).ToList();

            return ResponseHelper.Successful(res);
        }

        [HttpGet]
        [AuthorizeApi]
        public async Task<ApiResult> Get(string id)
        {
            var tid = new ObjectId(id);
            var e = DB.FOD<ProjectEntity>(t => t.id == tid);

            var res = ProjectMappings.FromEntityToRes(e);

            return ResponseHelper.Successful(res);
        }

        [HttpPost]
        [AuthorizeApi]
        public async Task<ApiResult> Create([FromBody] ProjectResponse req)
        {
            var e = await ProjModel.CreateAsync(req.name, UserIdObj);
            return ResponseHelper.Successful(e.id.ToString());
        }

        [HttpGet("assigned-tasks")]
        [AuthorizeApi]
        public ApiResult GetProjectAssignedTasks(string id)
        {
            var pid = new ObjectId(id);
            var groups = TModel.GetProjectAssignedTasks(pid);

            var res = new TaskGroupsResponse()
            {
                dates = groups.DateTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
                months = groups.MonthTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
                weeks = groups.WeekTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
            };

            return ResponseHelper.Successful(res);
        }

        [HttpGet("unassigned-tasks")]
        [AuthorizeApi]
        public ApiResult GetUnassignedTasks(string id)
        {
            var pid = new ObjectId(id);
            var tasks = TModel.GetUnassignedTasksByProjId(pid);

            var res = tasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList();

            return ResponseHelper.Successful(res);
        }


        [HttpPut]
        [AuthorizeApi]
        public async Task<ApiResult> Update([FromBody] ProjectResponse req)
        {
            var e = await ProjModel.UpdateFromRequestAsync(req);
            return ResponseHelper.Successful(e.id.ToString());
        }

        //-----

        [HttpPost("assign-task")]
        [AuthorizeApi]
        public async Task<ApiResult> AddTaskToProject([FromBody] ProjectTaskBindingRequest req)
        {
            var tid = new ObjectId(req.taskId);
            var pid = new ObjectId(req.projId);
            var e = await ProjModel.AddTaskToProjectAsync(tid, pid);
            return ResponseHelper.Successful(e.id.ToString());
        }

        [HttpDelete("unassign-task")]
        [AuthorizeApi]
        public async Task<ApiResult> RemoveTaskToProject(string taskId)
        {
            var tid = new ObjectId(taskId);            
            var e = await ProjModel.RemoveTaskToProjectAsync(tid);
            return ResponseHelper.Successful(true);
        }

        [HttpGet("list")]
        [AuthorizeApi]
        public ApiResult GetMyProjects()
        {
            var tasks = ProjModel.GetProjectsList();

            var res = tasks.Select(t => ProjectMappings.FromEntityToRes(t)).ToList();

            return ResponseHelper.Successful(res);
        }

        [HttpGet("list-particip")]
        [AuthorizeApi]
        public ApiResult GetParticipatingProjects()
        {
            var tasks = ProjModel.GetParticipatingProjectsList(UserIdObj);

            var res = tasks.Select(t => ProjectMappings.FromEntityToRes(t)).ToList();

            return ResponseHelper.Successful(res);
        }

    }


}
