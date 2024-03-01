using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Client;

public interface ICommitmentPermissionsApiClientFactory
{
    ICommitmentPermissionsApiClient CreateClient();
}

public class  CommitmentPermissionsApiClientFactory : ICommitmentPermissionsApiClientFactory
{
    private readonly CommitmentPermissionsApiClientConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public CommitmentPermissionsApiClientFactory(CommitmentPermissionsApiClientConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    public ICommitmentPermissionsApiClient CreateClient()
    {
        var httpClientFactory = new ManagedIdentityHttpClientFactory(_configuration, _loggerFactory);
        var httpClient = httpClientFactory.CreateHttpClient();
        var restHttpClient = new RestHttpClient(httpClient);

        return new CommitmentPermissionsApiClient(restHttpClient);
    }
}