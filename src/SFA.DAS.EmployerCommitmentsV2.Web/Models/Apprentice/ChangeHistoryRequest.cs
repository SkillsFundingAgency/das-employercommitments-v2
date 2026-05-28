using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ChangeHistoryRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }

    [FromRoute]
    public string AccountHashedId { get; set; }
}