using builder_mgmt_server.Controllers;
using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Enums;
using builder_mgmt_server.Models.Tasks;
using builder_mgmt_server.Models.TasksBusyness;
using builder_mgmt_server.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace builder_mgmt_server.Models
{
    public class TaskWorkloadModel : ITaskWorkloadModel
    {
        public IDbOperations DB;

        public UserIdService UserIdSvc;

        public TaskWorkloadModel(IDbOperations db, IUserIdService userIdSvc)
        {
            DB = db;
            UserIdSvc = (UserIdService)userIdSvc;
        }

        public List<CalendarMonthDO> Months = new List<CalendarMonthDO>();
        public List<CalendarWeekDO> Weeks = new List<CalendarWeekDO>();
        public List<CalendarDayDO> Days = new List<CalendarDayDO>();
        public List<TaskEntity> Data = new List<TaskEntity>();

        public DateTime SafeFrom;
        public DateTime SafeTo;

        public List<int> UseDays = new List<int>();
        public DateTime From;
        public DateTime To;

        private int WeekUsedDaysCount
        {
            get { return UseDays.Count; }
        }

        private double MinHours = 0;
        private double MaxHours = 0;

        private int HoursPerDay = 8;

        public void Process()
        {
            SetSafeDateRange();
            InitializeDaysField();
            LoadDataFromDB();
            AssignDataWorkLoad();
            AssignBusyIndex();
        }

        private void SetSafeDateRange()
        {
            SafeFrom = From.MonthStart().WeekStart();
            SafeTo = To.MonthEnd().WeekEnd();
        }

        private void LoadDataFromDB()
        {
            //months            
            var minMid = TaskUtils.MidFromDate(SafeFrom);
            var maxMid = TaskUtils.MidFromDate(SafeTo);

            var minWid = TaskUtils.WidFromDate(SafeFrom);
            var maxWid = TaskUtils.WidFromDate(SafeTo);

            var data = DB.List<TaskEntity>(t =>
            t.owner_id == UserIdSvc.IdObj
            &&
            (
                t.mid >= minMid && t.mid <= maxMid
                ||
                t.wid >= minWid && t.mid <= maxWid
                ||
                t.dateFrom < SafeTo && SafeFrom < t.dateTo
            )
            );

            Data = data;
        }

        private void AssignBusyIndex()
        {
            MaxHours = Days.Select(d => { return d.TotalHours; }).Max();

            foreach (var day in Days)
            {
                day.BusyIndex = TaskCommonUtils.CalculateIndex(MaxHours, day.TotalHours);
            }
        }

        private void AssignDataWorkLoad()
        {
            foreach (var item in Data)
            {
                var totalHours = item.manHours + (item.manDays * HoursPerDay); ;

                if (item.type == TaskTypeEnum.ExactFlexible)
                {
                    var currentDay = item.dateFrom.Value;

                    var usedDaysCount = TaskCommonUtils.UsedDaysInPeriod(UseDays, item.dateFrom.Value, item.dateTo.Value);

                    var manHoursPerDay = totalHours / usedDaysCount;

                    while (currentDay <= item.dateTo)
                    {
                        var day = Days.Find(d => d.Date.Equals(currentDay));

                        if (day != null)
                        {
                            if (TaskCommonUtils.IsDayUsed(UseDays, day.Date))
                            {
                                var load = new DayLoadDO()
                                {
                                    TaskId = item.id,
                                    Hours = manHoursPerDay,
                                    Type = item.type
                                };
                                day.Loads.Add(load);
                            }
                        }

                        currentDay = currentDay.AddDays(1);
                    }
                }

                if (item.type == TaskTypeEnum.ExactStatic)
                {
                    var currentDay = item.dateFrom.Value;

                    var daysCount = DaysInPeriod(item.dateFrom.Value, item.dateTo.Value);

                    var manHoursPerDay = totalHours / daysCount;

                    while (currentDay <= item.dateTo)
                    {
                        var day = Days.Find(d => d.Date.Equals(currentDay));
                        if (day != null)
                        {
                            var load = new DayLoadDO()
                            {
                                TaskId = item.id,
                                Hours = manHoursPerDay,
                                Type = item.type
                            };
                            day.Loads.Add(load);
                        }
                        currentDay = currentDay.AddDays(1);
                    }
                }

                if (item.type == TaskTypeEnum.Week)
                {
                    var week = Weeks.Find(w => w.Year == item.year && w.No == item.week);

                    var oneDayHours = totalHours / WeekUsedDaysCount;

                    foreach (var day in week.Days)
                    {

                        if (TaskCommonUtils.IsDayUsed(UseDays, day.Date))
                        {
                            var load = new DayLoadDO()
                            {
                                TaskId = item.id,
                                Hours = oneDayHours,
                                Type = item.type
                            };
                            day.Loads.Add(load);
                        }
                    }

                }

                if (item.type == TaskTypeEnum.Month)
                {
                    var month = Months.Find(w => w.Year == item.year && w.No == item.month);

                    var usedDaysCount = TaskCommonUtils.UsedDaysInPeriod(UseDays, month.Days.First().Date, month.Days.Last().Date);
                    var oneDayHours = totalHours / usedDaysCount;

                    foreach (var day in month.Days)
                    {
                        if (TaskCommonUtils.IsDayUsed(UseDays, day.Date))
                        {
                            var load = new DayLoadDO()
                            {
                                TaskId = item.id,
                                Hours = oneDayHours,
                                Type = item.type
                            };
                            day.Loads.Add(load);
                        }
                    }

                }
            }
        }

        private double DaysInPeriod(DateTime from, DateTime to)
        {
            return (to - from).TotalDays + 1;
        }

        private void InitializeDaysField()
        {
            var currentDay = SafeFrom;

            while (currentDay <= SafeTo)
            {
                var weekNo = currentDay.GetIso8601WeekOfYear();

                var month = GetMonth(currentDay.Year, currentDay.Month);
                var week = GetWeek(currentDay.Year, weekNo);

                var day = new CalendarDayDO()
                {
                    Date = currentDay,
                    Year = new CalendarYearDO() { No = currentDay.Year },
                    Month = month,
                    Week = week
                };
                Days.Add(day);

                week.Days.Add(day);
                month.Days.Add(day);

                currentDay = currentDay.AddDays(1);
            }

            foreach (var month in Months)
            {
                var usedDaysCount = TaskCommonUtils.UsedDaysInPeriod(UseDays, month.Days.First().Date, month.Days.Last().Date);
                month.WorkingDays = usedDaysCount;
            }

        }

        private CalendarWeekDO GetWeek(int year, int no)
        {
            var existing = Weeks.Find(m => m.Year == year && m.No == no);
            if (existing != null)
            {
                return existing;
            }

            var newWeek = new CalendarWeekDO() { Year = year, No = no, Days = new List<CalendarDayDO>() };
            Weeks.Add(newWeek);
            return newWeek;
        }

        private CalendarMonthDO GetMonth(int year, int no)
        {
            var existing = Months.Find(m => m.Year == year && m.No == no);
            if (existing != null)
            {
                return existing;
            }

            var newMonth = new CalendarMonthDO() { Year = year, No = no, Days = new List<CalendarDayDO>() };
            Months.Add(newMonth);
            return newMonth;
        }
    }

    public class DateRange
    {
        public DateTime From;
        public DateTime To;
    }
}
