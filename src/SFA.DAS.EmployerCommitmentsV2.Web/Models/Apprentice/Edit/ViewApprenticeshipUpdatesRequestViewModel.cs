namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit
{
    public class ViewApprenticeshipUpdatesRequestViewModel 
    {
        public ViewApprenticeshipUpdatesRequestViewModel()
        {
            ApprenticeshipUpdates = new BaseEdit();
            OriginalApprenticeship = new BaseEdit();
        }

        public BaseEdit ApprenticeshipUpdates { get; set; }

        public bool? ApproveChanges { get; set; }

        public string ProviderName { get; set; }

        public BaseEdit OriginalApprenticeship { get; set; }
    }
}
