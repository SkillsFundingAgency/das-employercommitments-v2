using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System.Linq;
using System.Text.RegularExpressions;
using static SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.GetManageApprenticeshipDetailsResponse;
using static SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.GetManageApprenticeshipDetailsResponse.GetApprenticeshipUpdateResponse;
using static SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.GetManageApprenticeshipDetailsResponse.GetPriceEpisodeResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ApprenticeshipDetailsRequestToViewModelMapperTests
{
    private Fixture autoFixture = new Fixture();
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private Mock<IEncodingService> _mockEncodingService;
    private GetManageApprenticeshipDetailsResponse.GetApprenticeshipResponse _apprenticeshipResponse;
    private GetPriceEpisodeResponse _priceEpisodesResponse;
    private GetApprenticeshipUpdateResponse _apprenticeshipUpdatesResponse;
    private GetDataLockResponse _dataLocksResponse;
    private GetChangeOfPartyRequestResponse _changeOfPartyRequestsResponse;
    private GetApprenticeshipOverlappingTrainingDateResponse _overlappingTrainingDateRequestResponce;
            
    private GetTrainingProgrammeResponse _getTrainingProgrammeByStandardUId;
    private GetNewerTrainingProgrammeVersionsResponse _newerTrainingProgrammeVersionsResponse;
    private GetTrainingProgrammeResponse _getTrainingProgrammeResponse;
    private GetChangeOfProviderChainResponse _changeOfProviderChainReponse;
    private ApprenticeshipDetailsRequest _request;
    private ApprenticeshipDetailsRequestToViewModelMapper _mapper;
    public GetManageApprenticeshipDetailsResponse GetManageApprenticeshipDetailsResponse;

    private const long ApprenticeshipIdFirst = 456;
    private const long ApprenticeshipIdMiddle = 356;
    private const long ApprenticeshipIdLast = 256;
    private const string ApprenticeshipEmail = "a@a.com";

    private GetManageApprenticeshipDetailsResponse.GetApprenticeshipResponse _apprenticeshipDetailsResponse;

    [SetUp]
    public void SetUp()
    {
        //Arrange
        _request = autoFixture.Build<ApprenticeshipDetailsRequest>()
            .With(x => x.AccountHashedId, $"A123")
            .With(x => x.ApprenticeshipHashedId, $"A{ApprenticeshipIdFirst}")
            .Create();

        _apprenticeshipResponse = autoFixture.Build<GetManageApprenticeshipDetailsResponse.GetApprenticeshipResponse>()
            .With(x => x.Id, ApprenticeshipIdFirst)
            .With(x => x.CourseCode, "123")
            .With(x => x.StandardUId, "ST0001_1.0")
            .With(x => x.Version, "1.0")
            .With(x => x.DateOfBirth, autoFixture.Create<DateTime>())
            .Create();
        _priceEpisodesResponse = autoFixture.Build<GetPriceEpisodeResponse>()
            .With(x => x.PriceEpisodes, new List<PriceEpisode> {
                new PriceEpisode { Cost = 1000, ToDate = DateTime.Now.AddMonths(-1)}})
            .Create();

        _overlappingTrainingDateRequestResponce = autoFixture.Create<GetApprenticeshipOverlappingTrainingDateResponse>();

        _apprenticeshipUpdatesResponse = autoFixture.Build<GetApprenticeshipUpdateResponse>()
            .With(x => x.ApprenticeshipUpdates, new List<ApprenticeshipUpdate> {
                new ApprenticeshipUpdate { OriginatingParty = Party.Employer } })
            .Create();
        _dataLocksResponse = autoFixture.Build<GetDataLockResponse>().Create();
        _changeOfPartyRequestsResponse = autoFixture.Build<GetChangeOfPartyRequestResponse>().Create();

        var trainingProgrammeByStandardUId = autoFixture.Build<TrainingProgramme>()
            .With(x => x.CourseCode, _apprenticeshipResponse.CourseCode)
            .With(x => x.StandardUId, "ST0001_1.0")
            .With(x => x.Version, "1.0")
            .Create();
        _getTrainingProgrammeByStandardUId = new GetTrainingProgrammeResponse
        {
            TrainingProgramme = trainingProgrammeByStandardUId
        };

        var framework = autoFixture.Build<TrainingProgramme>()
            .Without(x => x.Version)
            .Without(x => x.StandardUId)
            .With(x => x.CourseCode, "1-2-3")
            .Create();
        _getTrainingProgrammeResponse = new GetTrainingProgrammeResponse
        {
            TrainingProgramme = framework
        };

        var trainingProgrammeVersions = autoFixture.Build<TrainingProgramme>().CreateMany(2).ToList();
        trainingProgrammeVersions[0].Version = "1.1";
        trainingProgrammeVersions[1].Version = "1.2";

        _newerTrainingProgrammeVersionsResponse = autoFixture.Build<GetNewerTrainingProgrammeVersionsResponse>()
            .With(x => x.NewerVersions, trainingProgrammeVersions).Create();

        _changeOfProviderChainReponse = autoFixture.Build<GetChangeOfProviderChainResponse>().Create();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _mockCommitmentsApiClient.Setup(t => t.GetNewerTrainingProgrammeVersions(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_newerTrainingProgrammeVersionsResponse);
        _mockCommitmentsApiClient.Setup(t => t.GetChangeOfProviderChain(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(_changeOfProviderChainReponse);
        _mockCommitmentsApiClient.Setup(c => c.GetTrainingProgrammeVersionByStandardUId(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(_getTrainingProgrammeByStandardUId);
        _mockCommitmentsApiClient.Setup(c => c.GetTrainingProgramme(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(_getTrainingProgrammeResponse);

        _mockEncodingService = new Mock<IEncodingService>();
        _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), It.IsAny<EncodingType>()))
            .Returns((long value, EncodingType encodingType) => $"A{value}");
        _mockEncodingService.Setup(t => t.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
            .Returns((string value, EncodingType encodingType) => long.Parse(Regex.Replace(value, "[A-Za-z ]", "")));

        _apprenticeshipDetailsResponse = autoFixture.Create<GetManageApprenticeshipDetailsResponse.GetApprenticeshipResponse>();

        _approvalsApiClient = new Mock<IApprovalsApiClient>();


        GetManageApprenticeshipDetailsResponse = autoFixture.Build<GetManageApprenticeshipDetailsResponse>()
            .With(x => x.HasMultipleDeliveryModelOptions, false)
            .With(x => x.ApprenticeshipUpdates)
            .With(x => x.ChangeOfPartyRequests)
            .With(x => x.PriceEpisodes, new List<PriceEpisode> { new PriceEpisode { Cost = 1000, ToDate = DateTime.Now.AddMonths(-1)}})
            .With(x => x.ChangeOfProviderChain)
            .With(x => x.DataLocks)
            .With(x => x.OverlappingTrainingDateRequest)
            .Create();

        _approvalsApiClient.Setup(x =>
                x.GetManageApprenticeshipDetails(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetManageApprenticeshipDetailsResponse);


        _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());
    }

    [Test]
    public async Task HashedApprenticeshipId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.HashedApprenticeshipId, Is.EqualTo(_request.ApprenticeshipHashedId));
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
    }

    [Test]
    public async Task ApprenticeName_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ApprenticeName, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.FirstName + " " + GetManageApprenticeshipDetailsResponse.Apprenticeship.LastName));
    }

    [Test]
    public async Task ULN_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ULN, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.Uln));
    }

    [Test]
    public async Task DateOfBirth_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.DateOfBirth, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.DateOfBirth));
    }

    [Test]
    public async Task StartDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.StartDate, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.StartDate));
    }

    [Test]
    public async Task EndDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EndDate, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.EndDate));
    }

    [Test]
    public async Task StopDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.StopDate, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.StopDate));
    }

    [Test]
    public async Task CanResendInvitationLink_IsFalse_WhenStopped()
    {
        //Act
        GetManageApprenticeshipDetailsResponse.Apprenticeship.Status = ApprenticeshipStatus.Stopped;
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.CanResendInvitation, Is.False);
    }

    [Test]
    public async Task PauseDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.PauseDate, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.PauseDate));
    }

    [Test]
    public async Task CompletionDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.CompletionDate, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.CompletionDate));
    }

    [Test]
    public async Task TrainingName_IsMapped()
    {
        //Arrange
        var expectedVersion = _getTrainingProgrammeByStandardUId.TrainingProgramme;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TrainingName, Is.EqualTo(expectedVersion.Name));
    }

    [Test]
    public async Task Option_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.Option, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.Option));
    }

    [Test]
    public async Task Price_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.Cost, Is.EqualTo(GetManageApprenticeshipDetailsResponse.PriceEpisodes.FirstOrDefault().Cost));
    }

    [TestCase(DeliveryModel.PortableFlexiJob, DeliveryModel.PortableFlexiJob)]
    [TestCase(DeliveryModel.Regular, DeliveryModel.Regular)]
    public async Task DeliveryModel_IsMapped(DeliveryModel dm, DeliveryModel? expected)
    {
        GetManageApprenticeshipDetailsResponse.Apprenticeship.DeliveryModel = dm;
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.DeliveryModel, Is.EqualTo(expected));
    }

    [Test]
    public async Task EmploymentEndDate_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EmploymentEndDate, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.EmploymentEndDate));
    }

    [Test]
    public async Task EmploymentPrice_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EmploymentPrice, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.EmploymentPrice));
    }

    [TestCase(ApprenticeshipStatus.Live, "Live")]
    [TestCase(ApprenticeshipStatus.Paused, "Paused")]
    [TestCase(ApprenticeshipStatus.WaitingToStart, "Waiting to start")]
    [TestCase(ApprenticeshipStatus.Stopped, "Stopped")]
    [TestCase(ApprenticeshipStatus.Completed, "Completed")]
    public async Task StatusText_IsMapped(ApprenticeshipStatus status, string statusText)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.Apprenticeship.Status = status;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ApprenticeshipStatus.GetDescription(), Is.EqualTo(statusText));
    }

    [Test]
    public async Task ProviderName_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ProviderName, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.ProviderName));
    }

    [Test]
    public async Task PendingChanges_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.PendingChanges, Is.EqualTo(PendingChanges.ReadyForApproval));
    }

    [Test]
    public async Task EmployerReference_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EmployerReference, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.EmployerReference));
    }

    [Test]
    public async Task CohortReference_IsMapped()
    {
        //Arrange
        var CohortReference = string.Empty;
        _mockEncodingService
            .Setup(service => service.Encode(GetManageApprenticeshipDetailsResponse.Apprenticeship.CohortId, EncodingType.CohortReference))
            .Returns(CohortReference);

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.CohortReference, Is.EqualTo(CohortReference));
    }

    [TestCase(DataLockErrorCode.Dlock03, false)]
    [TestCase(DataLockErrorCode.Dlock04, false)]
    [TestCase(DataLockErrorCode.Dlock05, false)]
    [TestCase(DataLockErrorCode.Dlock06, false)]
    public async Task EnableEdit_HasDataLockCourseChangeTriaged_IsMapped(DataLockErrorCode dataLockErrorCode, bool expectedTriageOption)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.ApprenticeshipUpdates = new List<GetManageApprenticeshipDetailsResponse.GetApprenticeshipUpdateResponse.ApprenticeshipUpdate>
        {
            new GetManageApprenticeshipDetailsResponse.GetApprenticeshipUpdateResponse.ApprenticeshipUpdate()
            {
                OriginatingParty = Party.None
            }
        };
        GetManageApprenticeshipDetailsResponse.DataLocks = new List<GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock>
        { new GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock
            {
                Id = 1,
                TriageStatus = TriageStatus.Restart,
                DataLockStatus = Status.Fail,
                IsResolved = false,
                ErrorCode = dataLockErrorCode
            },
        };

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EnableEdit, Is.EqualTo(expectedTriageOption));
    }

    [TestCase(DataLockErrorCode.Dlock07, false)]
    public async Task EnableEdit_HasDataLockPriceTriaged_IsMapped(DataLockErrorCode dataLockErrorCode, bool expectedTriageOption)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.ApprenticeshipUpdates = new List<ApprenticeshipUpdate>
        {
            new ApprenticeshipUpdate()
            {
                OriginatingParty = Party.None
            }
        };
        GetManageApprenticeshipDetailsResponse.DataLocks = new List<GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock>
        { new GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock
            {
                Id = 1,
                TriageStatus = TriageStatus.Change,
                DataLockStatus = Status.Fail,
                IsResolved = false,
                ErrorCode = dataLockErrorCode
            },
        };

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EnableEdit, Is.EqualTo(expectedTriageOption));
    }

    [TestCase(DataLockErrorCode.Dlock03, false)]
    [TestCase(DataLockErrorCode.Dlock04, false)]
    [TestCase(DataLockErrorCode.Dlock05, false)]
    [TestCase(DataLockErrorCode.Dlock06, false)]
    [TestCase(DataLockErrorCode.Dlock07, false)]
    public async Task DataLock_TriageStatus_Mapped(DataLockErrorCode dataLockErrorCode, bool expectedTriageOption)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.DataLocks = new List<GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock>
        { new GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock
            {
                Id = 1,
                TriageStatus = TriageStatus.Change,
                DataLockStatus = Status.Fail,
                IsResolved = false,
                ErrorCode = dataLockErrorCode
            },
        };

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EnableEdit, Is.EqualTo(expectedTriageOption));
    }

    [TestCase(ApprenticeshipStatus.Live, true)]
    [TestCase(ApprenticeshipStatus.Paused, true)]
    [TestCase(ApprenticeshipStatus.WaitingToStart, true)]
    [TestCase(ApprenticeshipStatus.Stopped, false)]
    [TestCase(ApprenticeshipStatus.Completed, false)]
    public async Task CanEditStaus_IsMapped(ApprenticeshipStatus status, bool expectedAllowEditApprentice)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.Apprenticeship.Status = status;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.CanEditStatus, Is.EqualTo(expectedAllowEditApprentice));
    }

    [Test]
    public async Task EndpointAssessorName_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EndpointAssessorName, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.EndpointAssessorName));
    }

    [Test]
    public async Task TrainingType_IsMapped()
    {
        //Arrange
        var expectedVersion = _getTrainingProgrammeByStandardUId.TrainingProgramme;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.TrainingType, Is.EqualTo(expectedVersion.ProgrammeType));
    }

    [TestCase(ChangeOfPartyRequestStatus.Approved, false)]
    [TestCase(ChangeOfPartyRequestStatus.Rejected, false)]
    [TestCase(ChangeOfPartyRequestStatus.Withdrawn, false)]
    [TestCase(ChangeOfPartyRequestStatus.Pending, true)]
    public async Task HasPendingChangeOfProviderRequest_IsMapped(ChangeOfPartyRequestStatus changeOfPartyRequestStatus, bool pendingChangeRequest)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.ChangeOfPartyRequests = new List<GetManageApprenticeshipDetailsResponse.GetChangeOfPartyRequestResponse.ChangeOfPartyRequest>()
        {
            new GetManageApprenticeshipDetailsResponse.GetChangeOfPartyRequestResponse.ChangeOfPartyRequest
            {
                Id = 1,
                ChangeOfPartyType = ChangeOfPartyRequestType.ChangeProvider,
                OriginatingParty = Party.Employer,
                Status = changeOfPartyRequestStatus,
                WithParty = Party.Provider
            }
        };

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.HasPendingChangeOfProviderRequest, Is.EqualTo(pendingChangeRequest));
    }

    [TestCase(12345, false)]
    [TestCase(null, true)]
    public async Task ShowChangeTrainingProviderLink_IsMapped_When_HasContinuation(long? continuedBy, bool expected)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.Apprenticeship.Status = ApprenticeshipStatus.Stopped;
        GetManageApprenticeshipDetailsResponse.Apprenticeship.ContinuedById = continuedBy;
        GetManageApprenticeshipDetailsResponse.Apprenticeship.DeliveryModel = DeliveryModel.Regular;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ShowChangeTrainingProviderLink, Is.EqualTo(expected));
    }

    [TestCase(ApprenticeshipStatus.Stopped, true)]
    [TestCase(ApprenticeshipStatus.Paused, true)]
    [TestCase(ApprenticeshipStatus.Live, true)]
    [TestCase(ApprenticeshipStatus.WaitingToStart, true)]
    [TestCase(ApprenticeshipStatus.Completed, false)]
    public async Task ShowChangeProviderLink_IsMapped_For_ApprenticeshipStatus(ApprenticeshipStatus apprenticeshipStatus, bool expected)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.Apprenticeship.Status = apprenticeshipStatus;
        GetManageApprenticeshipDetailsResponse.Apprenticeship.ContinuedById = null;
        GetManageApprenticeshipDetailsResponse.Apprenticeship.DeliveryModel = DeliveryModel.Regular;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ShowChangeTrainingProviderLink, Is.EqualTo(expected));
    }

    [TestCase(DeliveryModel.PortableFlexiJob, false)]
    [TestCase(DeliveryModel.Regular, true)]
    public async Task ShowChangeTrainingProviderLink_IsMapped_When_DeliveryModelIsSet(DeliveryModel dm, bool expected)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.Apprenticeship.Status = ApprenticeshipStatus.Stopped;
        GetManageApprenticeshipDetailsResponse.Apprenticeship.ContinuedById = null;
        GetManageApprenticeshipDetailsResponse.Apprenticeship.DeliveryModel = dm;

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.ShowChangeTrainingProviderLink, Is.EqualTo(expected));
    }

    [TestCase(ApprenticeshipIdFirst, 0, 3)]
    [TestCase(ApprenticeshipIdMiddle, 1, 3)]
    [TestCase(ApprenticeshipIdLast, 2, 3)]
    public async Task TrainingProviderHistory_IsMapped_When_Change_Of_Provider_Chain(long apprenticeshipId, int linkHidden, int historyCount)
    {
        //Arrange
        DateTime now = DateTime.Now;
        _request.ApprenticeshipHashedId = $"A{apprenticeshipId}";
        GetManageApprenticeshipDetailsResponse.Apprenticeship.Id = apprenticeshipId;
        GetManageApprenticeshipDetailsResponse.Apprenticeship.Status = ApprenticeshipStatus.WaitingToStart;
        GetManageApprenticeshipDetailsResponse.ChangeOfProviderChain = new List<GetManageApprenticeshipDetailsResponse.GetChangeOfProviderLinkResponse.ChangeOfProviderLink>
        {
            new GetManageApprenticeshipDetailsResponse.GetChangeOfProviderLinkResponse.ChangeOfProviderLink
            {
                ApprenticeshipId = ApprenticeshipIdFirst,
                StartDate = now,
                EndDate = now.AddDays(10),
                StopDate = null,
                ProviderName = "ProviderFirst",
                CreatedOn = now.AddDays(-12)
            },
            new GetManageApprenticeshipDetailsResponse.GetChangeOfProviderLinkResponse.ChangeOfProviderLink
            {
                ApprenticeshipId = ApprenticeshipIdMiddle,
                StartDate = now.AddDays(-10),
                EndDate = now.AddDays(-5),
                StopDate = now.AddDays(-10),
                ProviderName = "ProviderMiddle",
                CreatedOn = now.AddDays(-22)
            },
            new GetManageApprenticeshipDetailsResponse.GetChangeOfProviderLinkResponse.ChangeOfProviderLink
            {
                ApprenticeshipId = ApprenticeshipIdLast,
                StartDate = now.AddDays(-20),
                EndDate = now.AddDays(-15),
                StopDate = now.AddDays(-20),
                ProviderName = "ProviderLast",
                CreatedOn = now.AddDays(-30)
            }
        };


        _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(historyCount, Is.EqualTo(result.TrainingProviderHistory.Count));
        for (int counter = 0; counter < result.TrainingProviderHistory.Count; counter++)
        {
            if (counter == linkHidden)
                Assert.That(result.TrainingProviderHistory[counter].ShowLink, Is.False);
            else
                Assert.That(result.TrainingProviderHistory[counter].ShowLink, Is.True);
        }
    }

    [TestCase(null)]
    [TestCase(ConfirmationStatus.Unconfirmed)]
    [TestCase(ConfirmationStatus.Confirmed)]
    [TestCase(ConfirmationStatus.Overdue)]
    public async Task GetApprenticeshipConfirmationStatus_IsMappedCorrectly(ConfirmationStatus? confirmationStatus)
    {
        // Arrange
        GetManageApprenticeshipDetailsResponse.Apprenticeship = autoFixture.Build<GetManageApprenticeshipDetailsResponse.GetApprenticeshipResponse>()
            .With(x => x.Id, ApprenticeshipIdFirst)
            .With(x => x.CourseCode, "ABC")
            .With(x => x.Version, "1.0")
            .With(x => x.DateOfBirth, autoFixture.Create<DateTime>())
            .With(x => x.ConfirmationStatus, confirmationStatus).Create();


        _apprenticeshipDetailsResponse = autoFixture.Create<GetManageApprenticeshipDetailsResponse.GetApprenticeshipResponse>();

        _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

        // Act
        var result = await _mapper.Map(_request);

        // Assert
        Assert.That(confirmationStatus, Is.EqualTo(result.ConfirmationStatus));
    }

    [Test]
    public async Task CheckEmailIsMappedCorrectly()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.Email, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.Email));
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task CheckEmailShouldBePresentIsMappedCorrectly(bool expected)
    {
        GetManageApprenticeshipDetailsResponse.Apprenticeship.EmailShouldBePresent = expected;

        var result = await _mapper.Map(_request);

        Assert.That(result.EmailShouldBePresent, Is.EqualTo(expected));
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task HasNewerVersionsIsMappedCorrectly(bool hasNewerVersions)
    {
        if (hasNewerVersions == false)
            _newerTrainingProgrammeVersionsResponse.NewerVersions = new List<TrainingProgramme>();

        var result = await _mapper.Map(_request);

        Assert.That(result.HasNewerVersions, Is.EqualTo(hasNewerVersions));
    }

    [Test]
    public async Task VersionOptionsAreMappedCorrectly()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.VersionOptions, Is.EqualTo(_getTrainingProgrammeByStandardUId.TrainingProgramme.Options));
    }

    [Test]
    public async Task RecognisePriorLearning_IsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.RecognisePriorLearning, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.RecognisePriorLearning));
    }

    [Test]
    public async Task PriceReducedByIsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.PriceReducedBy, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.PriceReducedBy));
    }

    [Test]
    public async Task DurationReducedByIsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.DurationReducedBy, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.DurationReducedBy));
    }

    [Test]
    public async Task HasPendingOverlappingTrainingDateRequestIsMappedWhenStatusIsPending()
    {
        //Arrange
        _overlappingTrainingDateRequestResponce = autoFixture.Create<GetManageApprenticeshipDetailsResponse.GetApprenticeshipOverlappingTrainingDateResponse>();
        _overlappingTrainingDateRequestResponce.ApprenticeshipOverlappingTrainingDates.First().Status = OverlappingTrainingDateRequestStatus.Pending;

        _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.HasPendingOverlappingTrainingDateRequest, Is.EqualTo(true));
    }

    [Test]
    public async Task HasPendingOverlappingTrainingDateRequestIsMappedWhenNooverlappingTrainingDateRequest()
    {
        //Arrange
        _overlappingTrainingDateRequestResponce = null;

        _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.HasPendingOverlappingTrainingDateRequest, Is.EqualTo(true));
    }

    [Test]
    public async Task HasPendingOverlappingTrainingDateRequestIsMappedWhenStatusIsRejected()
    {
        //Arrange
        _overlappingTrainingDateRequestResponce = autoFixture.Create<GetManageApprenticeshipDetailsResponse.GetApprenticeshipOverlappingTrainingDateResponse>();
        foreach (var request in _overlappingTrainingDateRequestResponce.ApprenticeshipOverlappingTrainingDates)
        {
            request.Status = OverlappingTrainingDateRequestStatus.Rejected;
        }

        _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.HasPendingOverlappingTrainingDateRequest, Is.EqualTo(true));
    }

    [TestCase(OverlappingTrainingDateRequestStatus.Pending, false)]
    [TestCase(OverlappingTrainingDateRequestStatus.Resolved, true)]
    [TestCase(OverlappingTrainingDateRequestStatus.Rejected, true)]
    public async Task CheckEnableEdit_WhenMappingOverlappingTrainingDateRequest_IsMapped(OverlappingTrainingDateRequestStatus status, bool expected)
    {
        //Arrange
        GetManageApprenticeshipDetailsResponse.ApprenticeshipUpdates = new List<GetManageApprenticeshipDetailsResponse.GetApprenticeshipUpdateResponse.ApprenticeshipUpdate>
        {
            new GetManageApprenticeshipDetailsResponse.GetApprenticeshipUpdateResponse.ApprenticeshipUpdate()
            {
                OriginatingParty = Party.None
            }
        };

        GetManageApprenticeshipDetailsResponse.DataLocks = new List<GetManageApprenticeshipDetailsResponse.GetDataLockResponse.DataLock>();

        GetManageApprenticeshipDetailsResponse.Apprenticeship.Status = ApprenticeshipStatus.Live;

        GetManageApprenticeshipDetailsResponse.OverlappingTrainingDateRequest = autoFixture.Create<List<GetApprenticeshipOverlappingTrainingDateResponse.ApprenticeshipOverlappingTrainingDate>>();

        foreach (var request in GetManageApprenticeshipDetailsResponse.OverlappingTrainingDateRequest)
        {
            request.Status = status;
        }

        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.EnableEdit, Is.EqualTo(expected));
    }

    [Test]
    public async Task IsOnFlexiPaymentPilotIsMapped()
    {
        //Act
        var result = await _mapper.Map(_request);

        //Assert
        Assert.That(result.IsOnFlexiPaymentPilot, Is.EqualTo(GetManageApprenticeshipDetailsResponse.Apprenticeship.IsOnFlexiPaymentPilot));
    }
}