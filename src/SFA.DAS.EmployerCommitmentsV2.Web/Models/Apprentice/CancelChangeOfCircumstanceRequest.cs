using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class CancelChangeOfCircumstanceRequest : IAuthorizationContextModel
{
    [FromRoute]
    public string AccountHashedId { get; set; }

    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }
}