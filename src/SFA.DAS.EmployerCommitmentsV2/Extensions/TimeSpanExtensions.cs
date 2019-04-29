using System;

namespace SFA.DAS.EmployerCommitmentsV2.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToHealthCheckString(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalSeconds <= 1)
            {
                return $"{timeSpan:%f} ms";
            }
            
            if (timeSpan.TotalMinutes <= 1)
            {
                return $"{timeSpan:%s} seconds";
            }

            if (timeSpan.TotalHours <= 1)
            {
                return $"{timeSpan:%m} minutes";
            }

            if (timeSpan.TotalDays <= 1)
            {
                return $"{timeSpan:%h} hours";
            }

            return $"{timeSpan:%d} days";
        }
    }
}