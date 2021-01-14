using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ConfirmDetailsAndSendViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<ITrainingProgrammeApiClient> _mockTrainingProgrammeApiClient;

        private ChangeOfProviderRequest _request;

        private GetApprenticeshipResponse _apprenticeshipResponse;
        private GetPriceEpisodesResponse _priceEpisodeResponse;
        private StandardSummary _standardSummary;

        private ConfirmDetailsAndSendViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Build<ChangeOfProviderRequest>()
                .With(x => x.NewStartMonth,1)
                .With(x => x.NewStartYear, 2020)
                .With(x => x.NewEndMonth,1)
                .With(x => x.NewEndYear,2022)
                .With(x => x.NewPrice, 1500)
                .Create();

            _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                .With(x => x.CourseCode, "ABC").Create();

            _priceEpisodeResponse = autoFixture.Build<GetPriceEpisodesResponse>()
                .With(x => x.PriceEpisodes, new List<PriceEpisode> { new PriceEpisode { Cost = 2000, ToDate = null } })
                .Create();

            _standardSummary = autoFixture.Build<StandardSummary>()
                .With(x => x.CurrentFundingCap, 1000).Create();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockTrainingProgrammeApiClient = new Mock<ITrainingProgrammeApiClient>();

            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_apprenticeshipResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_priceEpisodeResponse);
            _mockTrainingProgrammeApiClient.Setup(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode))
                .ReturnsAsync(_standardSummary);

            _mapper = new ConfirmDetailsAndSendViewModelMapper(_mockCommitmentsApiClient.Object, _mockTrainingProgrammeApiClient.Object);
        }

        [Test]
        public async Task GetFundingCapIsCalled()
        {
            var result = await _mapper.Map(_request);

            _mockTrainingProgrammeApiClient.Verify(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode), Times.Once());
        }

        [Test]
        public async Task GetPriceEpisodesCapIsCalled()
        {
            var result = await _mapper.Map(_request);

            _mockCommitmentsApiClient.Verify(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetApprenticeshipCapIsCalled()
        {
            var result = await _mapper.Map(_request);

            _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }

    }
}
