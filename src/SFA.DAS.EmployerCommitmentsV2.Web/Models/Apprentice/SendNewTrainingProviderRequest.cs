namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class SendNewTrainingProviderRequest : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public long AccountId { get; set; }
    public string ApprenticeshipHashedId { get; set; }
    public long ApprenticeshipId { get; set; }
    public long ProviderId { get; set; }
    public bool? StoppedDuringCoP { get; set; }
}