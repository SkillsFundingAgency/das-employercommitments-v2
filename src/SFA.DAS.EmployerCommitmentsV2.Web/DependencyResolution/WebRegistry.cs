using SFA.DAS.CommitmentsV2.Api.Client;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<ICommitmentsApiClientFactory>().Use<CommitmentsApiClientFactory>();
        }
    }
}