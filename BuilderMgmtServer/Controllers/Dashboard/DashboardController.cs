using builder_mgmt_server.Controllers.User;
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
    [Route("api/dashboard")]
    public class DashboardController : BaseApiController
    {
        TaskModel TModel;

        public DashboardController(ITaskModel tModel, IDbOperations db) : base(db)
        {
            TModel = (TaskModel)tModel;
        }

        [HttpGet]
        [AuthorizeApi]
        public async Task<ApiResult> Get(bool onlyUnfinished, string search)
        {
            var str = string.IsNullOrEmpty(search) ? null : search.ToLower();

            var tes = GetTopicResults<TaskEntity, TaskParticipantEntity>(str, onlyUnfinished);

            var taskResponses = MapTasks(tes);

            var pes = GetTopicResults<ProjectEntity, ProjectParticipantEntity>(str, onlyUnfinished);

            var projsRespones = MapProjs(pes);

            var res = new DashResultResponse()
            {
                tasks = taskResponses,
                projs = projsRespones
            };

            return ResponseHelper.Successful(res);
        }

        private List<TTopicEntity> GetTopicResults<TTopicEntity, TParticipantEntity>
            (string str, bool onlyUnfinished)
            where TParticipantEntity : TopicParticipantEntity
            where TTopicEntity : SearchableEntity
        {
            var pes = DB.List<TaskParticipantEntity>(t => t.user_id == UserIdObj);
            var topicIds = pes.Select(t => t.topic_id).ToList();

            var safeStr = str == null ? "" : str;

            var ents = DB.List<TTopicEntity>(t =>
                //(onlyUnfinished && t.finished != true)                
                //&&
                (t.owner_id == UserIdObj || topicIds.Contains(t.id))
                &&
                (string.IsNullOrEmpty(str) || t.name.ToLower().Contains(safeStr))
            );
            return ents.ToList();
        }

        private List<DashTaskResponse> MapTasks(List<TaskEntity> es)
        {
            var rs = es.Select(t => new DashTaskResponse()
            {
                id = t.id.ToString(),
                name = t.name,
                type = t.type,
                isOwner = t.owner_id == UserIdObj
            }).ToList();
            return rs;
        }

        private List<DashProjResponse> MapProjs(List<ProjectEntity> es)
        {
            var rs = es.Select(t => new DashProjResponse()
            {
                id = t.id.ToString(),
                name = t.name,
                isOwner = t.owner_id == UserIdObj
            }).ToList();
            return rs;
        }

        //[HttpGet("dashboard-tasks")]
        //[AuthorizeApi]
        //public ApiResult GetDashboarTasks()
        //{
        //    var groups = TModel.GetDashboardTasks();

        //    var res = new TaskGroupsResponse()
        //    {
        //        dates = groups.DateTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
        //        months = groups.MonthTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
        //        weeks = groups.WeekTasks.Select(t => TaskMappings.FromEntityToRes(t)).ToList(),
        //    };

        //    return ResponseHelper.Successful(res);
        //}

    }


}
