using SFA.DAS.Authorization.EmployerUserRoles.DependencyResolution;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.CommitmentsV2.Api.Client.DependencyResolution;
using SFA.DAS.EmployerCommitmentsV2.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<AutoConfigurationRegistry>();
            registry.IncludeRegistry<CommitmentsApiClientRegistry>();
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<EncodingRegistry>();
            registry.IncludeRegistry<EmployerUserRolesAuthorizationRegistry>();
            registry.IncludeRegistry<WebRegistry>();
        }
    }
}