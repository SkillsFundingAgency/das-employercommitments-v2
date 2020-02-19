using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
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

        private class IndexViewModelMapperTestsFixture
        {
            private IndexViewModelMapper _mapper;
            private Mock<ICommitmentsApiClient> _apiClient;
            private GetApprovedProvidersResponse _apiProvidersResponse;
            private IndexRequest _request;
            private IndexViewModel _result;

            public IndexViewModelMapperTestsFixture()
            {
                var autoFixture = new Fixture();

                _apiProvidersResponse = new GetApprovedProvidersResponse(new List<long> { 123, 456 });

                _apiClient = new Mock<ICommitmentsApiClient>();
                _apiClient.Setup(x => x.GetApprovedProviders(It.IsAny<long>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_apiProvidersResponse);

                _mapper = new IndexViewModelMapper(_apiClient.Object);
                _request = autoFixture.Create<IndexRequest>();
                
            }

            public IndexViewModelMapperTestsFixture SetupProviders(int numberOfProviders)
            {
                var providers = new List<long>();

                for (var i = 0; i < numberOfProviders;i++)
                {
                    providers.Add(i);
                }

                _apiProvidersResponse = new GetApprovedProvidersResponse(providers);

                _apiClient.Setup(x => x.GetApprovedProviders(It.IsAny<long>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_apiProvidersResponse);
                return this;
            }

            public void VerifySetPaymentOrderLinkVisibility(bool expectVisible)
            {
                Assert.AreEqual(expectVisible, _result.ShowSetPaymentOrderLink);
            }

            public async Task Map()
            {
                _result = await _mapper.Map(TestHelper.Clone(_request));
            }

            public void VerifyAccountHashedIdIsMappedCorrectly()
            {
                Assert.AreEqual(_request.AccountHashedId, _result.AccountHashedId);
            }
        }
    }
}
