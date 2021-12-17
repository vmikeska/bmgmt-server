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
    public class TempData
    {

        public static List<TaskEntity> GetTempWorkloadData(ObjectId userId) {
            
            var data = new List<TaskEntity>
            {
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Postavit zeď",
                    dateFrom = DateTimeUtils.Utc(2021, 1, 4),
                    dateTo = DateTimeUtils.Utc(2021, 1, 9),
                    type = TaskTypeEnum.ExactStatic,
                    manDays = 7,
                    manHours = 4
                },
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Velká jáma",
                    dateFrom = DateTimeUtils.Utc(2021, 3, 4),
                    dateTo = DateTimeUtils.Utc(2021, 3, 11),
                    type = TaskTypeEnum.ExactStatic,
                    manDays = 7,
                    manHours = 4
                },
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Vykopat základy",
                    dateFrom = DateTimeUtils.Utc(2021, 1, 10),
                    dateTo = DateTimeUtils.Utc(2021, 1, 20),
                    type = TaskTypeEnum.ExactFlexible,
                    manDays = 10,
                    manHours = 0
                },
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Fasáda izolace",
                    dateFrom = null,
                    dateTo = null,
                    year = 2021,
                    month = 1,
                    mid = 202101,
                    //wid = null,
                    type = TaskTypeEnum.Month,
                    manDays = 20
                },
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Nainstalovat okna",
                    dateFrom = null,
                    dateTo = null,
                    year = 2021,
                    month = 2,
                    mid = 202102,
                    //wid = null,
                    type = TaskTypeEnum.Month,
                    manDays = 40
                },
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Natřít rámy",
                    dateFrom = null,
                    dateTo = null,
                    year = 2021,
                    week = 2,
                    wid = 202102,
                    //mid = null,
                    type = TaskTypeEnum.Week,
                    manDays = 5
                },
                new TaskEntity
                {
                    id = ObjectId.GenerateNewId(),
                    owner_id = userId,
                    name = "Namontovat okna",
                    dateFrom = null,
                    dateTo = null,
                    year = 2021,
                    week = 1,
                    wid = 202101,
                    //mid = null,
                    type = TaskTypeEnum.Week,
                    manDays = 20
                }
            };

            return data;
        }
    }
}
