using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
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

        private ChangeOptionRequest _request;

        private GetApprenticeshipResponse _getApprenticeshipResponse;
        private GetTrainingProgrammeResponse _getVersionResponse;
        private EditApprenticeshipRequestViewModel _editViewModel;

        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<ITempDataDictionaryFactory> _mockTempDataFactory;
        private Mock<ITempDataDictionary> _mockTempDataDictionary;

        private ChangeOptionViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();

            _request = _fixture.Create<ChangeOptionRequest>();

            var baseDate = DateTime.Now;

            var startDate = baseDate;
            var endDate = baseDate.AddYears(2);
            var dateOfBirth = baseDate.AddYears(-18);

            _getApprenticeshipResponse = _fixture.Build<GetApprenticeshipResponse>()
                    .With(x => x.StartDate, startDate)
                    .With(x => x.EndDate, endDate)
                    .With(x => x.DateOfBirth, dateOfBirth)
                .Create();

            _editViewModel = _fixture.Build<EditApprenticeshipRequestViewModel>()
                    .With(x => x.CourseCode, _getApprenticeshipResponse.CourseCode)
                    .With(x => x.Version, _getApprenticeshipResponse.Version)
                    .With(x => x.StartDate, new MonthYearModel(startDate.ToString("MMyyyy")))
                    .With(x => x.EndDate, new MonthYearModel(endDate.ToString("MMyyyy")))
                    .With(x => x.DateOfBirth, new MonthYearModel(dateOfBirth.ToString("MMyyyy")))
                .Create();

            _getVersionResponse = _fixture.Create<GetTrainingProgrammeResponse>();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockCommitmentsApiClient
                .Setup(c => c.GetTrainingProgrammeVersionByCourseCodeAndVersion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getVersionResponse);
            _mockCommitmentsApiClient
                .Setup(c => c.GetApprenticeship(_request.ApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getApprenticeshipResponse);

            _mockTempDataDictionary = new Mock<ITempDataDictionary>();

            _mockTempDataFactory = new Mock<ITempDataDictionaryFactory>();
            _mockTempDataFactory.Setup(f => f.GetTempData(It.IsAny<HttpContext>()))
                .Returns(_mockTempDataDictionary.Object);

            _mapper = new ChangeOptionViewModelMapper(_mockCommitmentsApiClient.Object, 
                Mock.Of<IHttpContextAccessor>(),
                _mockTempDataFactory.Object);
        }

        [Test]
        public async Task Then_AccountHashedIdIsMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.AccountHashedId.Should().Be(_request.AccountHashedId);
        }

        [Test]
        public async Task Then_ApprenticeshipHashedIdIsMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.ApprenticeshipHashedId.Should().Be(_request.ApprenticeshipHashedId);
        }

        [Test]
        public async Task Then_SelectedVersionIsMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.SelectedVersion.Should().Be(_editViewModel.Version);
        }

        [Test]
        public async Task Then_SelectedVersionNameIsMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.SelectedVersionName.Should().Be(_getVersionResponse.TrainingProgramme.Name);
        }

        [Test]
        public async Task Then_SelectedVersionUrlIsMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.SelectedVersionUrl.Should().Be(_getVersionResponse.TrainingProgramme.StandardPageUrl);
        }

        [Test]
        public async Task Then_CurrentOptionIsMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.CurrentOption.Should().Be(_getApprenticeshipResponse.Option);
        }

        [Test]
        public async Task And_ApprenticeshipOption_IsEmpty_Then_CurrentOptionIsMappedToNA()
        {
            _getApprenticeshipResponse.Option = string.Empty;

            var viewModel = await _mapper.Map(_request);

            viewModel.CurrentOption.Should().Be("N/A");
        }

        [Test]
        public async Task Then_OptionsAreMapped()
        {
            var viewModel = await _mapper.Map(_request);

            viewModel.Options.Should().BeEquivalentTo(_getVersionResponse.TrainingProgramme.Options);
        }

        [Test]
        public async Task When_CourseCodeHasChanged_Then_SetReturnToEditTrue()
        {
            _editViewModel.CourseCode = "1"; 
            SetupTempDataDictionary();

            var viewModel = await _mapper.Map(_request);

            viewModel.ReturnToEdit.Should().BeTrue();
        }

        [Test]
        public async Task When_StartDateHasChanged_Then_SetReturnToEditTrue()
        {
            _editViewModel.StartDate.Date.Value.AddMonths(1);
            SetupTempDataDictionary();

            var viewModel = await _mapper.Map(_request);

            viewModel.ReturnToEdit.Should().BeTrue();
        }

        [Test]
        public async Task When_VersionHasChanged_Then_SetReturnToChangeVersionTrue()
        {
            _editViewModel.Version = "1.1";
            SetupTempDataDictionary();

            var viewModel = await _mapper.Map(_request);

            viewModel.ReturnToEdit.Should().BeTrue();
        }

        [Test]
        public async Task When_UnconfirmedChangesAreInProgress_Then_MapNewValues()
        {
            _editViewModel.Version = "1.1";
            _editViewModel.Option = "New Option";
            SetupTempDataDictionary();

            var viewModel = await _mapper.Map(_request);

            viewModel.SelectedVersion.Should().Be(_editViewModel.Version);
            viewModel.SelectedOption.Should().Be(_editViewModel.Option);
        }

        private void SetupTempDataDictionary()
        {
            object viewModel = JsonConvert.SerializeObject(_editViewModel);

            _mockTempDataDictionary.Setup(d => d.TryGetValue("EditApprenticeshipRequestViewModel", out viewModel))
                .Returns(true);
        }
    }
}
