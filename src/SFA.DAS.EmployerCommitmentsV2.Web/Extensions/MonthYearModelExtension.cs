﻿using SFA.DAS.CommitmentsV2.Shared.Models;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class MonthYearModelExtension
    {
        public static bool IsEqualToOrAfterMonthYearOfDateTime(this MonthYearModel monthYearModel, DateTime dateTime)
        {
            var result = DateTime.Compare(monthYearModel.Date.Value, new DateTime(dateTime.Year, dateTime.Month, 1));

            return (result >= 0);
        }

        public static bool IsBeforeMonthYearOfDateTime(this MonthYearModel monthYearModel, DateTime datetime) 
        {
            return monthYearModel.Date.Value < new DateTime(datetime.Year, datetime.Month, 1);
        }
    }
}
