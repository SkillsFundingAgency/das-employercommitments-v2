using SFA.DAS.EmployerCommitmentsV2.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class AssignViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public Guid? ReservationId { get; set; }
    public string LegalEntityName { get; set; }
    public Guid? ApprenticeshipSessionKey { get; set; }
    public FundingType? FundingType { get; set; }

    [Required(ErrorMessage = "Select who will add apprentices")]
    public WhoIsAddingApprentices? WhoIsAddingApprentices { get; set; }
    public string Message { get; set; }
}

public enum WhoIsAddingApprentices
{
    Employer,
    Provider
}