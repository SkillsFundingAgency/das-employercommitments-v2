using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks;
using SFA.DAS.Http;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerCommitmentsV2.UnitTests.Web.HealthChecks
{
    [TestFixture]
    [Parallelizable]
    public class CommitmentsApiHealthCheckTests : FluentTest<CommitmentsApiHealthCheckTestsFixture>
    {
        [Test]
        public Task CheckHealthAsync_WhenPingSucceeds_ThenShouldReturnHealthyStatus()
        {
            return TestAsync(
                f => f.SetPingSuccess(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Status.Should().Be(HealthStatus.Healthy));
        }
        
        [Test]
        public Task CheckHealthAsync_WhenPingFails_ThenShouldReturnUnhealthyStatus()
        {
            return TestAsync(
                f => f.SetPingFailure(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Status.Should().Be(HealthStatus.Unhealthy));
        }
        
        [Test]
        public Task CheckHealthAsync_WhenPingFails_ThenShouldReturnException()
        {
            return TestAsync(
                f => f.SetPingFailure(),
                f => f.CheckHealthAsync(),
                (f, r) => r.Exception.Should().Be(f.Exception));
        }
    }

    public class CommitmentsApiHealthCheckTestsFixture
    {
        public HealthCheckContext HealthCheckContext { get; set; }
        public Mock<ICommitmentsApiClient> ApiClient { get; set; }
        public Mock<ILogger<CommitmentsApiHealthCheck>> Logger { get; set; }
        public CommitmentsApiHealthCheck CommitmentsApiHealthCheck { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public RestHttpClientException Exception { get; set; }

        public CommitmentsApiHealthCheckTestsFixture()
        {
            HealthCheckContext = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("Foo", Mock.Of<IHealthCheck>(), null, null)
            };
            
            ApiClient = new Mock<ICommitmentsApiClient>();
            Logger = new Mock<ILogger<CommitmentsApiHealthCheck>>();
            CommitmentsApiHealthCheck = new CommitmentsApiHealthCheck(ApiClient.Object, Logger.Object);

            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                RequestMessage = new HttpRequestMessage(),
                ReasonPhrase = "Url not found"
            };
            
            Exception = new RestHttpClientException(HttpResponseMessage, "Url not found");
        }

        public Task<HealthCheckResult> CheckHealthAsync()
        {
            return CommitmentsApiHealthCheck.CheckHealthAsync(HealthCheckContext);
        }

        public CommitmentsApiHealthCheckTestsFixture SetPingSuccess()
        {
            ApiClient.Setup(c => c.HealthCheck()).ReturnsAsync(true);
            
            return this;
        }

        public CommitmentsApiHealthCheckTestsFixture SetPingFailure()
        {
            ApiClient.Setup(c => c.HealthCheck()).ThrowsAsync(Exception);
            
            return this;
        }
    }
}