using builder_mgmt_server.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models
{
    public static class TaskCommonUtils
    {
        public static double CalculateIndex(double max, double value)
        {
            if (value == 0 || max == 0)
            {
                return 0;
            }

            var onePercent = max / 100;
            var percents = value / onePercent;
            var index = percents / 100;
            return index;
        }

        public static int GetWeeksInYear(int year)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date1 = new DateTime(year, 12, 31);
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static bool IsDayUsed(List<int> useDays, DateTime date)
        {
            var dayNo = date.DayOfWeekISO();
            var isDayUsed = useDays.Contains(dayNo);
            return isDayUsed;
        }

        public static int UsedDaysInPeriod(List<int> useDays, DateTime from, DateTime to)
        {
            var currentDay = from;
            var daysCnt = 0;

            while (currentDay <= to)
            {
                if (IsDayUsed(useDays, currentDay))
                {
                    daysCnt++;
                }
                currentDay = currentDay.AddDays(1);
            }

            return daysCnt;
        }
    }
}
