using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.CommitmentsV2.Api.Client.Http;
using SFA.DAS.EmployerCommitmentsV2.Client;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.LocalDevRegistry;

public class LocalDevApiClientFactory : ICommitmentsApiClientFactory, ICommitmentPermissionsApiClientFactory
{
    private readonly CommitmentsClientApiConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public LocalDevApiClientFactory(CommitmentsClientApiConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    public ICommitmentsApiClient CreateClient()
    {
        var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (value == "Development")
        {
            var httpClientBuilder = new HttpClientBuilder();
            var httpClient = httpClientBuilder
                .WithDefaultHeaders()
                .Build();

            httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);
            var byteArray = System.Text.Encoding.ASCII.GetBytes($"employer:password1234");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var restHttpClient = new CommitmentsRestHttpClient(httpClient, _loggerFactory);
            return new CommitmentsApiClient(restHttpClient);
        }

        throw new UnauthorizedAccessException("Not accessible");
    }

    ICommitmentPermissionsApiClient ICommitmentPermissionsApiClientFactory.CreateClient()
    {
        var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (value == "Development")
        {
            var httpClientBuilder = new HttpClientBuilder();
            var httpClient = httpClientBuilder
                .WithDefaultHeaders()
                .Build();

            httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);

            var restHttpClient = new CommitmentsRestHttpClient(httpClient, _loggerFactory);
            return new CommitmentPermissionsApiClient(restHttpClient);
        }

        throw new UnauthorizedAccessException("Not accessible");
    }
}