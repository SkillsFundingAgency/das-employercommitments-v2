using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Client;

public interface IApprovalsApiClientFactory
{
    IApprovalsApiClient CreateClient();
}
    
public class ApprovalsApiClientFactory : IApprovalsApiClientFactory
{
    private readonly ApprovalsApiClientConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApprovalsApiClientFactory(ApprovalsApiClientConfiguration configuration, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public IApprovalsApiClient CreateClient()
    {
        var httpClient = new HttpClientBuilder()
            .WithDefaultHeaders()
            .WithApimAuthorisationHeader(_configuration)
            .WithLogging(_loggerFactory)
            .Build();

        httpClient.BaseAddress = !_configuration.ApiBaseUrl.EndsWith('/') 
            ? new Uri($"{_configuration.ApiBaseUrl}/") 
            : new Uri(_configuration.ApiBaseUrl);

        return new ApprovalsApiClient(new OuterApiClient(httpClient, _configuration, _loggerFactory.CreateLogger<OuterApiClient>(), _httpContextAccessor));
    }
}