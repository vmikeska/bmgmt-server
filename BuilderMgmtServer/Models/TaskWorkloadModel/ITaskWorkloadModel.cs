using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.TasksBusyness
{
    public interface ITaskWorkloadModel
    {
        void Process();

        //List<CalendarMonthDO> Months { get; set; }
        //List<CalendarWeekDO> Weeks;
        //List<CalendarDayDO> Days;
        //List<TaskEntity> Data;

        //DateTime SafeFrom;
        //DateTime SafeTo;

        //List<int> UseDays = new List<int>();
        //DateTime From;
        //DateTime To;
    }
}
