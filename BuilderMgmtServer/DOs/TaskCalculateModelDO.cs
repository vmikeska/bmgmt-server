using builder_mgmt_server.Enums;
using builder_mgmt_server.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.DOs
{


    public class CalendarUnitBase
    {
        public double Hours;
    }

    public class CalendarMonthDO
    {
        public int Year;
        public int No;

        public List<CalendarDayDO> Days;

        public int WorkingDays;

        public double TotalHours
        {
            get
            {
                var t = Days.Sum(d => d.TotalHours);
                return t;
            }
        }

        public List<string> InvolvedTasksIds
        {
            get
            {
                var ids = Days.SelectMany((d) => {
                    return d.Loads.Select((l) =>
                    { 
                        return l.TaskId.ToString();
                    });
                }).Distinct().ToList();
                return ids;
            }
        }
        
    }

    public class CalendarWeekDO
    {
        public int Year;
        public int No;

        public List<CalendarDayDO> Days;

        public double TotalHours
        {
            get
            {
                var t = Days.Sum(d => d.TotalHours);
                return t;
            }
        }
    }

    public class CalendarYearDO
    {
        public int No;
    }

    public class CalendarDayDO
    {
        public DateTime Date;

        public double BusyIndex;

        public List<DayLoadDO> Loads = new List<DayLoadDO>();

        public double TotalHours
        {
            get
            {
               var tot = Loads.Sum(l => { return l.Hours; });
               return tot;
            }
        }

        public CalendarWeekDO Week;
        public CalendarMonthDO Month;
        public CalendarYearDO Year;
    }

    public class DayLoadDO
    {
        public ObjectId TaskId;
        public double Hours;
        public TaskTypeEnum Type;
    }
}
