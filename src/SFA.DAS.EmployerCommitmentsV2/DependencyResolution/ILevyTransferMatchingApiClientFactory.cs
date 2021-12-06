using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution
{
    public interface ILevyTransferMatchingApiClientFactory
    {
        ILevyTransferMatchingApiClient CreateClient();
    }
}
