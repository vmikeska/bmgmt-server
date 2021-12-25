using builder_mgmt_server.Controllers;
using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Enums;
using builder_mgmt_server.Models.Project;
using builder_mgmt_server.Utils;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.Tasks
{
    public class ProjectModel : IProjectModel
    {
        public IDbOperations DB;

        public UserIdService UserIdSvc;

        public ProjectModel(IDbOperations db, IUserIdService userIdSvc)
        {
            DB = db;
            UserIdSvc = (UserIdService)userIdSvc;
        }


        public async Task<ProjectEntity> CreateAsync(string name, ObjectId ownerId)
        {
            var e = new ProjectEntity()
            {
                id = ObjectId.GenerateNewId(),
                owner_id = ownerId,
                name = name,
            };

            var res = await DB.SaveAsync(e);
            return res;
        }

        public async Task<ProjectEntity> UpdateFromRequestAsync(ProjectResponse req)
        {
            var id = new ObjectId(req.id);

            var oldProj = DB.FOD<ProjectEntity>(p => p.id == id);

            var e = new ProjectEntity()
            {
                id = oldProj.id,
                owner_id = oldProj.owner_id,

                name = req.name,
                desc = req.desc
            };

            var res = await DB.ReplaceOneAsync(e);
            return e;
        }

        public async Task<ProjectsTaskEntity> AddTaskToProjectAsync(ObjectId taskId, ObjectId projId)
        {
            await DB.DeleteAsync<ProjectsTaskEntity>(i => i.task_id == taskId);

            var e = new ProjectsTaskEntity()
            {
                id = ObjectId.GenerateNewId(),
                proj_id = projId,
                task_id = taskId
            };

            var res = await DB.SaveAsync(e);
            return res;
        }

        public async Task<bool> RemoveTaskToProjectAsync(ObjectId taskId)
        {
            var res = await DB.DeleteAsync<ProjectsTaskEntity>(i => i.task_id == taskId);
            return true;
        }

        public List<ProjectEntity> GetProjectsList()
        {
            var items = DB.List<ProjectEntity>(t =>
                t.owner_id == UserIdSvc.IdObj
            ).ToList();
            return items;
        }

        public List<ProjectEntity> GetParticipatingProjectsList(ObjectId userId)
        {
            var bindings = DB.List<ProjectParticipantEntity>(p => p.user_id == userId);

            if (!bindings.Any())
            {
                return new List<ProjectEntity>();
            }

            var projIds = bindings.Select(i => i.topic_id).ToList();

            var items = DB.List<ProjectEntity>(t => projIds.Contains(t.id)).ToList();
            return items;
        }




    }

}
