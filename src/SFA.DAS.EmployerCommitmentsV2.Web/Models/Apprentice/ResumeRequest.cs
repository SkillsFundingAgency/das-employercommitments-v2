using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ResumeRequest : IAuthorizationContextModel
    {
        [FromRoute]
        public string AccountHashedId { get; set; }
        [FromRoute]
        public string ApprenticeshipHashedId { get; set; }
        public long ApprenticeshipId { get; set; }
    }
}
