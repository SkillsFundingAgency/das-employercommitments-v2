using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.AddDraftApprenticeshipToNewCohort
{
    public class RouteModel
    {
        [FromRoute]
        public string AccountId { get; set; }
        [FromQuery]
        public string ReservationId { get; set; }
        [FromQuery]
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        [FromQuery]
        public string StartMonthYear { get; set; }
        [FromQuery]
        public string CourseCode { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>
            {
                {nameof(AccountId), AccountId },
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