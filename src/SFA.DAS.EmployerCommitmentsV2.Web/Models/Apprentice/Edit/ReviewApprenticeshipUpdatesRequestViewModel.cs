namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit
{
    public class ReviewApprenticeshipUpdatesRequestViewModel : BaseConfirmEdit
    {
        public bool? ApproveChanges { get; set; }

        public string ProviderName { get; set; }

        public BaseConfirmEdit OriginalApprenticeship { get; set; }
    }
}
