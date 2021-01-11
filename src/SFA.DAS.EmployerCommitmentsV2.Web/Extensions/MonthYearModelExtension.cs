using SFA.DAS.CommitmentsV2.Shared.Models;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class MonthYearModelExtension
    {
        public static bool IsGreaterThanOrEqualToDateTimeMonthYear(this MonthYearModel monthYearModel, DateTime dateTime)
        {
            var result = DateTime.Compare(new DateTime(monthYearModel.Year.Value, monthYearModel.Month.Value, 1), new DateTime(dateTime.Year, dateTime.Month, 1));

            return (result >= 0);
        }
    }
}
