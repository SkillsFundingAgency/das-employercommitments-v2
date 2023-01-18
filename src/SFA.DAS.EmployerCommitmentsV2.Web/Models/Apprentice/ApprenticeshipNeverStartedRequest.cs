using System;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticeshipNeverStartedRequest : IAuthorizationContextModel
    {
        [FromRoute]
        public string AccountHashedId { get; set; }

        [FromRoute]
        public string ApprenticeshipHashedId { get; set; }

        public long ApprenticeshipId { get; set; }

        public DateTime PlannedStartDate { get; set; }

        public bool IsCoPJourney { get; set; }

        public int? StopMonth { get; set; }

        public int? StopYear { get; set; }
    }
}