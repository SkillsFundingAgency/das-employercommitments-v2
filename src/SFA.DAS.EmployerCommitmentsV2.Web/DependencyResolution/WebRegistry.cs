using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Services.Stubs;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using StructureMap;
using StructureMap.Building.Interception;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<IEmployerAccountsApiClient>().Use<StubEmployerAccountsApiClient>();
            //Toggle<IEmployerAccountsApiClient, StubEmployerAccountsApiClient>("UseStubEmployerAccountsApiClient");
        }

        private void Toggle<TPluginType, TConcreteType>(string configurationKey) where TConcreteType : TPluginType
        {
            For<TPluginType>().InterceptWith(new FuncInterceptor<TPluginType>((c, o) => c.GetInstance<IConfiguration>().GetValue<bool>(configurationKey) ? c.GetInstance<TConcreteType>() : o));
        }
    }
}