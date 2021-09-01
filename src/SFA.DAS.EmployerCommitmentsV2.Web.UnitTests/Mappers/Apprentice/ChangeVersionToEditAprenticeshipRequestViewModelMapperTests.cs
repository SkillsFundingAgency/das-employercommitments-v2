using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ChangeVersionToEditAprenticeshipRequestViewModelMapperTests
    {
        private ChangeVersionViewModel _viewModel;

        private GetApprenticeshipResponse _getApprenticeshipResponse;
        private GetPriceEpisodesResponse _getPriceEpisodesResponse;

        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private ChangeVersionViewModelToEditApprenticehipRequestViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            _viewModel = fixture.Create<ChangeVersionViewModel>();
            _getApprenticeshipResponse = fixture.Create<GetApprenticeshipResponse>();

            var priceEpisode = fixture.Build<PriceEpisode>()
                .With(x => x.ApprenticeshipId, _getApprenticeshipResponse.Id)
                .With(x => x.FromDate, _getApprenticeshipResponse.StartDate.AddDays(-1))
                .Without(x => x.ToDate)
                .Create();

            _getPriceEpisodesResponse = fixture.Build<GetPriceEpisodesResponse>()
                .With(x => x.PriceEpisodes, new List<PriceEpisode> { priceEpisode })
                .Create();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(_viewModel.ApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getApprenticeshipResponse);
            
            _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(_viewModel.ApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getPriceEpisodesResponse);

            _mapper = new ChangeVersionViewModelToEditApprenticehipRequestViewModelMapper(_mockCommitmentsApiClient.Object);
        }

        [Test]
        public async Task VerifyGetApprenticeship()
        {
            var result = await _mapper.Map(_viewModel);

            _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(_viewModel.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task VerifyGetPriceEpisodes()
        {
            var result = await _mapper.Map(_viewModel);

            _mockCommitmentsApiClient.Verify(c => c.GetPriceEpisodes(_viewModel.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task VerifyViewModelIsMapped()
        {
            var result = await _mapper.Map(_viewModel);

            result.Version.Should().Be(_viewModel.SelectedVersion);
            result.AccountHashedId.Should().Be(_viewModel.AccountHashedId);
            result.HashedApprenticeshipId.Should().Be(_viewModel.ApprenticeshipHashedId);

            result.ULN.Should().Be(_getApprenticeshipResponse.Uln);
            result.FirstName.Should().Be(_getApprenticeshipResponse.FirstName);
            result.LastName.Should().Be(_getApprenticeshipResponse.LastName);
            result.Email.Should().Be(_getApprenticeshipResponse.Email);
            result.CourseCode.Should().Be(_getApprenticeshipResponse.CourseCode);
            result.EmployerReference.Should().Be(_getApprenticeshipResponse.EmployerReference);

            //result.Cost.Should().Be(_getPriceEpisodesResponse.PriceEpisodes.GetPrice());
        }
    }
}
