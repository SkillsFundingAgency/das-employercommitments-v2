using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ConfirmHasNotStopViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }

    public long AccountId { get; set; }

    public string ApprenticeshipHashedId { get; set; }

    public long ApprenticeshipId { get; set; }

    public string ApprenticeName { get; set; }

    public string ULN { get; set; }

    public string Course { get; set; }

    public bool? StopConfirmed { get; set; }
}