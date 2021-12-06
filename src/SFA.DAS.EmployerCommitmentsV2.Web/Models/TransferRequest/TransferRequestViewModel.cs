using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest
{
    public class TransferRequestViewModel
    {
        public TransferRequestViewModel()
        {
            TrainingList = new List<TrainingCourseSummaryViewModel>();
        }

        public string AccountHashedId { get; set; }
        public string TransferRequestHashedId { get; set; }
        public string HashedCohortReference { get; set; }
        public decimal TotalCost { get; set; }
        public int FundingCap { get; set; }
        public List<TrainingCourseSummaryViewModel> TrainingList { get; set; }
        public string TransferApprovalStatusDesc { get; set; }
        public TransferApprovalStatus TransferApprovalStatus { get; set; }
        public string TransferApprovalSetBy { get; set; }
        public DateTime? TransferApprovalSetOn { get; set; }
        public bool PendingApproval => TransferApprovalStatus == TransferApprovalStatus.Pending;
        public bool ShowFundingCapWarning { get; set; }
        public bool? ApprovalConfirmed { get; set; }
        public bool AutoApprovalEnabled { get; set; }
        public string HashedPledgeId { get; set; }
        public string HashedPledgeApplicationId { get; set; }
        public bool AutomaticallyApproved => AutoApprovalEnabled && TransferApprovalStatus == TransferApprovalStatus.Approved;
        public bool AutomaticallyRejected => AutoApprovalEnabled && TransferApprovalStatus == TransferApprovalStatus.Rejected;
    }
}
