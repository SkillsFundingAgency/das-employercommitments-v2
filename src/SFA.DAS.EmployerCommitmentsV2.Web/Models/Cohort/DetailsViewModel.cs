using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort
{
    public class DetailsViewModel : IAuthorizationContextModel
    {
        public string AccountHashedId { get; set; }
        public Party WithParty { get; set; }
        public string CohortReference { get; set; }
        public long CohortId { get; set; }
        public string AccountLegalEntityHashedId { get; set; }
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
        public string TransferSenderHashedId { get; set; }
        public int DraftApprenticeshipsCount => Courses?.SelectMany(c => c.DraftApprenticeships).Count() ?? 0;
        public IReadOnlyCollection<DetailsViewCourseGroupingModel> Courses { get; set; }
        public string PageTitle { get; set; }
        public CohortDetailsOptions? Selection { get; set; }
        public string SendMessage { get; set; }
        public string ApproveMessage { get; set; }
        public bool IsApprovedByProvider { get; set; }
        public int TotalCost => Courses?.Sum(g => g.DraftApprenticeships.Sum(a => a.Cost ?? 0)) ?? 0;
        public string DisplayTotalCost => TotalCost.ToGdsCostFormat();
        public bool IsAgreementSigned { get; set; }
        public string OptionsTitle => IsAgreementSigned && IsCompleteForEmployer && !HasEmailOverlaps ? "Approve these details?": "Choose an option";
        public bool ShowViewAgreementOption => !IsAgreementSigned;
        public bool EmployerCanApprove => IsAgreementSigned && IsCompleteForEmployer && !HasOverlappingUln && !HasEmailOverlaps;
        public bool ShowApprovalOptionMessage => EmployerCanApprove && IsApprovedByProvider;
        public bool ShowGotoHomePageOption => (!IsCompleteForEmployer && IsAgreementSigned) || HasEmailOverlaps;
        public bool IsReadOnly => WithParty != Party.Employer;
        public bool IsCompleteForEmployer { get; set; }
        public bool HasEmailOverlaps { get; set; }
        public bool ShowAddAnotherApprenticeOption { get; set; }
        public string SendBackToProviderOptionMessage
        {
            get
            {
                if (!IsAgreementSigned)
                {
                    return "Send to the training provider to review or add details";
                }
                if (!EmployerCanApprove)
                {
                    return "Request changes from training provider";
                }
                return "No, request changes from training provider";
            }
        }

        public bool HasOverlappingUln
        {
            get
            {
                return Courses != null
                    && Courses.Any(x => x.DraftApprenticeships != null
                    && x.DraftApprenticeships.Any(x => x.HasOverlappingUln));
            }
        }
    }

    public enum CohortDetailsOptions
    {
        Send,
        Approve,
        ViewEmployerAgreement, 
        Homepage
    }
}
