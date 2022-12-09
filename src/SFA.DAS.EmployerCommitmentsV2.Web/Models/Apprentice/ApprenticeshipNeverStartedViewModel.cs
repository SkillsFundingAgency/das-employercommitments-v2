using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.Authorization.ModelBinding;
using System.ComponentModel;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticeshipNeverStartedViewModel :IAuthorizationContextModel
    {
        public ApprenticeshipNeverStartedViewModel() { }

        public string AccountHashedId { get; set; }

        public string ApprenticeshipHashedId { get; set; }
        public long ApprenticeshipId { get; set; }

        public DateTime PlannedStartDate { get; set; }

        public int? StartYear { get; set;}
    }
}