using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution
{
    public class LevyTransferMatchingApiClientRegistry : Registry
    {
        public LevyTransferMatchingApiClientRegistry()
        {
            For<ILevyTransferMatchingApiClient>().Use(c => c.GetInstance<ILevyTransferMatchingApiClientFactory>().CreateClient()).Singleton();
            For<ILevyTransferMatchingApiClientFactory>().Use<LevyTransferMatchingApiClientFactory>();
        }
    }
}
