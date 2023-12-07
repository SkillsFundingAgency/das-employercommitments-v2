using Microsoft.Extensions.Configuration;
using SFA.DAS.Authorization.EmployerUserRoles.DependencyResolution.StructureMap;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.CommitmentsV2.Shared.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.DependencyResolution;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry, IConfiguration config)
        {
            //registry.IncludeRegistry<AutoConfigurationRegistry>();
            //registry.IncludeRegistry<CommitmentsSharedRegistry>();
            registry.IncludeRegistry<EmployerAccountsRegistry>();
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<EncodingRegistry>();
            registry.IncludeRegistry<EmployerUserRolesAuthorizationRegistry>();
            registry.IncludeRegistry<WebRegistry>();
            registry.IncludeRegistry<ApprovalsApiClientRegistry>();
            registry.IncludeRegistry(new GovUkStructureMap($"SFA.DAS.Employer.GovSignIn:{nameof(GovUkOidcConfiguration)}"));
            
            // Enable in appsettings if you want to bypass MI when developing locally
            if (config["UseLocalDevRegistry"] != null && bool.Parse(config["UseLocalDevRegistry"]))
            {
                registry.IncludeRegistry<LocalDevRegistry.LocalDevRegistry>();
            }
        }
    }
}