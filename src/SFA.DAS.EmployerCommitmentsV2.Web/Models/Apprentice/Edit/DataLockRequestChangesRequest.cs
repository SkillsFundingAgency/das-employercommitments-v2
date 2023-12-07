using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

public class DataLockRequestChangesRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    public long AccountId { get; set; }

    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }

    public long ApprenticeshipId { get; set; }
}