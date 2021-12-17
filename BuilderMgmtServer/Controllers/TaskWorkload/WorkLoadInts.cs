using builder_mgmt_server.DOs;
using builder_mgmt_server.Enums;
using System.Collections.Generic;

namespace builder_mgmt_server.Controllers
{

    public class WorkloadResponse
    {
        public List<WorkloadDayResponse> days { get; set; }
        public List<TaskResponse> tasks { get; set; }

        public DateRange dateRange { get; set; }
        public List<WeekResponse> weeks { get; set; }
        public List<MonthResponse> months { get; set; }
    }

    public class WorkloadDayResponse
    {
        public string date { get; set; }
        public List<DayLoadResponse> loads { get; set; }
        public double totalHours { get; set; }
        public double busyIndex { get; set; }
    }

    public class DayLoadResponse
    {
        public string taskId { get; set; }
        public double hours { get; set; }
        public TaskTypeEnum type { get; set; }
    }

    public class DateRange
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class WeekResponse
    {
        public int year { get; set; }
        public int no { get; set; }
        public double totalHours { get; set; }
    }

    public class MonthResponse
    {
        public int year { get; set; }
        public int no { get; set; }
        public double totalHours { get; set; }
        public int workingDays { get; set; }

        public List<string> involvedTasksIds { get; set; }
    }
}