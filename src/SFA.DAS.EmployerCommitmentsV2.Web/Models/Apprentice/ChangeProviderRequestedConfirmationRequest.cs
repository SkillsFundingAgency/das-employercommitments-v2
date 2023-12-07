using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ChangeProviderRequestedConfirmationRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }

    [FromRoute]
    public long ProviderId{ get; set; }

    public long ApprenticeshipId { get; set; }

    public bool? ProviderAddDetails { get; set; }

    public bool? StoppedDuringCoP { get; set; }
}