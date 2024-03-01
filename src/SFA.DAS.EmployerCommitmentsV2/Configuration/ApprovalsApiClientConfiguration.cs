using SFA.DAS.Http.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Configuration;

public class ApprovalsApiClientConfiguration : IApimClientConfiguration
{
    public string ApiBaseUrl { get; set; }

    public string SubscriptionKey { get; set; }

    public string ApiVersion { get; set; }
}