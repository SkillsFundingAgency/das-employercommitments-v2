namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit
{
    public class ReviewApprenticeshipUpdatesRequestViewModel : BaseEdit
    {
        public bool? ApproveChanges { get; set; }

        public string ProviderName { get; set; }

        public BaseEdit OriginalApprenticeship { get; set; }
    }
}
