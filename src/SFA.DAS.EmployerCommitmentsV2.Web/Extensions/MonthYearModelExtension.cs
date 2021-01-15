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
    }
}
