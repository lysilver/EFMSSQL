using System;

namespace Library
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 某月第一天
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /// <summary>
        /// 某月最后一天
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        public static DateTime? EndDateTime(DateTime? endDate)
        {
            return new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);
        }
    }
}