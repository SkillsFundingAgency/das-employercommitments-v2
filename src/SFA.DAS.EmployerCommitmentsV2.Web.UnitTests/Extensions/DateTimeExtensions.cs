using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToMonthYearString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString("MMyyyy") : "";
        }
    }
}
