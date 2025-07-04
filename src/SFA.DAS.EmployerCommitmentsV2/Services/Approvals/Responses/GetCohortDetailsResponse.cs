﻿namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetCohortDetailsResponse
{
    public string LegalEntityName { get; set; }
    public string ProviderName { get; set; }
    public long ProviderId { get; set; }
    public bool HasUnavailableFlexiJobAgencyDeliveryModel { get; set; }
    public bool HasFoundationApprenticeships { get; set; }
}