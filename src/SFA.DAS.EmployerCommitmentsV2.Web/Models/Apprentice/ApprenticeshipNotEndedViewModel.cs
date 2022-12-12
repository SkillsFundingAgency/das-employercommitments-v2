using SFA.DAS.Authorization.ModelBinding;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ApprenticeshipNotEndedViewModel:IAuthorizationContextModel
    {
        public ApprenticeshipNotEndedViewModel() { }

        public string AccountHashedId { get; set; }

        public string ApprenticeshipHashedId { get; set; }
        public long ApprenticeshipId { get; set; }
    }
}
