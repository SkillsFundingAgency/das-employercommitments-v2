using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public class DeleteDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string DraftApprenticeshipHashedId { get; set; }
        public long? DraftApprenticeshipId { get; set; }
        public bool IsLastApprenticeshipInCohort { get; set; }
        public bool? ConfirmDelete { get; set; }
        [FromQuery]
        public DeleteDraftApprenticeshipOrigin Origin { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
