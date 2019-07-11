using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.AddDraftApprenticeshipToNewCohort
{
    public class StartRequest
    {
        [FromRoute]
        public string AccountId { get; set; }
        
        public string ReservationId { get; set; }
        
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        
        public string StartMonthYear { get; set; }
        
        public string CourseCode { get; set; }

        public virtual Dictionary<string, string> ToDictionary()
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

    public class SelectProviderRequest : StartRequest
    {


    }

    public class ConfirmProviderRequest : SelectProviderRequest
    {
        public long ProviderId { get; set; }

        public override Dictionary<string, string> ToDictionary()
        {
            var result = base.ToDictionary();

            result.Add("ProviderId", ProviderId.ToString());

            return result;
        }
    }



}