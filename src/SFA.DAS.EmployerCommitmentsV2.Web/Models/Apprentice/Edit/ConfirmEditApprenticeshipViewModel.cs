using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ConfirmEditApprenticeshipViewModel : BaseEdit
{
    public bool? ConfirmChanges { get; set; }
    public BaseEdit OriginalApprenticeship { get; set; }

    public bool ReturnToChangeOption { get; set; }
    public bool ReturnToChangeVersion { get; set; }
    public Guid? CacheKey { get; set; }
}