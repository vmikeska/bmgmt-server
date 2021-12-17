using builder_mgmt_server.Database;
using builder_mgmt_server.Mappings;
using builder_mgmt_server.Models;
using builder_mgmt_server.Models.TasksBusyness;
using builder_mgmt_server.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    [ApiController]
    [Route("api/task-workload")]        
    public class TaskWorkloadController : BaseApiController
    {
        TaskWorkloadModel TWModel;

        public TaskWorkloadController(ITaskWorkloadModel twModel, IDbOperations db) : base (db)
        {
            TWModel = (TaskWorkloadModel)twModel;
        }

        [HttpGet]
        [AuthorizeApi]
        public async Task<ApiResult> Get([FromQuery] List<int> useDays, string from, string to)
        {
            var dFrom = DateTime.Parse(from);
            var dTo = DateTime.Parse(to);

            TWModel.UseDays = useDays;
            TWModel.From = dFrom;
            TWModel.To = dTo;
            TWModel.Process();

            var days = TWModel.Days.Select(d =>
            {
                var day = new WorkloadDayResponse
                {
                    date = d.Date.ToString("yyyy-MM-dd"),
                    loads = d.Loads.Select(l =>
                    {
                        var ld = new DayLoadResponse()
                        {
                            taskId = l.TaskId.ToString(),
                            hours = l.Hours,
                            type = l.Type
                        };
                        return ld;
                    }).ToList(),
                    totalHours = d.TotalHours,
                    busyIndex = d.BusyIndex
                };
                return day;
            }).ToList();

            var tasks = TWModel.Data.Select(i =>
            {
                var res = TaskMappings.FromEntityToRes(i);
                return res;
            }).ToList();

            var weeks = TWModel.Weeks.Select(i =>
            {
                var res = new WeekResponse()
                {
                    year = i.Year,
                    no = i.No,
                    totalHours = i.TotalHours
                };

                return res;
            }).ToList();

            var months = TWModel.Months.Select(i =>
            {
                var res = new MonthResponse()
                {
                    year = i.Year,
                    no = i.No,
                    totalHours = i.TotalHours,
                    workingDays = i.WorkingDays,
                    involvedTasksIds = i.InvolvedTasksIds
                };

                return res;
            }).ToList();

            var response = new WorkloadResponse
            {
                days = days,
                tasks = tasks,
                weeks = weeks,
                months = months,
                dateRange = new DateRange()
                {
                    from = TWModel.SafeFrom.ToIsoDateString(),
                    to = TWModel.SafeTo.ToIsoDateString()
                }
            };
            
            return ResponseHelper.Successful(response);
        }
    }

    
}
