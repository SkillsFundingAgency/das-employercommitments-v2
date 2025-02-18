using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;
using SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Stubs;
using SFA.DAS.EmployerUrlHelper.Configuration;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCaseSource(nameof(GetControllerTypes))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Controllers(TestType toResolve)
    {
        RunTestForType(toResolve);
    }

    [TestCaseSource(nameof(GetModelMappingTypes))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_ModelMappers(TestType toResolve)
    {
        RunTestForType(toResolve);
    }

    [TestCase(typeof(CommitmentPermissionsApiClientConfiguration))]
    [TestCase(typeof(ZenDeskConfiguration))]
    [TestCase(typeof(CommitmentsClientApiConfiguration))]
    [TestCase(typeof(CommitmentPermissionsApiClientConfiguration))]
    [TestCase(typeof(EncodingConfig))]
    [TestCase(typeof(EmployerUrlHelperConfiguration))]
    [TestCase(typeof(ApprovalsApiClientConfiguration))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Configuration(Type toResolve)
    {
        RunTestForType(new TestType(toResolve.Name, toResolve));
    }

    [TestCase(typeof(IAuthorizationContext))]
    [TestCase(typeof(IAuthorizationContextProvider))]
    [TestCase(typeof(ICommitmentsAuthorisationHandler))]
    [TestCase(typeof(IEmployerAccountAuthorisationHandler))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Authorization_Services(Type toResolve)
    {
        RunTestForType(new TestType(toResolve.Name, toResolve));
    }

    private static void RunTestForType(TestType toResolve)
    {
        var serviceCollection = new ServiceCollection();
        SetupServiceCollection(serviceCollection);
        var provider = serviceCollection.BuildServiceProvider();

        var type = provider.GetService(toResolve.Type);

        type.Should().NotBeNull($"Unable to resolve all required services for type '{toResolve.Name}'.");
    }

    private static IEnumerable<TestType> GetControllerTypes()
    {
        var mappingAssembly = typeof(HomeController).Assembly;

        var types = mappingAssembly
            .GetTypes()
            .Where(type => typeof(Controller).IsAssignableFrom(type));

        return types.Select(x => new TestType(x.Name, x));
    }

    public record TestType(string Name, Type Type);

    private static IEnumerable<TestType> GetModelMappingTypes()
    {
        var mappingAssembly = typeof(ChangeVersionViewModelMapper).Assembly;

        var mappingTypes = mappingAssembly
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>)));

        foreach (var mapperType in mappingTypes.Where(x => x != typeof(AttachUserInfoToSaveRequests<,>)))
        {
            var interfaceType = mapperType
                .GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>));

            yield return new TestType(mapperType.Name, interfaceType);
        }
    }

    private static void SetupServiceCollection(IServiceCollection services)
    {
        var mockHostEnvironment = new Mock<IHostEnvironment>();

        var configuration = GenerateConfiguration();
        var employerCommitmentsV2Configuration = configuration
            .GetSection(ConfigurationKeys.EmployerCommitmentsV2);

        services.AddLogging();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddSingleton(Mock.Of<IWebHostEnvironment>());
        services.AddSingleton(Mock.Of<IConfiguration>());
        services.AddConfigurationOptions(configuration);
        services.AddDistributedMemoryCache();
        services.AddModelMappings();
        services.AddCommitmentPermissionsApiClient(configuration);
        services.AddApplicationServices();
        services.AddModelMappings();
        services.AddDasMvc();
        
        services.AddTransient<IUrlHelper, StubUrlHelper>();
        services.AddTransient<UrlBuilder>(_ => new UrlBuilder("test"));

        services.AddCommitmentsApiClient(configuration);
        services.AddApprovalsApiClient();
        services.AddEncodingServices(configuration);
        services.AddAuthorizationServices(configuration);
        services.AddDasEmployerAuthentication(configuration);
        services.AddDasDataProtection(configuration, mockHostEnvironment.Object);

        foreach (var controllerType in GetControllerTypes())
        {
            services.AddTransient(controllerType.Type);
        }
    }

    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new($"{ConfigurationKeys.Encoding}", "{'Encodings':[{'EncodingType':'AccountId','Salt':'test','MinHashLength':6,'Alphabet':'46789BCDFGHJKLMNPRSTVWXY'}]}"),

                new($"{ConfigurationKeys.EmployerUrlConfiguration}:AccountsBaseUrl", "https://local.test/"),

                new($"{ConfigurationKeys.EmployerCommitmentsV2}:UseStubEmployerAccountsApiClient", "true"),
                new($"{ConfigurationKeys.EmployerCommitmentsV2}:UseGovSignIn", "true"),

                new("StubAuth", "true"),
                new("ResourceEnvironmentName", "LOCAL"),

                new($"{ConfigurationKeys.GovUkSignInConfiguration}:BaseUrl", "https://internal.test/"),
                new($"{ConfigurationKeys.GovUkSignInConfiguration}:ClientId", "SDFDFDF"),
                new($"{ConfigurationKeys.GovUkSignInConfiguration}:KeyVaultIdentifier", "1223445"),

                new($"{ConfigurationKeys.AuthenticationConfiguration}:Authority", "https://internal.test/"),
                new($"{ConfigurationKeys.AuthenticationConfiguration}:ClientId", "ABC123"),
                new($"{ConfigurationKeys.AuthenticationConfiguration}:ClientSecret", "AABBCCDD"),
                new($"{ConfigurationKeys.AuthenticationConfiguration}:LogoutEndpoint", "https://internal.test/something"),
                new($"{ConfigurationKeys.AuthenticationConfiguration}:MetadataAddress", "https://internal.test/"),

                new($"{ConfigurationKeys.CommitmentsApiClientConfiguration}:ApiBaseUrl", "https://internal.test/"),
                new($"{ConfigurationKeys.CommitmentsApiClientConfiguration}:IdentifierUrl", "https://internal.test/"),

                new($"{ConfigurationKeys.EmployerFeaturesConfiguration}:FeatureToggles:Feature", "DeliveryModel"),
                new($"{ConfigurationKeys.EmployerFeaturesConfiguration}:FeatureToggles:IsEnabled", "false"),

                new($"{ConfigurationKeys.ZenDeskConfiguration}:SectionId", "235345345"),
                new($"{ConfigurationKeys.ZenDeskConfiguration}:SnippetKey", Guid.NewGuid().ToString()),
                new($"{ConfigurationKeys.ZenDeskConfiguration}:CobrowsingSnippetKey", "ABC123"),

                new($"{ConfigurationKeys.ApprovalsApiClientConfiguration}:ApiBaseUrl", "https://internal.test/"),
                new($"{ConfigurationKeys.ApprovalsApiClientConfiguration}:ApiVersion", "1"),
                new($"{ConfigurationKeys.ApprovalsApiClientConfiguration}:SubscriptionKey", "test"),
                
                new($"{ConfigurationKeys.GovUkSignInConfiguration}:GovUkOidcConfiguration:BaseUrl", "https://internal.test/"),
                new($"{ConfigurationKeys.GovUkSignInConfiguration}:GovUkOidcConfiguration:ClientId", "test"),
                new($"{ConfigurationKeys.GovUkSignInConfiguration}:GovUkOidcConfiguration:KeyVaultIdentifier", "test"),

                new($"{ConfigurationKeys.UserBearerTokenSigningKeyConfiguration}", "a-string-of-at-least-32-characters")
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}