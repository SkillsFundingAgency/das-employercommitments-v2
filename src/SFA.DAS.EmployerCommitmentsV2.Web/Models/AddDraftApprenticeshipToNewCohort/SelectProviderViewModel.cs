using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.AddDraftApprenticeshipToNewCohort
{
    public class SelectProviderViewModel
    {
       
        public string AccountId { get; set; }
       
        public string ReservationId { get; set; }
       
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
       
       public string StartMonthYear { get; set; }

        public string CourseCode { get; set; }

        public long ProviderId { get; set; }

    }
}
