using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Utils
{

    public static class DateTimeUtils
    {
        public static DateTime Utc(int year, int month, int day)
        {
            var d = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            return d;
        }

        public static DateTime Utc(string str)
        {
            var prms = str.Split("-");
            var du = Utc(int.Parse(prms[0]), int.Parse(prms[1]), int.Parse(prms[2]));
            return du;
        }
    }


    public static class DateTimeExtensions
    {

        public static int DayOfWeekISO(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return 7;
            }

            return (int)date.DayOfWeek;
        }

        public static string ToIsoDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string ToIsoDateTimeString(this DateTime date)
        {
            return date.ToString("s", CultureInfo.InvariantCulture);
            //To get the additional Z at the end as the OP requires, use "o" instead of "s".
        }

        public static bool IsWeekDay(this DateTime date)
        {
            return date.DayOfWeekISO() <= 5;
        }

        public static DateTime WeekStart(this DateTime date)
        {
            return date.AddDays(-(date.DayOfWeekISO() - 1));
        }

        public static DateTime WeekEnd(this DateTime date)
        {
            return date.AddDays(7 - date.DayOfWeekISO());
        }

        public static DateTime MonthStart(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime MonthEnd(this DateTime date)
        {
            return date.MonthStart().AddMonths(1).AddDays(-1);
        }

        public static DateTime ToDateEnd(this DateTime date, DateTimeKind kind = DateTimeKind.Utc)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, kind);
            return dateTime;
        }

        public static DateTime ToDateStart(this DateTime date, DateTimeKind kind = DateTimeKind.Utc)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, kind);
            return dateTime;
        }



        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }



    }
}
