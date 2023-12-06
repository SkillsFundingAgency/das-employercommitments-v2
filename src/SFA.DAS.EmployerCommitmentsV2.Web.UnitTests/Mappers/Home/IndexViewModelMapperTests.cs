using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Home;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Home
{
    [TestFixture]
    public class IndexViewModelMapperTests
    {
        private IndexViewModelMapperTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new IndexViewModelMapperTestsFixture();
        }

        [Test]
        public async Task AccountHashedIdIsMappedCorrectly()
        {
            _fixture.SetupProviders(2);
            await _fixture.Map();
            _fixture.VerifyAccountHashedIdIsMappedCorrectly();
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        public async Task SetPaymentOrderLinkIsVisibleWithMultipleApprovedProviders(int providers, bool expectShowLink)
        {
            _fixture.SetupProviders(providers);
            await _fixture.Map();
            _fixture.VerifySetPaymentOrderLinkVisibility(expectShowLink);
        }

        [TestCase("prd")]
        [TestCase("pp")]
        [TestCase("TEST")]
        public async Task ThenThePublicSectorReportingUrlIsCorrectlySet(string environment)
        {
            _fixture.SetupProviders(2);
            _fixture.SetResourceEnvironment(environment);
            await _fixture.Map();
            _fixture.VerifyPublicSectorReportingLink(environment);
        }

        private class IndexViewModelMapperTestsFixture
        {
            private IndexViewModelMapper _mapper;
            private Mock<ICommitmentsApiClient> _apiClient;
            private Mock<ILogger<IndexViewModelMapper>> _logger;
            private GetProviderPaymentsPriorityResponse _apiProvidersResponse;
            private IndexRequest _request;
            private IndexViewModel _result;
            private readonly Mock<IConfiguration> _configuration;

            public IndexViewModelMapperTestsFixture()
            {
                var autoFixture = new Fixture();

                _apiClient = new Mock<ICommitmentsApiClient>();
                _logger = new Mock<ILogger<IndexViewModelMapper>>();
                _configuration = new Mock<IConfiguration>();
                _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");
                
                _mapper = new IndexViewModelMapper(_apiClient.Object, _logger.Object, _configuration.Object);
                _request = autoFixture.Create<IndexRequest>();
            }

            public IndexViewModelMapperTestsFixture SetResourceEnvironment(string environment)
            {
                _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns(environment);
                return this;
            }
            
            public IndexViewModelMapperTestsFixture SetupProviders(int numberOfProviders)
            {
                var providerPaymentPriorities = new List<GetProviderPaymentsPriorityResponse.ProviderPaymentPriorityItem>();

                for (var i = 0; i < numberOfProviders; i++)
                {
                    providerPaymentPriorities.Add(new GetProviderPaymentsPriorityResponse.ProviderPaymentPriorityItem
                    {
                        ProviderId = i + 10000,
                        ProviderName = $"Test{i}",
                        PriorityOrder = i
                    });
                }

                _apiProvidersResponse = new GetProviderPaymentsPriorityResponse
                {
                    ProviderPaymentPriorities = providerPaymentPriorities
                };

                _apiClient.Setup(x => x.GetProviderPaymentsPriority(It.IsAny<long>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_apiProvidersResponse);
                
                return this;
            }

            public void VerifySetPaymentOrderLinkVisibility(bool expectVisible)
            {
                Assert.That(_result.ShowSetPaymentOrderLink, Is.EqualTo(expectVisible));
            }

            public async Task Map()
            {
                _result = await _mapper.Map(TestHelper.Clone(_request));
            }

            public void VerifyAccountHashedIdIsMappedCorrectly()
            {
                Assert.That(_result.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
            }

            public void VerifyPublicSectorReportingLink(string environment)
            {
                if(environment.ToLower() == "prd")
                {
                    Assert.That(_result.PublicSectorReportingLink, Is.EqualTo($"https://reporting.manage-apprenticeships.service.gov.uk/accounts/{_result.AccountHashedId}/home"));   
                }
                else
                {
                    Assert.That(_result.PublicSectorReportingLink, Is.EqualTo($"https://reporting.{environment.ToLower()}-eas.apprenticeships.education.gov.uk/accounts/{_result.AccountHashedId}/home"));
                }
            }
        }
    }
}
