using SFA.DAS.CommitmentsV2.Api.Client;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<ICommitmentsApiClientFactory>().Use<CommitmentsApiClientFactory>();
        }
    }
}