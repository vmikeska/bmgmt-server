using builder_mgmt_server.Models;
using builder_mgmt_server.Utils;
using System;
using Xunit;

namespace TestX
{
    public class UnitTest1
    {
        [Fact]
        public void TestExactTimeRange()
        {
            //var dateFrom = new DateTime(2021, 1, 4);
            //var dateTo = new DateTime(2021, 1, 8);

            //var tcm = new TaskWorkloadModel();
            //tcm.From = dateFrom;
            //tcm.To = dateTo;
            //tcm.Process();


            //Assert.True(true);
            //Assert.Equal(safeFrom, new DateTime(2021, 10, 11));
            //Assert.Equal(safeTo, new DateTime(2021, 10, 17));
        }

        [Fact]
        public void TestSafeDateRange()
        {

            var unsafeFrom = new DateTime(2021, 10, 13);
            var unsafeTo = new DateTime(2021, 10, 13);

            var safeFrom = unsafeFrom.WeekStart();
            var safeTo = unsafeTo.WeekEnd();

            Assert.Equal(safeFrom, new DateTime(2021, 10, 11));
            Assert.Equal(safeTo, new DateTime(2021, 10, 17));
        }

        [Fact]
        public void Test1()
        {

            //var tcm = new TaskWorkloadModel();
            //tcm.Process();


            //Assert.False(false);
        }
    }
}
