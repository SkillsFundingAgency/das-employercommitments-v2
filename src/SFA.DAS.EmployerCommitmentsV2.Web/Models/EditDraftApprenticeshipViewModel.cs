using System;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models
{
    public class EditDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public EditDraftApprenticeshipViewModel(DateTime? dateOfBirth, DateTime? startDate, DateTime? endDate) : base(dateOfBirth, startDate, endDate)
        {
        }

        public EditDraftApprenticeshipViewModel()
        {
        }

        public string AccountHashedId { get; set; }
        public string DraftApprenticeshipHashedId { get; set; }
        public long DraftApprenticeshipId { get; set; }
    }
}