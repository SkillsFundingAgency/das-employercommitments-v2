namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ConfirmStopRequestViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }

    public long AccountId { get; set; }

    public string ApprenticeshipHashedId { get; set; }

    public long ApprenticeshipId { get; set; }

    public int? StopMonth { get; set ; }
        
    public int? StopYear { get; set; }

    public bool IsCoPJourney { get; set; }

    public bool? MadeRedundant { get; set; }

    public string ApprenticeName { get; set; }

    public DateTime StopDate { get; set; }

    public string ULN { get; set; }

    public string Course { get; set; }

    public bool? StopConfirmed { get; set; }
}