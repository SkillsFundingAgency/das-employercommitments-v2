namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class WithTrainingProviderCohortSummaryViewModel
{
    public long ProviderId { get; set; }
    public string CohortReference { get; set; }
    public string ProviderName { get; set; }
    public int NumberOfApprentices { get; set; }
    public string LastMessage { get; set; }
}