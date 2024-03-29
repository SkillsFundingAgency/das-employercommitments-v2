﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class DataLockRequestChangesRequestToViewModelMapperTests
{
    private DataLockRequestChangesRequestToViewModelMapperTestsFixture _fixture;
        
    [SetUp]
    public void Setup()
    {
        _fixture = new DataLockRequestChangesRequestToViewModelMapperTestsFixture()
            .WithCourseAndPriceDataLock();
    }

    [Test]
    public async Task GetApprenticeshipDatalockSummariesStatus_IsCalled()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyGetApprenticeshipDatalockSummariesStatusIsCalled();
    }

    [Test]
    public async Task GetGetPriceEpisodes_IsCalled()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyGetPriceEpisodesIsCalled();
    }

    [Test]
    public async Task GetApprenticeship_IsCalled()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyGetApprenticeshipIsCalled();
    }

    [Test]
    public async Task GetAllTrainingProgrammes_IsCalled()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyGetAllTrainingProgrammesIsCalled();
    }

    [Test]
    public async Task ApprenticeshipHashedId_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyApprenticeshipHashedIdIsMapped();
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyAccountHashedIdIsMapped();
    }

    [Test]
    public async Task AccountId_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyAccountIdIsMapped();
    }

    [Test]
    public async Task ApprenticeshipId_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyApprenticeshipIdIsMapped();
    }

    [Test]
    public async Task FirstName_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyFirstNameIsMapped();
    }

    [Test]
    public async Task LastName_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyLastNameIsMapped();
    }

    [Test]
    public async Task DateOfBirth_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyDateOfBirthIsMapped();
    }

    [Test]
    public async Task ULN_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyUlnIsMapped();
    }

    [Test]
    public async Task StartDate_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyStartDateIsMapped();
    }

    [Test]
    public async Task EndDate_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyEndDateIsMapped();
    }

    [Test]
    public async Task CourseCode_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyCourseCodeIsMapped();
    }

    [Test]
    public async Task CourseName_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyCourseNameIsMapped();
    }

    [Test]
    public async Task CourseAndPriceDataLock_CourseChanges_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyCourseChangesIsMapped();
    }

    [Test]
    public async Task CourseAndPriceDataLock_PriceChanges_IsMapped()
    {
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyPriceChangesIsMapped();
    }

    [Test]
    public async Task PriceDataLock_CourseChanges_IsEmpty()
    {
        //Arrange
        _fixture = new DataLockRequestChangesRequestToViewModelMapperTestsFixture()
            .WithPriceDataLock(DateTime.Now);

        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyCourseChangesIsEmpty();
    }

    [Test]
    public async Task CourseDataLock_PriceChanges_IsEmpty()
    {
        //Arrange
        _fixture = new DataLockRequestChangesRequestToViewModelMapperTestsFixture()
            .WithCourseDataLock(DataLockErrorCode.Dlock04);
            
        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyPriceChangesIsEmpty();
    }

    [Test]
    public async Task PriceDataLock_With_No_Matching_Price_Episode_Maps_First_Price_Episode()
    {
        //Arrange
        _fixture = new DataLockRequestChangesRequestToViewModelMapperTestsFixture()
            .WithPriceDataLock(DateTime.Now)
            .WithPriceEpisode(DateTime.Now.AddDays(1), null, 1000);

        //Act
        await _fixture.Map();

        //Assert
        _fixture.VerifyCourseChangesIsEmpty();
    }
}

public class DataLockRequestChangesRequestToViewModelMapperTestsFixture
{
    private readonly Fixture _autoFixture;
        
    private readonly DataLockRequestChangesRequest _request;
        
    private readonly Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
    private GetDataLockSummariesResponse _dataLockSummariesResponse;
    private GetPriceEpisodesResponse _priceEpisodesResponse;
    private readonly GetApprenticeshipResponse _apprenticeshipResponse;
    private GetAllTrainingProgrammesResponse _allTrainingProgrammeResponse;

    private readonly List<DataLock> _dataLocksWithCourseMismatch = [];
    private readonly List<DataLock> _dataLocksWithOnlyPriceMismatch = [];
    private readonly List<PriceEpisode> _priceEpisodes = [];
    private readonly List<TrainingProgramme> _trainingProgrammes = [];

    private readonly DataLockRequestChangesRequestToViewModelMapper _mapper;
    private DataLockRequestChangesViewModel _viewModel;

    private readonly DateTime _courseAndPriceChangeFromDate = DateTime.Now.Date.AddDays(15);
    private const decimal CourseAndPriceChangeCost = 1500.00M;
    private const string CourseAndPriceChangeCourseCode = "222";
    private const string CourseAndPriceChangeCourseName = "Training 222";
    private const TriageStatus CourseAndPriceChangeTriageStatus = TriageStatus.Change;

    public async Task<DataLockRequestChangesViewModel> Map()
    {
        _viewModel = await _mapper.Map(_request);
        return _viewModel;
    }

    internal DataLockRequestChangesRequestToViewModelMapperTestsFixture VerifyGetApprenticeshipDatalockSummariesStatusIsCalled()
    {
        _mockCommitmentsApiClient.Verify(t => t.GetApprenticeshipDatalockSummariesStatus(_request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once());
        return this;
    }

    internal DataLockRequestChangesRequestToViewModelMapperTestsFixture VerifyGetPriceEpisodesIsCalled()
    {
        _mockCommitmentsApiClient.Verify(t => t.GetPriceEpisodes(_request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once());
        return this;
    }

    internal DataLockRequestChangesRequestToViewModelMapperTestsFixture VerifyGetApprenticeshipIsCalled()
    {
        _mockCommitmentsApiClient.Verify(t => t.GetApprenticeship(_request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once());
        return this;
    }

    internal DataLockRequestChangesRequestToViewModelMapperTestsFixture VerifyGetAllTrainingProgrammesIsCalled()
    {
        _mockCommitmentsApiClient.Verify(t => t.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()), Times.Once());
        return this;
    }

    internal void VerifyApprenticeshipHashedIdIsMapped()
    {
        Assert.That(_viewModel.ApprenticeshipHashedId, Is.EqualTo(_request.ApprenticeshipHashedId));
    }
        
    internal void VerifyAccountHashedIdIsMapped()
    {
        Assert.That(_viewModel.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
    }

    internal void VerifyAccountIdIsMapped()
    {
        Assert.That(_viewModel.AccountId, Is.EqualTo(_request.AccountId));
    }

    internal void VerifyApprenticeshipIdIsMapped()
    {
        Assert.That(_viewModel.ApprenticeshipId, Is.EqualTo(_request.ApprenticeshipId));
    }

    internal void VerifyProviderNameIsMapped()
    {
        Assert.That(_viewModel.ProviderName, Is.EqualTo(_apprenticeshipResponse.ProviderName));
    }

    internal void VerifyFirstNameIsMapped()
    {
        Assert.That(_viewModel.OriginalApprenticeship.FirstName, Is.EqualTo(_apprenticeshipResponse.FirstName));
    }

    internal void VerifyLastNameIsMapped()
    {
        Assert.That(_viewModel.OriginalApprenticeship.LastName, Is.EqualTo(_apprenticeshipResponse.LastName));
    }

    internal void VerifyDateOfBirthIsMapped()
    {
        Assert.That(_viewModel.OriginalApprenticeship.DateOfBirth, Is.EqualTo(_apprenticeshipResponse.DateOfBirth.Date));
    }

    internal void VerifyUlnIsMapped()
    {
        Assert.That(_viewModel.OriginalApprenticeship.ULN, Is.EqualTo(_apprenticeshipResponse.Uln));
    }

    internal void VerifyStartDateIsMapped()
    {
        var startDate = new DateTime(_apprenticeshipResponse.StartDate.Value.Year, _apprenticeshipResponse.StartDate.Value.Month, 1);
        Assert.That(_viewModel.OriginalApprenticeship.StartDate, Is.EqualTo(startDate));
    }

    internal void VerifyEndDateIsMapped()
    {
        var endDate = new DateTime(_apprenticeshipResponse.EndDate.Year, _apprenticeshipResponse.EndDate.Month, 1);
        Assert.That(_viewModel.OriginalApprenticeship.EndDate, Is.EqualTo(endDate));
    }

    internal void VerifyCourseCodeIsMapped()
    {
        Assert.That(_viewModel.OriginalApprenticeship.CourseCode, Is.EqualTo(_apprenticeshipResponse.CourseCode));
    }

    internal void VerifyCourseNameIsMapped()
    {
        Assert.That(_viewModel.OriginalApprenticeship.CourseName, Is.EqualTo(_apprenticeshipResponse.CourseName));
    }

    internal void VerifyCourseChangesIsMapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_viewModel.CourseChanges[0].CurrentStartDate, Is.EqualTo(DateTime.Now.Date));
            Assert.That(_viewModel.CourseChanges[0].CurrentEndDate, Is.EqualTo(DateTime.Now.Date.AddDays(100)));
            Assert.That(_viewModel.CourseChanges[0].CurrentTrainingProgram, Is.EqualTo("Training 111"));
            Assert.That(_viewModel.CourseChanges[0].IlrStartDate, Is.EqualTo(_courseAndPriceChangeFromDate));
            Assert.That(_viewModel.CourseChanges[0].IlrEndDate, Is.EqualTo(null));
            Assert.That(_viewModel.CourseChanges[0].IlrTrainingProgram, Is.EqualTo(CourseAndPriceChangeCourseName));
        });
    }

    internal void VerifyPriceChangesIsMapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_viewModel.PriceChanges[0].CurrentStartDate, Is.EqualTo(DateTime.Now.Date));
            Assert.That(_viewModel.PriceChanges[0].CurrentEndDate, Is.EqualTo(null));
            Assert.That(_viewModel.PriceChanges[0].CurrentCost, Is.EqualTo(1000.0M));
            Assert.That(_viewModel.PriceChanges[0].IlrStartDate, Is.EqualTo(_courseAndPriceChangeFromDate));
            Assert.That(_viewModel.PriceChanges[0].IlrEndDate, Is.EqualTo(null));
            Assert.That(_viewModel.PriceChanges[0].IlrCost, Is.EqualTo(CourseAndPriceChangeCost));
        });
    }

    internal void VerifyCourseChangesIsEmpty()
    {
        Assert.That(_viewModel.CourseChanges, Is.Empty);
    }

    internal void VerifyPriceChangesIsEmpty()
    {
        Assert.That(_viewModel.PriceChanges, Is.Empty);
    }

    public DataLockRequestChangesRequestToViewModelMapperTestsFixture()
    {
        // Arrange
        _autoFixture = new Fixture();
        _request = _autoFixture.Build<DataLockRequestChangesRequest>()
            .With(x => x.AccountHashedId, "123")
            .With(x => x.ApprenticeshipHashedId, "456")
            .Create();

        _dataLockSummariesResponse = _autoFixture.Build<GetDataLockSummariesResponse>()
            .With(x => x.DataLocksWithCourseMismatch, _dataLocksWithCourseMismatch)
            .With(x => x.DataLocksWithOnlyPriceMismatch, _dataLocksWithOnlyPriceMismatch)
            .Create();

        _priceEpisodes.Add(new PriceEpisode { FromDate = DateTime.Now.Date, ToDate = null, Cost = 1000.0M });
        _priceEpisodesResponse = _autoFixture.Build<GetPriceEpisodesResponse>()
            .With(x => x.PriceEpisodes, _priceEpisodes)
            .Create();

        _trainingProgrammes.Add(new TrainingProgramme { CourseCode = "111", ProgrammeType = ProgrammeType.Standard, Name = "Training 111" });
        _allTrainingProgrammeResponse = _autoFixture.Build<GetAllTrainingProgrammesResponse>()
            .With(x => x.TrainingProgrammes, _trainingProgrammes)
            .Create();

        _apprenticeshipResponse = _autoFixture.Build<GetApprenticeshipResponse>()
            .With(p => p.CourseCode, "111")
            .With(p => p.CourseName, "Training 111")
            .With(p => p.EndDate, DateTime.Now.Date.AddDays(100))
            .Create();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            
        _mockCommitmentsApiClient.Setup(r => r.GetApprenticeshipDatalockSummariesStatus(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_dataLockSummariesResponse);
            
        _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_priceEpisodesResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_apprenticeshipResponse);

        _mockCommitmentsApiClient.Setup(t => t.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_allTrainingProgrammeResponse);

        _mapper = new DataLockRequestChangesRequestToViewModelMapper(_mockCommitmentsApiClient.Object);
    }

    public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithCourseAndPriceDataLock()
    {
        return WithCourseAndPriceDataLock(_courseAndPriceChangeFromDate, CourseAndPriceChangeCost, CourseAndPriceChangeCourseCode, CourseAndPriceChangeTriageStatus, DataLockErrorCode.Dlock03 | DataLockErrorCode.Dlock07)
            .WithTrainingProgramme(CourseAndPriceChangeCourseCode, ProgrammeType.Standard, CourseAndPriceChangeCourseName);
    }

    public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithCourseDataLock(DataLockErrorCode courseDataLockErrorCode)
    {
        _dataLocksWithCourseMismatch.Add(
            new DataLock { IsResolved = false, DataLockStatus = Status.Fail, ErrorCode = courseDataLockErrorCode });

        _dataLockSummariesResponse = _autoFixture.Build<GetDataLockSummariesResponse>()
            .With(x => x.DataLocksWithCourseMismatch, _dataLocksWithCourseMismatch)
            .With(x => x.DataLocksWithOnlyPriceMismatch, _dataLocksWithOnlyPriceMismatch)
            .Create();

        _mockCommitmentsApiClient.Setup(r => r.GetApprenticeshipDatalockSummariesStatus(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_dataLockSummariesResponse);

        return this;
    }

    public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithPriceDataLock(DateTime ilrEffectiveFromDate)
    {
        _dataLocksWithOnlyPriceMismatch.Add(
            new DataLock { IsResolved = false, DataLockStatus = Status.Fail, ErrorCode = DataLockErrorCode.Dlock07, IlrEffectiveFromDate = ilrEffectiveFromDate });

        _dataLockSummariesResponse = _autoFixture.Build<GetDataLockSummariesResponse>()
            .With(x => x.DataLocksWithCourseMismatch, _dataLocksWithCourseMismatch)
            .With(x => x.DataLocksWithOnlyPriceMismatch, _dataLocksWithOnlyPriceMismatch)
            .Create();

        _mockCommitmentsApiClient.Setup(r => r.GetApprenticeshipDatalockSummariesStatus(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_dataLockSummariesResponse);

        return this;
    }

    private DataLockRequestChangesRequestToViewModelMapperTestsFixture WithCourseAndPriceDataLock(DateTime ilrEffectiveFromDate, decimal totalCost, string ilrTrainingCourseCode, TriageStatus triageStatus, DataLockErrorCode courseDataLockErrorCode)
    {
        _dataLocksWithCourseMismatch.Add(
            new DataLock 
            { 
                IsResolved = false, 
                DataLockStatus = Status.Fail, 
                IlrEffectiveFromDate = ilrEffectiveFromDate,
                IlrTotalCost = totalCost,
                IlrTrainingCourseCode = ilrTrainingCourseCode,
                TriageStatus = triageStatus,
                ErrorCode = courseDataLockErrorCode | DataLockErrorCode.Dlock07 
            });

        _dataLockSummariesResponse = _autoFixture.Build<GetDataLockSummariesResponse>()
            .With(x => x.DataLocksWithCourseMismatch, _dataLocksWithCourseMismatch)
            .With(x => x.DataLocksWithOnlyPriceMismatch, _dataLocksWithOnlyPriceMismatch)
            .Create();

        _mockCommitmentsApiClient.Setup(r => r.GetApprenticeshipDatalockSummariesStatus(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_dataLockSummariesResponse);

        return this;
    }

    public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithPriceEpisode(DateTime fromDate, DateTime? toDate, decimal cost)
    {
        _priceEpisodes.Clear();
        _priceEpisodes.Add(
            new PriceEpisode { FromDate = fromDate, ToDate = toDate, Cost = cost });

        _priceEpisodesResponse = _autoFixture.Build<GetPriceEpisodesResponse>()
            .With(x => x.PriceEpisodes, _priceEpisodes)
            .Create();

        _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_priceEpisodesResponse);

        return this;
    }

    private DataLockRequestChangesRequestToViewModelMapperTestsFixture WithTrainingProgramme(string courseCode, ProgrammeType programmeType, string name)
    {
        _trainingProgrammes.Add(
            new TrainingProgramme { CourseCode = courseCode, ProgrammeType = programmeType, Name = name });

        _allTrainingProgrammeResponse = _autoFixture.Build<GetAllTrainingProgrammesResponse>()
            .With(x => x.TrainingProgrammes, _trainingProgrammes)
            .Create();

        _mockCommitmentsApiClient.Setup(t => t.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_allTrainingProgrammeResponse);

        return this;
    }
}