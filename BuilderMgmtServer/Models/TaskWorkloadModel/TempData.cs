using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Enums;
using builder_mgmt_server.Utils;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.TasksBusyness
{
    public class DatePoints
    {
        public DateTime from;
        public DateTime to;
    }

    public class TempData
    {

        public static DateTime To(DateTime from, int days)
        {
            return from.AddDays(days);
        }

        public static List<TaskEntity> tasks = new List<TaskEntity>();

        public static ObjectId UserId;


        public static void AddSimpleTask(string name)
        {
            tasks.Add(
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = UserId,
                    name = name,
                    dateFrom = null,
                    dateTo = null,
                    type = TaskTypeEnum.Unassigned,
                    manDays = 1,
                    manHours = 2
                });

        }

        public static void AddExactStaticTask(string name)
        {
            tasks.Add(
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = UserId,
                    name = name,
                    dateFrom = null,
                    dateTo = null,
                    type = TaskTypeEnum.Unassigned,
                    manDays = 1,
                    manHours = 2
                });

        }

        public static List<TaskEntity> GetTasks(ObjectId userId, int month, int year)
        {
            tasks.Clear();
            UserId = userId;

            var firstDayOfMonth = DateTimeUtils.Utc(year, month, 1);
            var firstDayFirstWeek = firstDayOfMonth.WeekStart();
            var firstDaySecondWeek = firstDayOfMonth.AddDays(7);
            var firstDayThirdWeek = firstDayOfMonth.AddDays(14);
            var firstDayFourthWeek = firstDayOfMonth.AddDays(21);


            AddSimpleTask("Koupit barvy na okna");
            AddSimpleTask("Nacenit zeď pro Kropáčka");
            AddSimpleTask("Vrátit palety");

            tasks.Add(
                 new TaskEntity
                 {
                     id = ObjectId.GenerateNewId(),
                     owner_id = userId,
                     name = "Postavit zeď",
                     dateFrom = DateTimeUtils.Utc(year, firstDayFirstWeek.Month, firstDayFirstWeek.Day),
                     dateTo = DateTimeUtils.Utc(year, To(firstDayFirstWeek, 7).Month, To(firstDayFirstWeek, 7).Day),
                     type = TaskTypeEnum.ExactStatic,
                     manDays = 7,
                     manHours = 4
                 }
                 );


            tasks.Add(
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Velká jáma",
                    dateFrom = DateTimeUtils.Utc(year, To(firstDayFirstWeek, 2).Month, To(firstDayFirstWeek, 2).Day),
                    dateTo = DateTimeUtils.Utc(year, To(firstDayFirstWeek, 5).Month, To(firstDayFirstWeek, 5).Day),
                    type = TaskTypeEnum.ExactStatic,
                    manDays = 7,
                    manHours = 4
                }
            );

            tasks.Add(
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Vykopat základy",
                    dateFrom = DateTimeUtils.Utc(2021, firstDaySecondWeek.Month, firstDaySecondWeek.Day),
                    dateTo = DateTimeUtils.Utc(2021, To(firstDaySecondWeek, 10).Month, To(firstDaySecondWeek, 10).Day),
                    type = TaskTypeEnum.ExactFlexible,
                    manDays = 10,
                    manHours = 0
                }
            );

            tasks.Add(
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Fasáda izolace",
                    dateFrom = null,
                    dateTo = null,
                    year = year,
                    month = month,
                    mid = 202101,
                    type = TaskTypeEnum.Month,
                    manDays = 20
                }
                );


            var nextMonth = firstDayFourthWeek.AddMonths(1);
            var lMonth = nextMonth.Month;
            var lYear = nextMonth.Year;

            tasks.Add(
                 new TaskEntity
                 {
                     id = ObjectId.GenerateNewId(),
                     owner_id = userId,
                     name = "Nainstalovat okna",
                     dateFrom = null,
                     dateTo = null,
                     type = TaskTypeEnum.Month,
                     manDays = 40,
                     year = lYear,
                     month = lMonth,
                     mid = TaskUtils.MidFromMonth(lYear, lMonth)
                 }
            );

            var lWeek = firstDaySecondWeek.GetIso8601WeekOfYear();
            lYear = firstDaySecondWeek.Year;

            tasks.Add(
                 new TaskEntity
                 {
                     id = ObjectId.GenerateNewId(),
                     owner_id = userId,
                     name = "Natřít rámy",
                     dateFrom = null,
                     dateTo = null,
                     year = lYear,
                     week = lWeek,
                     type = TaskTypeEnum.Week,
                     wid = TaskUtils.WidFromWeek(lYear, lWeek),
                     manDays = 5
                 }
            );

            lWeek = firstDayThirdWeek.GetIso8601WeekOfYear();
            lYear = firstDayThirdWeek.Year;

            tasks.Add(
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Namontovat okna",
                    dateFrom = null,
                    dateTo = null,
                    year = lYear,
                    week = lWeek,
                    wid = TaskUtils.WidFromWeek(lYear, lWeek),
                    type = TaskTypeEnum.Week,
                    manDays = 20
                }
            );

            return tasks;
        }
    }
}
