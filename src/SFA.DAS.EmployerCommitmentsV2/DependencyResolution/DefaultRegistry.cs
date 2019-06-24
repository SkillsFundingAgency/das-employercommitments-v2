using SFA.DAS.ProviderUrlHelper;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS.EmployerCommitmentsV2"));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<ILinkGenerator>().Use<LinkGenerator>().Singleton();
        }
    }
}