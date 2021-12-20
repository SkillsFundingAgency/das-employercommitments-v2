using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution
{
    public class ApprovalsApiClientRegistry : Registry
    {
        public ApprovalsApiClientRegistry()
        {
            For<IApprovalsApiClient>().Use(c => c.GetInstance<IApprovalsApiClientFactory>().CreateClient()).Singleton();
            For<IApprovalsApiClientFactory>().Use<ApprovalsApiClientFactory>();
        }
    }
}
