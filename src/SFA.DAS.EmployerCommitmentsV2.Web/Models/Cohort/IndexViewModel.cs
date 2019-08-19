using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class IndexViewModel
    {
        public string AccountHashedId { get; set; }
        public string ReservationId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
        public virtual Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>
            {
                {nameof(AccountHashedId), AccountHashedId },
                {nameof(AccountLegalEntityHashedId), AccountLegalEntityHashedId }
            };
            
            if (!string.IsNullOrWhiteSpace(ReservationId))
                dictionary.Add(nameof(ReservationId), ReservationId);
            if (!string.IsNullOrWhiteSpace(StartMonthYear))
                dictionary.Add(nameof(StartMonthYear), StartMonthYear);
            if (!string.IsNullOrWhiteSpace(CourseCode))
                dictionary.Add(nameof(CourseCode), CourseCode);

            return dictionary;
        }
    }
}