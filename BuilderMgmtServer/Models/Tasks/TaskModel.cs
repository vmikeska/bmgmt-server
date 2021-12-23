using builder_mgmt_server.Controllers;
using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Enums;
using builder_mgmt_server.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.Tasks
{
    public class TaskModel : ITaskModel
    {
        public IDbOperations DB;

        public UserIdService UserIdSvc;

        public TaskModel(IDbOperations db, IUserIdService userIdSvc)
        {
            DB = db;
            UserIdSvc = (UserIdService)userIdSvc;
        }


        public async Task<TaskEntity> CreateFromRequestAsync(TaskResponse req)
        {
            var e = new TaskEntity()
            {
                id = ObjectId.GenerateNewId(),
                owner_id = UserIdSvc.IdObj,
                name = req.name,
                type = req.type,
                manDays = req.manDays,
                manHours = req.manHours,
                desc = req.desc                
            };

            if (req.type == TaskTypeEnum.Month)
            {
                e.month = req.month;
                e.year = req.year;
                e.mid = TaskUtils.MidFromMonth(req.year, req.month);
            }

            if (req.type == TaskTypeEnum.Week)
            {
                e.week = req.week;
                e.year = req.year;
                e.wid = TaskUtils.WidFromWeek(req.year, req.week);
            }

            if (
                req.type == TaskTypeEnum.ExactFlexible
                ||
                req.type == TaskTypeEnum.ExactStatic
                )
            {
                e.dateFrom = DateTimeUtils.Utc(req.dateFrom);
                e.dateTo = DateTimeUtils.Utc(req.dateTo);
            }

            var res = await DB.SaveAsync(e);
            return res;
        }

        public async Task<bool> UpdateFromTaskTypeRequestAsync(TaskDateTypeResponse req)
        {
            var type = req.type;
            var id = new ObjectId(req.id);

            var us = new List<UpdateDefinition<TaskEntity>>();

            var filter = DB.F<TaskEntity>().Eq(p => p.id, id);

            var u = DB.U<TaskEntity>();

            if (!String.IsNullOrEmpty(req.name))
            {
                us.Add(u.Set(p => p.name, req.name));
            }

            us.Add(u.Set(p => p.type, req.type));
            us.Add(u.Set(p => p.manHours, req.manHours));
            us.Add(u.Set(p => p.manDays, req.manDays));

            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            int week = 0;
            int year = 0;
            int month = 0;

            int mid = 0;
            int wid = 0;

            if (req.type != TaskTypeEnum.Unassigned)
            {
                if (type == TaskTypeEnum.ExactFlexible || type == TaskTypeEnum.ExactStatic)
                {
                    dateFrom = DateTimeUtils.Utc(req.dateFrom);
                    dateTo = DateTimeUtils.Utc(req.dateTo);
                }

                if (type == TaskTypeEnum.Month)
                {
                    month = req.month;
                    year = req.year;
                    mid = TaskUtils.MidFromMonth(year, month);
                }

                if (type == TaskTypeEnum.Week)
                {
                    week = req.week;
                    year = req.year;
                    wid = TaskUtils.WidFromWeek(year, week);
                }
            }

            us.Add(u.Set(p => p.dateFrom, dateFrom));
            us.Add(u.Set(p => p.dateTo, dateTo));

            us.Add(u.Set(p => p.week, week));
            us.Add(u.Set(p => p.year, year));
            us.Add(u.Set(p => p.month, month));

            us.Add(u.Set(p => p.mid, mid));
            us.Add(u.Set(p => p.wid, wid));

            var update = u.Combine(us);

            var res = await DB.UpdateAsync(filter, update);
            var successful = res.MatchedCount == 1;
            return successful;
        }

        //public async Task<TaskEntity> UpdateFromRequestAsync(TaskResponse req)
        //{
        //    var type = req.type;
        //    var id = new ObjectId(req.id);

        //    var oldTask = DB.FOD<TaskEntity>(t => t.id == id);

        //    var e = new TaskEntity()
        //    {
        //        id = oldTask.id,
        //        owner_id = oldTask.owner_id,

        //        name = req.name,
        //        type = type,
        //        desc = req.desc
        //    };

        //    if (req.type != TaskTypeEnum.Unassigned)
        //    {
        //        e.manDays = req.manDays;
        //        e.manHours = req.manHours;

        //        if (type == TaskTypeEnum.ExactFlexible || type == TaskTypeEnum.ExactStatic)
        //        {
        //            e.dateFrom = DateTimeUtils.Utc(req.dateFrom);
        //            e.dateTo = DateTimeUtils.Utc(req.dateTo);
        //        }

        //        if (type == TaskTypeEnum.Month)
        //        {
        //            e.month = req.month;
        //            e.year = req.year;
        //            e.mid = TaskUtils.MidFromMonth(e.year, e.month);
        //        }

        //        if (type == TaskTypeEnum.Week)
        //        {
        //            e.week = req.week;
        //            e.year = req.year;
        //            e.wid = TaskUtils.WidFromWeek(e.year, e.week);
        //        }
        //    }

        //    var res = await DB.ReplaceOneAsync(e);
        //    return e;
        //}

        public List<TaskEntity> GetUnassignedTask()
        {
            var items = DB.List<TaskEntity>(t =>
                t.owner_id == UserIdSvc.IdObj
                &&
                t.type == TaskTypeEnum.Unassigned
            ).ToList();

            return items;
        }

        public GroupedTasksResult GetDashboardTasks()
        {
            var from = DateTime.UtcNow.ToDateStart();
            var to = from.MonthEnd().AddMonths(1);

            var tasks = this.GetTasksInDateRange(from, to);

            var res = this.MapGroupedTasks(tasks);
            return res;
        }

        public GroupedTasksResult GetProjectAssignedTasks(ObjectId id)
        {
            var tasks = GetAssignedTasksByProjId(id);

            var res = MapGroupedTasks(tasks);
            return res;
        }

        public GroupedTasksResult GetProjectUnassignedTasks(ObjectId id)
        {
            var tasks = GetUnassignedTasksByProjId(id);

            var res = MapGroupedTasks(tasks);
            return res;
        }

        private GroupedTasksResult MapGroupedTasks(List<TaskEntity> tasks)
        {
            var dateTasks = new List<TaskEntity>();
            var monthTasks = new List<TaskEntity>();
            var weekTasks = new List<TaskEntity>();

            tasks.ForEach((t) =>
            {
                if (t.type == TaskTypeEnum.Month)
                {
                    monthTasks.Add(t);
                }

                if (t.type == TaskTypeEnum.Week)
                {
                    weekTasks.Add(t);
                }

                if (t.type == TaskTypeEnum.ExactFlexible || t.type == TaskTypeEnum.ExactStatic)
                {
                    dateTasks.Add(t);
                }
            });

            var res = new GroupedTasksResult()
            {
                DateTasks = dateTasks.OrderBy(d => d.dateFrom).ToList(),
                MonthTasks = monthTasks.OrderBy(d => d.mid).ToList(),
                WeekTasks = weekTasks.OrderBy(d => d.wid).ToList()
            };

            return res;
        }

        private List<TaskEntity> GetAssignedTasksByProjId(ObjectId id)
        {
            var bindings = DB.List<ProjectsTaskEntity>(i => i.proj_id == id);
            var tasksIds = bindings.Select(i => i.task_id).ToList();

            var data = DB.List<TaskEntity>(t =>
            tasksIds.Contains(t.id)
            &&
            t.type != TaskTypeEnum.Unassigned
            );

            return data;
        }

        public List<TaskEntity> GetUnassignedTasksByProjId(ObjectId id)
        {
            var bindings = DB.List<ProjectsTaskEntity>(i => i.proj_id == id);
            var tasksIds = bindings.Select(i => i.task_id).ToList();

            var data = DB.List<TaskEntity>(t =>
            tasksIds.Contains(t.id)
            &&
            t.type == TaskTypeEnum.Unassigned
            );

            return data;
        }

        public List<TaskEntity> GetTasksInDateRange(DateTime from, DateTime to)
        {
            //months            
            var minMid = TaskUtils.MidFromDate(from);
            var maxMid = TaskUtils.MidFromDate(to);

            var minWid = TaskUtils.WidFromDate(from);
            var maxWid = TaskUtils.WidFromDate(to);

            var data = DB.List<TaskEntity>(t =>
            t.owner_id == UserIdSvc.IdObj
            &&
            t.type != TaskTypeEnum.Unassigned
            &&
            (
                t.mid >= minMid && t.mid <= maxMid
                ||
                t.wid >= minWid && t.mid <= maxWid
                ||
                t.dateFrom < to && from < t.dateTo
            )
            );

            return data;
        }

    }

    public class GroupedTasksResult
    {
        public List<TaskEntity> DateTasks;
        public List<TaskEntity> MonthTasks;
        public List<TaskEntity> WeekTasks;
    }
}
