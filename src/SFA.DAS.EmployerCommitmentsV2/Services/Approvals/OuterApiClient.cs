using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SFA.DAS.CommitmentsV2.Api.Types.Http;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.Http;
using System.Net.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

public class OuterApiClient : IOuterApiClient
{
    private const string SubscriptionKeyRequestHeaderKey = "Ocp-Apim-Subscription-Key";
    private const string VersionRequestHeaderKey = "X-Version";
    
    private readonly HttpClient _httpClient;
    private readonly ApprovalsApiClientConfiguration _config;
    private readonly ILogger<OuterApiClient> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OuterApiClient(HttpClient httpClient, ApprovalsApiClientConfiguration config, ILogger<OuterApiClient> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Get<TResponse>(string url)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
            
        AddRequestHeaders(request);
            
        using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        if (response.StatusCode.Equals(HttpStatusCode.NotFound))
        {
            return default;
        }

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        response.EnsureSuccessStatusCode();
            
        return default;
    }
    
    private void AddRequestHeaders(HttpRequestMessage request)
    {
        if (_httpContextAccessor.HttpContext.TryGetBearerToken(out var bearerToken))
        {
            request.Headers.Add("Authorization", $"Bearer {bearerToken}");
        }

        request.Headers.Add(SubscriptionKeyRequestHeaderKey, _config.SubscriptionKey);
        request.Headers.Add(VersionRequestHeaderKey, "1");
    }

    public Task<TResponse> Post<TResponse>(string url, object data)
    {
        return PutOrPost<TResponse>(url, data, HttpMethod.Post);
    }

    public Task<TResponse> Put<TResponse>(string url, object data)
    {
        return PutOrPost<TResponse>(url, data, HttpMethod.Put);
    }

    private async Task<TResponse> PutOrPost<TResponse>(string url, object data, HttpMethod method)
    {
        var stringContent = data != null
            ? new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json")
            : null;

        using var requestMessage = new HttpRequestMessage(method, url);
        requestMessage.Content = stringContent;
            
        AddRequestHeaders(requestMessage);

        using var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (IsNot200RangeResponseCode(response.StatusCode))
        {
            //Plug this in when moving another Post endpoint which throws domain errors
            if (response.StatusCode == HttpStatusCode.BadRequest &&
                response.GetSubStatusCode() == HttpSubStatusCode.DomainException)
            {
                throw CreateApiModelException(response, json);
            }

            throw new RestHttpClientException(response, json);
        }

        var responseBody = JsonConvert.DeserializeObject<TResponse>(json);

        return responseBody;
    }

    private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
    {
        return !((int)statusCode >= 200 && (int)statusCode <= 299);
    }

    private Exception CreateApiModelException(HttpResponseMessage httpResponseMessage, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.LogWarning($"{httpResponseMessage.RequestMessage.RequestUri} has returned an empty string when an array of error responses was expected.");
            return new CommitmentsApiModelException(new List<ErrorDetail>());
        }

        var errors = new CommitmentsApiModelException(JsonConvert.DeserializeObject<ErrorResponse>(content).Errors);

        var errorDetails = string.Join(";", errors.Errors.Select(e => $"{e.Field} ({e.Message})"));
        _logger.Log(errors.Errors.Count == 0 ? LogLevel.Warning : LogLevel.Debug, $"{httpResponseMessage.RequestMessage.RequestUri} has returned {errors.Errors.Count} errors: {errorDetails}");

        return errors;
    }
}