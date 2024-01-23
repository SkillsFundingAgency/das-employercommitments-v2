using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFA.DAS.CommitmentsV2.Api.Types.Http;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals
{
    public class OuterApiClient : IOuterApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ApprovalsApiClientConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILogger<OuterApiClient> _logger;
        const string SubscriptionKeyRequestHeaderKey = "Ocp-Apim-Subscription-Key";
        const string VersionRequestHeaderKey = "X-Version";

        public OuterApiClient(HttpClient httpClient, ApprovalsApiClientConfiguration config, ILogger<OuterApiClient> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Get<TResponse>(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeaders(requestMessage);

            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

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

        private void AddHeaders(HttpRequestMessage httpRequestMessage)
        {
            if (_httpContextAccessor.HttpContext.TryGetBearerToken(out var bearerToken))
            {
                httpRequestMessage.Headers.Add("Authorization", $"Bearer {bearerToken}");
            }

            //The http handler life time is set to 5 minutes
            //hence once the headers are added they don't need added again
            if (_httpClient.DefaultRequestHeaders.Contains(SubscriptionKeyRequestHeaderKey)) return;

            _httpClient.DefaultRequestHeaders.Add(SubscriptionKeyRequestHeaderKey, _config.SubscriptionKey);
            _httpClient.DefaultRequestHeaders.Add(VersionRequestHeaderKey, "1");
        }

        public async Task<TResponse> Post<TResponse>(string url, object data)
        {
            return await PutOrPost<TResponse>(url, data, HttpMethod.Post);
        }

        public async Task<TResponse> Put<TResponse>(string url, object data)
        {
            return await PutOrPost<TResponse>(url, data, HttpMethod.Put);
        }

        private async Task<TResponse> PutOrPost<TResponse>(string url, object data, HttpMethod method)
        {
            var requestMessage = new HttpRequestMessage(method, url);
            AddHeaders(requestMessage);

            var stringContent = data != null
                ? new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json")
                : null;

            requestMessage.Content = stringContent;

            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // var errorContent = "";
            var responseBody = (TResponse) default;


            if (IsNot200RangeResponseCode(response.StatusCode))
            {
                //Plug this in when moving another Post endpoint which throws domain errors
                if (response.StatusCode == HttpStatusCode.BadRequest &&
                    response.GetSubStatusCode() == HttpSubStatusCode.DomainException)
                {
                    throw CreateApiModelException(response, json);
                }
                else
                {
                    throw new RestHttpClientException(response, json);
                }
            }
            else
            {
                responseBody = JsonConvert.DeserializeObject<TResponse>(json);
            }

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
                return new CommitmentsV2.Api.Types.Validation.CommitmentsApiModelException(new List<CommitmentsV2.Api.Types.Validation.ErrorDetail>());
            }

            var errors = new CommitmentsV2.Api.Types.Validation.CommitmentsApiModelException(JsonConvert.DeserializeObject<CommitmentsV2.Api.Types.Validation.ErrorResponse>(content).Errors);

            var errorDetails = string.Join(";", errors.Errors.Select(e => $"{e.Field} ({e.Message})"));
            _logger.Log(errors.Errors.Count == 0 ? LogLevel.Warning : LogLevel.Debug, $"{httpResponseMessage.RequestMessage.RequestUri} has returned {errors.Errors.Count} errors: {errorDetails}");

            return errors;
        }

    }
    
    public interface IOuterApiClient
    {
        Task<TResponse> Get<TResponse>(string url);
        Task<TResponse> Post<TResponse>(string url, object data);
        Task<TResponse> Put<TResponse>(string url, object data);
    }
}
