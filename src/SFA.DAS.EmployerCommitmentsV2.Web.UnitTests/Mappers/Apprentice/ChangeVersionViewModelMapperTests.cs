﻿using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ChangeVersionViewModelMapperTests
    {
        private Fixture _fixture;

        private ChangeVersionRequest _request;
        private GetApprenticeshipResponse _getApprenticeshipResponse;
        private GetTrainingProgrammeVersionsResponse _getTrainingProgrammeVersionsResponse;

        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private ChangeVersionViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();

            _request = _fixture.Create<ChangeVersionRequest>();

            _getApprenticeshipResponse = _fixture.Build<GetApprenticeshipResponse>()
                    .With(x => x.Version, "1.1")
                    .With(x => x.StandardUId, "ST0001_1.1")
                .Create();

            _getTrainingProgrammeVersionsResponse = GetTrainingProgrammeVersions();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(_request.ApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getApprenticeshipResponse);

            _mockCommitmentsApiClient.Setup(c => c.GetTrainingProgrammeVersions(_getApprenticeshipResponse.CourseCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getTrainingProgrammeVersionsResponse);

            _mapper = new ChangeVersionViewModelMapper(_mockCommitmentsApiClient.Object);
        }

        [Test]
        public async Task Then_CurrentVersionIsMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.CurrentVersion.Should().Be(_getApprenticeshipResponse.Version);
        }

        [Test]
        public async Task Then_CurrentVersionInfoIsMapped()
        {
            var currentVersion = _getTrainingProgrammeVersionsResponse.TrainingProgrammeVersions.FirstOrDefault(x => x.Version == _getApprenticeshipResponse.Version);

            var viewModel = await _mapper.Map(_request);

            viewModel.StandardTitle.Should().Be(currentVersion.Name);
            viewModel.StandardUrl.Should().Be(currentVersion.StandardPageUrl);
        }

        [Test]
        public async Task Then_NewerVersionsAreMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.NewerVersions.Count().Should().Be(1);
            viewModel.NewerVersions.Should().Contain("1.2");
        }

        private GetTrainingProgrammeVersionsResponse GetTrainingProgrammeVersions()
        {
            var versions = _fixture.CreateMany<TrainingProgramme>(3).ToList();

            foreach(var version in versions)
            {
                version.CourseCode = "1";
            }

            versions[0].Version = "1.0";
            versions[1].Version = "1.1";
            versions[1].StandardUId = "ST0001_1.1";
            versions[2].Version = "1.2";

            return new GetTrainingProgrammeVersionsResponse
            {
                TrainingProgrammeVersions = versions
            };
        }
    }
}