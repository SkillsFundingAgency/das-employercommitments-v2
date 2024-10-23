using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ConfirmEditApprenticeshipRequest : BaseEdit
{    
    public Guid? CacheKey { get; set; }
}