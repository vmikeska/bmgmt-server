using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Utils
{
    public static class TaskUtils
    {
        public static int MidFromDate(DateTime d)
        {
            var id = (d.Year * 100) + d.Month;
            return id;
        }

        public static int WidFromDate(DateTime d)
        {
            var id = (d.Year * 100) + d.GetIso8601WeekOfYear();
            return id;
        }

        public static int MidFromMonth(int year, int month)
        {
            var id = (year * 100) + month;
            return id;
        }

        public static int WidFromWeek(int year, int week)
        {
            var id = (year * 100) + week;
            return id;
        }
    }
}
