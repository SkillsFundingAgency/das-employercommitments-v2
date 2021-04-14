using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ConfirmEditApprenticeshipViewModel : BaseConfirmEdit
    {
        public bool? ConfirmChanges { get; set; }

        public BaseConfirmEdit OriginalApprenticeship { get; set; }
    }
}
