﻿using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ChangeOptionViewModelToEditApprenticeshipRequestViewModelMapperTests
{
    private Fixture _fixture;

    private ChangeOptionViewModel _viewModel;
    private EditApprenticeshipRequestViewModel _editViewModel;

    private GetApprenticeshipResponse _getApprenticeshipResponse;
    private GetPriceEpisodesResponse _getPriceEpisodesResponse;
    private GetTrainingProgrammeResponse _getVersionResponse;

    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
    private Mock<ICacheStorageService> _mockCacheStorageService;

    private ChangeOptionViewModelToEditApprenticeshipRequestViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        _fixture = new Fixture();

        _viewModel = _fixture.Create<ChangeOptionViewModel>();

        var baseDate = DateTime.Now;

        var startDate = baseDate;
        var endDate = baseDate.AddYears(2);
        var dateOfBirth = baseDate.AddYears(-18);

        _getApprenticeshipResponse = _fixture.Build<GetApprenticeshipResponse>()
            .With(x => x.StartDate, startDate)
            .With(x => x.EndDate, endDate)
            .With(x => x.DateOfBirth, dateOfBirth)
            .Without(x => x.EmploymentEndDate)
            .Create();

        _editViewModel = _fixture.Build<EditApprenticeshipRequestViewModel>()
            .With(x => x.CourseCode, _getApprenticeshipResponse.CourseCode)
            .With(x => x.Version, _getApprenticeshipResponse.Version)
            .With(x => x.StartDate, new MonthYearModel(startDate.ToString("MMyyyy")))
            .With(x => x.EndDate, new MonthYearModel(endDate.ToString("MMyyyy")))
            .With(x => x.DateOfBirth, new MonthYearModel(dateOfBirth.ToString("MMyyyy")))
            .With(x => x.EmploymentEndDate, new MonthYearModel(""))
            .Create();

        var priceEpisode = _fixture.Build<PriceEpisode>()
            .With(x => x.ApprenticeshipId, _getApprenticeshipResponse.Id)
            .With(x => x.FromDate, _getApprenticeshipResponse.StartDate.Value.AddDays(-1))
            .Without(x => x.ToDate)
            .Create();

        _getPriceEpisodesResponse = _fixture.Build<GetPriceEpisodesResponse>()
            .With(x => x.PriceEpisodes, new List<PriceEpisode> { priceEpisode })
            .Create();

        var trainingProgramme = _fixture.Build<TrainingProgramme>().With(x => x.Name, _getApprenticeshipResponse.CourseName).Create();

        _getVersionResponse = new GetTrainingProgrammeResponse { TrainingProgramme = trainingProgramme };

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(_viewModel.ApprenticeshipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getApprenticeshipResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(_viewModel.ApprenticeshipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getPriceEpisodesResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetTrainingProgrammeVersionByCourseCodeAndVersion(_getApprenticeshipResponse.CourseCode, _getApprenticeshipResponse.Version, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getVersionResponse);

        _mockCacheStorageService = new Mock<ICacheStorageService>();

        _mapper = new ChangeOptionViewModelToEditApprenticeshipRequestViewModelMapper(
            _mockCommitmentsApiClient.Object,
            _mockCacheStorageService.Object);
    }

    [Test]
    public async Task When_EditViewModelStoredInTempData_Then_GetTempData()
    {
        SetUpCacheData();

        var result = await _mapper.Map(_viewModel);

        result.ULN.Should().Be(_editViewModel.ULN);
        result.FirstName.Should().Be(_editViewModel.FirstName);
        result.LastName.Should().Be(_editViewModel.LastName);
        result.Email.Should().Be(_editViewModel.Email);
        result.CourseCode.Should().Be(_editViewModel.CourseCode);
        result.Version.Should().Be(_editViewModel.Version);
        result.TrainingName.Should().Be(_editViewModel.TrainingName);
        result.EmployerReference.Should().Be(_editViewModel.EmployerReference);
    }

    [Test]
    public async Task When_EditViewModelStoredInTempData_Then_DoNotCallApis()
    {
        SetUpCacheData();

        await _mapper.Map(_viewModel);

        _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(_viewModel.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Never());
        _mockCommitmentsApiClient.Verify(c => c.GetPriceEpisodes(_viewModel.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Never());
        _mockCommitmentsApiClient.Verify(c => c.GetTrainingProgrammeVersionByCourseCodeAndVersion(
            _getApprenticeshipResponse.CourseCode, _getApprenticeshipResponse.StandardUId, It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    public async Task When_ChangingOptionDirectly_Then_GetApprencticeshipAndStandardVersionData()
    {
        var result = await _mapper.Map(_viewModel);

        result.ULN.Should().Be(_getApprenticeshipResponse.Uln);
        result.FirstName.Should().Be(_getApprenticeshipResponse.FirstName);
        result.LastName.Should().Be(_getApprenticeshipResponse.LastName);
        result.Email.Should().Be(_getApprenticeshipResponse.Email);
        result.DeliveryModel.Should().Be(_getApprenticeshipResponse.DeliveryModel);
        result.CourseCode.Should().Be(_getApprenticeshipResponse.CourseCode);
        result.Version.Should().Be(_getApprenticeshipResponse.Version);
        result.TrainingName.Should().Be(_getApprenticeshipResponse.CourseName);
        result.EmployerReference.Should().Be(_getApprenticeshipResponse.EmployerReference);
    }

    [Test]
    public async Task Then_OptionIsMapped()
    {
        var result = await _mapper.Map(_viewModel);

        result.Option.Should().Be(_viewModel.SelectedOption);
    }

    [Test]
    public async Task And_ChooseLaterIsSelected_Then_OptionIsEmptyString()
    {
        _viewModel.SelectedOption = "TBC";

        var result = await _mapper.Map(_viewModel);

        result.Option.Should().Be(string.Empty);
    }

    private void SetUpCacheData()
    {
        _mockCacheStorageService
            .Setup(d => d.RetrieveFromCache<EditApprenticeshipRequestViewModel>(nameof(EditApprenticeshipRequestViewModel)))
            .ReturnsAsync(_editViewModel);
    }
}