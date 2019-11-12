using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Enums;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship
{
    public class DeleteDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public string DraftApprenticeshipHashedId { get; set; }
        public long? DraftApprenticeshipId { get; set; }
        public string BackLink { get; set; }
        public bool? ConfirmDelete { get; set; }
        public Origin Origin { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string RedirectToOriginUrl { get; internal set; }
    }
}
