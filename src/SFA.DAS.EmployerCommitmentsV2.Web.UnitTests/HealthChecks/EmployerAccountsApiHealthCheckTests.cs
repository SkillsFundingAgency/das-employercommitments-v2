using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.HealthChecks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.HealthChecks;

[TestFixture]
[Parallelizable]
public class EmployerAccountsApiHealthCheckTests
{
    private EmployerAccountsApiHealthCheckTestsFixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new EmployerAccountsApiHealthCheckTestsFixture();
    }
        
    [Test]
    public async Task CheckHealthAsync_WhenPingSucceeds_ThenShouldReturnHealthyStatus()
    {
        var healthCheckResult = await _fixture.SetPingSuccess().CheckHealthAsync();

        Assert.That(healthCheckResult.Status, Is.EqualTo(HealthStatus.Healthy));
    }
        
    [Test]
    public async Task CheckHealthAsync_WhenPingFails_ThenShouldReturnDegradedStatus()
    {
        var healthCheckResult = await _fixture.SetPingFailure().CheckHealthAsync();

        Assert.Multiple(() =>
        {
            Assert.That(healthCheckResult.Status, Is.EqualTo(HealthStatus.Degraded));
            Assert.That(healthCheckResult.Description, Is.EqualTo(_fixture.Exception.Message));
        });
    }

    private class EmployerAccountsApiHealthCheckTestsFixture
    {
        private HealthCheckContext HealthCheckContext { get; set; }
        public CancellationToken CancellationToken { get; set; }
        private Mock<IEmployerAccountsApiClient> ProviderRelationshipsApiClient { get; set; }
        private EmployerAccountsApiHealthCheck HealthCheck { get; set; }
        public Exception Exception { get; set; }

        public EmployerAccountsApiHealthCheckTestsFixture()
        {
            HealthCheckContext = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("Foo", Mock.Of<IHealthCheck>(), null, null)
            };
                
            ProviderRelationshipsApiClient = new Mock<IEmployerAccountsApiClient>();
            HealthCheck = new EmployerAccountsApiHealthCheck(ProviderRelationshipsApiClient.Object);
            Exception = new Exception("Foobar");
        }
            
        public Task<HealthCheckResult> CheckHealthAsync()
        {
            return HealthCheck.CheckHealthAsync(HealthCheckContext, CancellationToken);
        }

        public EmployerAccountsApiHealthCheckTestsFixture SetPingSuccess()
        {
            ProviderRelationshipsApiClient.Setup(c => c.Ping(CancellationToken)).Returns(Task.CompletedTask);
                
            return this;
        }

        public EmployerAccountsApiHealthCheckTestsFixture SetPingFailure()
        {
            ProviderRelationshipsApiClient.Setup(c => c.Ping(CancellationToken)).ThrowsAsync(Exception);
                
            return this;
        }
    }
}