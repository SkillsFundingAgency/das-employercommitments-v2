using System;
using System.Linq;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class FundingCapExtension
    {
        public static int FundingCapOn(this TrainingProgramme course, DateTime date)
        {
            if (!IsActive(course.EffectiveTo, course.EffectiveFrom, date))
                return 0;

            var applicableFundingPeriod = course.FundingPeriods.FirstOrDefault(x => IsActive(x.EffectiveTo,x.EffectiveFrom,date));

            return applicableFundingPeriod?.FundingCap ?? 0;
        }
        
        private static bool IsActive(DateTime? effectiveTo,DateTime? effectiveFrom, DateTime date)
        {
            var dateOnly = date.Date;

            if (!effectiveTo.HasValue || effectiveTo.Value >= dateOnly)
                return true;

            return false;
        }
    }
}