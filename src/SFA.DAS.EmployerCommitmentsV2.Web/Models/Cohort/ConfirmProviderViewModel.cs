namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class ConfirmProviderViewModel : IndexViewModel
{
    public long ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string LegalEntityName { get; set; }
    public bool? UseThisProvider { get; set; }
    public FundingType? FundingType { get; set; }
}