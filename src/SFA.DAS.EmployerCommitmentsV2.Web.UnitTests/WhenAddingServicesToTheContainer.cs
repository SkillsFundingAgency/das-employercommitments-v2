using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCaseSource(nameof(GetControllerTypes))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Controllers(Type toResolve)
    {
        var serviceCollection = new ServiceCollection();
        SetupServiceCollection(serviceCollection);
        var provider = serviceCollection.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        Assert.That(type, Is.Not.Null);
    }

    private static IEnumerable<Type> GetControllerTypes()
    {
        var mappingAssembly = typeof(ChangeVersionViewModelMapper).Assembly;

        return mappingAssembly
            .GetTypes()
            .Where(type => typeof(Controller).IsAssignableFrom(type));
    }
    
    private static void SetupServiceCollection(IServiceCollection serviceCollection)
    {
        var configuration = GenerateConfiguration();
        var employerCommitmentsV2Configuration = configuration
            .GetSection("EmployerCommitmentsV2Configuration")
            .Get<EmployerCommitmentsV2Configuration>();
        serviceCollection.AddSingleton(Mock.Of<IWebHostEnvironment>());
        serviceCollection.AddSingleton(Mock.Of<IConfiguration>());
        serviceCollection.AddConfigurationOptions(configuration);
        serviceCollection.AddDistributedMemoryCache();
        serviceCollection.AddModelMappings();
        //serviceCollection.AddApplicationServices();

        foreach (var controllerType in GetControllerTypes())
        {
            serviceCollection.AddTransient(controllerType);
        }
    }
    
    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new($"{ConfigurationKeys.Encoding}", "{'Encodings':[{'EncodingType':'AccountId','Salt':'test','MinHashLength':6,'Alphabet':'46789BCDFGHJKLMNPRSTVWXY'}]}"),
                
                new($"{ConfigurationKeys.AuthenticationConfiguration}:Authority", "https://internal.test/"),
                new($"{ConfigurationKeys.AuthenticationConfiguration}:ClientId", "ABC123"),
                new($"{ConfigurationKeys.AuthenticationConfiguration}:ClientSecret", "AABBCCDD"),
                new($"{ConfigurationKeys.AuthenticationConfiguration}:LogoutEndpoint", "https://internal.test/something"),
                new($"{ConfigurationKeys.AuthenticationConfiguration}:MetadataAddress", "https://internal.test/"),
                
                new($"{ConfigurationKeys.CommitmentsApiClientConfiguration}:ApiBaseUrl", "https://internal.test/"),
                new($"{ConfigurationKeys.CommitmentsApiClientConfiguration}:IdentifierUrl", "https://internal.test/"),
                
                new($"{ConfigurationKeys.EmployerFeaturesConfiguration}:FeatureToggles:Feature", "DeliveryModel"),
                new($"{ConfigurationKeys.EmployerFeaturesConfiguration}:FeatureToggles:IsEnabled", "false"),
                
                new($"{ConfigurationKeys.AccountApiConfiguration}:ApiBaseUrl", "https://internal.test/"),
                new($"{ConfigurationKeys.AccountApiConfiguration}:IdentifierUrl", "https://internal.test/"),
                
                new($"{ConfigurationKeys.ZenDeskConfiguration}:SectionId", "235345345"),
                new($"{ConfigurationKeys.ZenDeskConfiguration}:SnippetKey", Guid.NewGuid().ToString()),
                new($"{ConfigurationKeys.ZenDeskConfiguration}:CobrowsingSnippetKey", "ABC123"),
                
                new($"{ConfigurationKeys.ApprovalsApiClientConfiguration}:ApiBaseUrl", "https://internal.test/"),
                new($"{ConfigurationKeys.ApprovalsApiClientConfiguration}:ApiVersion", "1"),
                new($"{ConfigurationKeys.ApprovalsApiClientConfiguration}:SubscriptionKey", "test"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}