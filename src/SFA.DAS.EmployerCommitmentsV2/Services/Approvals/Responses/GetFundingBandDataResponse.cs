﻿namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetFundingBandDataResponse
{
    public int LarsCode { get; set; }
    public string StandardUId { get; set; }
    public string Version { get; set; }
    public string StandardPageUrl { get; set; }
    public int ProposedMaxFunding { get; set; }
}