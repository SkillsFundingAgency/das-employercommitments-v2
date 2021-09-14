using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ChangeOptionViewModelMapperTests
    {
        private Fixture _fixture;
        private EditApprenticeshipRequestViewModel _editViewModel;
        private GetTrainingProgrammeResponse _getVersionResponse;
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private ChangeOptionViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
            
            var baseDate = DateTime.Now;
            var startDate = new MonthYearModel(baseDate.ToString("MMyyyy"));
            var endDate = new MonthYearModel(baseDate.AddYears(2).ToString("MMyyyy"));
            var dateOfBirth = new MonthYearModel(baseDate.AddYears(-18).ToString("MMyyyy"));

            _editViewModel = _fixture.Build<EditApprenticeshipRequestViewModel>()
                    .With(x => x.StartDate, startDate)
                    .With(x => x.EndDate, endDate)
                    .With(x => x.DateOfBirth, dateOfBirth)
                .Create();

            _getVersionResponse = _fixture.Create<GetTrainingProgrammeResponse>();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient
                .Setup(c => c.GetTrainingProgrammeVersionByCourseCodeAndVersion(_editViewModel.CourseCode, _editViewModel.Version, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getVersionResponse);

            _mapper = new ChangeOptionViewModelMapper(_mockCommitmentsApiClient.Object);
        }

        [Test]
        public async Task Then_SelectedVersionIsMapped()
        {
            var viewModel = await _mapper.Map(_editViewModel);

            viewModel.SelectedVersion.Should().Be(_editViewModel.Version);
        }

        [Test]
        public async Task Then_SelectedVersionNameIsMapped()
        {
            var viewModel = await _mapper.Map(_editViewModel);

            viewModel.SelectedVersionName.Should().Be(_getVersionResponse.TrainingProgramme.Name);
        }

        [Test]
        public async Task Then_SelectedVersionUrlIsMapped()
        {
            var viewModel = await _mapper.Map(_editViewModel);

            viewModel.SelectedVersionUrl.Should().Be(_getVersionResponse.TrainingProgramme.StandardPageUrl);
        }

        [Test]
        public async Task Then_OptionsAreMapped()
        {
            var viewModel = await _mapper.Map(_editViewModel);

            viewModel.Options.Should().BeEquivalentTo(_getVersionResponse.TrainingProgramme.Options);
        }
    }
}
