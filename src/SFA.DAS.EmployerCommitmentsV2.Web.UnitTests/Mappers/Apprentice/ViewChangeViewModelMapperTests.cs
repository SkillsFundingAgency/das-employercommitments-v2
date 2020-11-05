using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfPartyRequestsResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ViewChangeViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<IEncodingService> _mockEncodingService;

        private GetChangeOfPartyRequestsResponse _changeOfPartyResponse;

        private const long _providerId = 10000;
        private const string _cohortReference = "ABC123";

        private ViewChangesViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();

            _changeOfPartyResponse = autoFixture.Build<GetChangeOfPartyRequestsResponse>()
                                        .With(r => r.ChangeOfPartyRequests, GetChangeOfPartyRequests())
                                        .Create();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient.Setup(c => c.GetChangeOfPartyRequests(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_changeOfPartyResponse);

            _mockEncodingService = new Mock<IEncodingService>();
            _mockEncodingService.Setup(c => c.Encode(It.IsAny<long>(), EncodingType.CohortReference))
                .Returns(_cohortReference);

            _mapper = new ViewChangesViewModelMapper(_mockCommitmentsApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task ChangeOfPartyRequestIsCalled(ViewChangesRequest request)
        {
            var result = await _mapper.Map(request);

            _mockCommitmentsApiClient.Verify(c => c.GetChangeOfPartyRequests(request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetPriceHistoryIsCalled()
        {

        }

        [Test, MoqAutoData]
        public async Task WhenMoreThanOnePriceIsFoundInHistory_ThenTheMostRecentIsUsed()
        {

        }

        [Test, MoqAutoData]
        public async Task CohortIdIsEncoded()
        {

        }
        
        private List<ChangeOfPartyRequest> GetChangeOfPartyRequests()
        {
            return new List<ChangeOfPartyRequest>
            {
                new ChangeOfPartyRequest
                {
                    ProviderId = _providerId
                }
            };
        }
    }
}
