namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class HowToAddLearnersViewModel
{
    public string AccountHashedId { get; set; }
    public bool IsLevyFunded { get; set; }
    public Guid? ApprenticeshipSessionKey { get; set; }
    public string ContinueAction { get; set; }
}
