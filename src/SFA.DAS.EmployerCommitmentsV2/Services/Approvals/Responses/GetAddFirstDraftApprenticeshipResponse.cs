namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetAddFirstDraftApprenticeshipResponse
{
    public string LegalEntityName { get; set; }
    public string ProviderName { get; set; }
    public bool HasMultipleDeliveryModelOptions { get; set; }
    public string StandardPageUrl { get; set; }
    public int? ProposedMaxFunding { get; set; }
}