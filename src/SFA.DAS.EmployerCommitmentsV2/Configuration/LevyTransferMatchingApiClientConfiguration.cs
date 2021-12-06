using SFA.DAS.Http.Configuration;

namespace SFA.DAS.EmployerCommitmentsV2.Configuration
{
    public class LevyTransferMatchingApiClientConfiguration : IManagedIdentityClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}
