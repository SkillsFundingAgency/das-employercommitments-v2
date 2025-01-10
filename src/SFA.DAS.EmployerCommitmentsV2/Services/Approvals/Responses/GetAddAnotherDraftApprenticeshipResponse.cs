namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetAddAnotherDraftApprenticeshipResponse
{
    public long AccountLegalEntityId { get; set; }
    public string LegalEntityName { get; set; }
    public string ProviderName { get; set; }
    public bool HasMultipleDeliveryModelOptions { get; set; }
    public bool IsFundedByTransfer { get; set; }
    public long? TransferSenderId { get; set; }
    public string StandardPageUrl { get; set; }
    public int? ProposedMaxFunding { get; set; }
}