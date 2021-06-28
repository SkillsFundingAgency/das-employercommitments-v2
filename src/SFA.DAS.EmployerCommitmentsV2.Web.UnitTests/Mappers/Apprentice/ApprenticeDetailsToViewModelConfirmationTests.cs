using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetApprenticeshipUpdatesResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ApprenticeDetailsToViewModelConfirmationTests
    {
        private Fixture _fixture;
        private Mock<ICommitmentsApiClient> _api;
        private Mock<IEncodingService> _mockEncodingService;
        private ApprenticeshipDetailsRequestToViewModelMapper _mapper;
        private ApprenticeshipDetailsRequest _request;

        public ApprenticeDetailsToViewModelConfirmationTests()
        {
            _fixture = new Fixture();
            _api = new Mock<ICommitmentsApiClient>();
            _mockEncodingService = new Mock<IEncodingService>();
            _request = _fixture.Create<ApprenticeshipDetailsRequest>();
            Setup();
        }

        [Test]
        public async Task WhenTheResponseIsConfirmed_ThenInsureTheModelIsConfirmed()
        {
            // Arrange
            SetupResponseStatus(ConfirmationStatus.Confirmed);

             _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_api.Object, _mockEncodingService.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>(), Mock.Of<IAuthorizationService>());

            // Act
            var viewModel = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(viewModel.ConfirmationStatus, ConfirmationStatus.Confirmed);
        }

        [Test]
        public async Task WhenTheResponseIsUnconfirmed_ThenInsureTheModelIsUnconfirmed()
        {
            // Arrange
            SetupResponseStatus(ConfirmationStatus.Unconfirmed);

             _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_api.Object, _mockEncodingService.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>(), Mock.Of<IAuthorizationService>());

            // Act
            var viewModel = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(viewModel.ConfirmationStatus, ConfirmationStatus.Unconfirmed);
        }

        [Test]
        public async Task WhenTheResponseIsOverdue_ThenInsureTheModelIsOverdue()
        {
            // Arrange
            SetupResponseStatus(ConfirmationStatus.Overdue);

             _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_api.Object, _mockEncodingService.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>(), Mock.Of<IAuthorizationService>());

            // Act
            var viewModel = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(viewModel.ConfirmationStatus, ConfirmationStatus.Overdue);
        }

        private void SetupResponseStatus(ConfirmationStatus confirmationStatus)
        {
            var apprenticeshipResponseConfirmed = _fixture.Build<GetApprenticeshipResponse>()
               .With(x => x.ConfirmationStatus, confirmationStatus).Create();
            _api.Setup(x => x.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(apprenticeshipResponseConfirmed);
        }

        private void Setup()
        {
            var trainingProgrammeResponse = _fixture.Create<GetTrainingProgrammeResponse>();
            _api.Setup(t => t.GetTrainingProgramme(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(trainingProgrammeResponse);

            var apprenticeshipUpdatesResponse = _fixture.Build<GetApprenticeshipUpdatesResponse>()
                .With(x => x.ApprenticeshipUpdates, new List<ApprenticeshipUpdate> {
                    new ApprenticeshipUpdate { OriginatingParty = Party.Employer } }).Create();
            _api.Setup(c => c.GetApprenticeshipUpdates(It.IsAny<long>(), It.IsAny<GetApprenticeshipUpdatesRequest>(), CancellationToken.None))
                .ReturnsAsync(apprenticeshipUpdatesResponse);

            var dataLocksResponse = _fixture.Create<GetDataLocksResponse>();
            _api.Setup(c => c.GetApprenticeshipDatalocksStatus(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(dataLocksResponse);

            var changeOfPartyRequestsResponse = _fixture.Create<GetChangeOfPartyRequestsResponse>();
            _api.Setup(c => c.GetChangeOfPartyRequests(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(changeOfPartyRequestsResponse);

            var priceEpisodesResponse = _fixture.Build<GetPriceEpisodesResponse>()
                 .With(x => x.PriceEpisodes, new List<PriceEpisode> {
                    new PriceEpisode { Cost = 1000, ToDate = DateTime.Now.AddMonths(-1)}})
                .Create();
            _api.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(priceEpisodesResponse);
        }
    }
}
