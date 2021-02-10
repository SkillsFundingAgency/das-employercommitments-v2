using SFA.DAS.CommitmentsV2.Shared.Models;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class MonthYearModelExtension
    {
        public static bool IsEqualToOrAfterMonthYearOfDateTime(this MonthYearModel monthYearModel, DateTime dateTime)
        {
            return monthYearModel.Date.Value >= new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static bool IsBeforeMonthYearOfDateTime(this MonthYearModel monthYearModel, DateTime datetime)
        {
            return monthYearModel.Date.Value < new DateTime(datetime.Year, datetime.Month, 1);
        }

        public static bool IsNotInFutureMonthYear(this MonthYearModel monthYearModel)
        {
            var dateTimeNow = DateTime.UtcNow;
            var futureDateAndTime = new DateTime(dateTimeNow.Year, dateTimeNow.AddMonths(1).Month, 1);
            return monthYearModel.Date.Value < futureDateAndTime;
        }
    }
}

