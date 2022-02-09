using System;

namespace PocketSmith.DataExport.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToFirstOfNextMonth(this DateTime inputDateTime)
        {
            return new DateTime(inputDateTime.Month < 12 ? inputDateTime.Year : inputDateTime.Year + 1,
                inputDateTime.Month < 12 ? inputDateTime.Month + 1 : 1, 1);
        }
    }
}