using Microsoft.AspNetCore.Http;
using SFA.DAS.CommitmentsV2.Services.Shared;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Services;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Services.Stubs;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.Http.Configuration;
using StructureMap;
using StructureMap.Building.Interception;

namespace SFA.DAS.EmployerCommitmentsV2.Web.DependencyResolution
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For(typeof(IMapper<,>)).DecorateAllWith(typeof(AttachUserInfoToSaveRequests<,>));
            For<IModelMapper>().Use<ModelMapper>();
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<StubEmployerAccountsApiClient>().Use<StubEmployerAccountsApiClient>().Singleton();
            For<IEmployerAccountsApiClient>().InterceptWith(new FuncInterceptor<IEmployerAccountsApiClient>(
                (c, o) => c.GetInstance<EmployerCommitmentsV2Configuration>().UseStubEmployerAccountsApiClient ? c.GetInstance<StubEmployerAccountsApiClient>() : o));
            For(typeof(ICookieStorageService<>)).Use(typeof(CookieStorageService<>)).Singleton();
            For(typeof(HttpContext)).Use(c => c.GetInstance<IHttpContextAccessor>().HttpContext);
            For<IAcademicYearDateProvider>().Use<AcademicYearDateProvider>().Singleton();
        }
    }
}