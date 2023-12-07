using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Factories;

public interface IApprovalsApiClientFactory
{
    IApprovalsApiClient CreateClient();
}
    
public class ApprovalsApiClientFactory : IApprovalsApiClientFactory
{
    private readonly ApprovalsApiClientConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public ApprovalsApiClientFactory(ApprovalsApiClientConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    public IApprovalsApiClient CreateClient()
    {
        var httpClient = new HttpClientBuilder()
            .WithDefaultHeaders()
            .WithApimAuthorisationHeader(_configuration)
            .WithLogging(_loggerFactory)
            .Build();

        httpClient.BaseAddress = !_configuration.ApiBaseUrl.EndsWith("/") 
            ? new Uri(_configuration.ApiBaseUrl + "/") 
            : new Uri(_configuration.ApiBaseUrl);

        return new ApprovalsApiClient(new OuterApiClient(httpClient, _configuration, _loggerFactory.CreateLogger<OuterApiClient>()));
    }
}