using SFA.DAS.EmployerCommitmentsV2.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}