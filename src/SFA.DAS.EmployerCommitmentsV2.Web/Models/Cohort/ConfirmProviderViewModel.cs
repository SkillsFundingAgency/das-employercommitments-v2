﻿namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class ConfirmProviderViewModel : IndexViewModel
{
    public long ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string LegalEntityName { get; set; }

    public bool? UseThisProvider { get; set; }
    public string TransferSenderId { get; set; }
    public string EncodedPledgeApplicationId { get; set; }

    public override Dictionary<string, string> ToDictionary(bool includeCacheKey = false)
    {
        var result = base.ToDictionary(includeCacheKey);

        result.Add("ProviderId", ProviderId.ToString());

        return result;
    }
}