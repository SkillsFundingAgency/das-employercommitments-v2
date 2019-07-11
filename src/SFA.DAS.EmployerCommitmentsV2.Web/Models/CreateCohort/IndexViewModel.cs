using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort
{
    public class IndexViewModel
    {
        public string AccountHashedId { get; set; }
        public string ReservationId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }

        public virtual Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>
            {
                {nameof(AccountHashedId), AccountHashedId },
                {nameof(EmployerAccountLegalEntityPublicHashedId), EmployerAccountLegalEntityPublicHashedId }
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