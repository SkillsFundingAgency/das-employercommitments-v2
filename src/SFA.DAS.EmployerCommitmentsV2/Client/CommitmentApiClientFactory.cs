using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Client.Configuration;
using SFA.DAS.CommitmentsV2.Api.Client.Http;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Client;

public class LocalDevApiClientFactory(CommitmentsClientApiConfiguration configuration, ILoggerFactory loggerFactory)
    : ICommitmentsApiClientFactory, ICommitmentPermissionsApiClientFactory
{
    public ICommitmentsApiClient CreateClient()
    {
        var value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (value == "Development")
        {
            var httpClientBuilder = new HttpClientBuilder();
            var httpClient = httpClientBuilder
                .WithDefaultHeaders()
                .Build();

            httpClient.BaseAddress = new Uri(configuration.ApiBaseUrl);
            var byteArray = System.Text.Encoding.ASCII.GetBytes($"employer:password1234");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var restHttpClient = new CommitmentsRestHttpClient(httpClient, loggerFactory);
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

            httpClient.BaseAddress = new Uri(configuration.ApiBaseUrl);

            var restHttpClient = new CommitmentsRestHttpClient(httpClient, loggerFactory);
            return new CommitmentPermissionsApiClient(restHttpClient);
        }

        throw new UnauthorizedAccessException("Not accessible");
    }
}