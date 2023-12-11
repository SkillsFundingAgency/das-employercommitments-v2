using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks;
using SFA.DAS.Http;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.HealthChecks;

[TestFixture]
[Parallelizable]
public class CommitmentsApiHealthCheckTests : FluentTest<CommitmentsApiHealthCheckTestsFixture>
{
    [TestCase(new [] { Role.Employer }, HealthStatus.Healthy)]
    [TestCase(new [] { Role.Provider }, HealthStatus.Unhealthy)]
    [TestCase(new [] { Role.Employer, Role.Provider }, HealthStatus.Unhealthy)]
    public async Task CheckHealthAsync_WhenWhoAmISucceedsAndUserIsInRoles_ThenShouldReturnHealthStatus(string[] roles, HealthStatus healthStatus)
    {
        await TestAsync(
            f => f.SetWhoAmISuccess(roles),
            f => f.CheckHealthAsync(),
            (f, r) =>
            {
                r.Should().NotBeNull();
                r.Status.Should().Be(healthStatus);
                r.Data["Roles"].Should().BeOfType<List<string>>().Which.Should().BeEquivalentTo(roles);
            });
    }
        
    [Test]
    public async Task CheckHealthAsync_WhenWhoAmIFails_ThenShouldThrowException()
    {
        await TestExceptionAsync(
            f => f.SetWhoAmIFailure(),
            f => f.CheckHealthAsync(),
            (f, r) => r.Should().ThrowAsync<Exception>().Result.Which.Should().Be(f.Exception));
    }
}

public class CommitmentsApiHealthCheckTestsFixture
{
    public HealthCheckContext HealthCheckContext { get; set; }
    public Mock<ICommitmentsApiClient> ApiClient { get; set; }
    public CommitmentsApiHealthCheck HealthCheck { get; set; }
    public HttpResponseMessage HttpResponseMessage { get; set; }
    public RestHttpClientException Exception { get; set; }

    public CommitmentsApiHealthCheckTestsFixture()
    {
        HealthCheckContext = new HealthCheckContext
        {
            Registration = new HealthCheckRegistration("Foo", Mock.Of<IHealthCheck>(), null, null)
        };
            
        ApiClient = new Mock<ICommitmentsApiClient>();
        HealthCheck = new CommitmentsApiHealthCheck(ApiClient.Object);

        HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            RequestMessage = new HttpRequestMessage(),
            ReasonPhrase = "Url not found"
        };
            
        Exception = new RestHttpClientException(HttpResponseMessage, "Url not found");
    }

    public Task<HealthCheckResult> CheckHealthAsync()
    {
        return HealthCheck.CheckHealthAsync(HealthCheckContext);
    }

    public CommitmentsApiHealthCheckTestsFixture SetWhoAmISuccess(IEnumerable<string> roles)
    {
        ApiClient.Setup(c => c.WhoAmI()).ReturnsAsync(new WhoAmIResponse { Roles = roles.ToList() });
            
        return this;
    }

    public CommitmentsApiHealthCheckTestsFixture SetWhoAmIFailure()
    {
        ApiClient.Setup(c => c.WhoAmI()).ThrowsAsync(Exception);
            
        return this;
    }
}