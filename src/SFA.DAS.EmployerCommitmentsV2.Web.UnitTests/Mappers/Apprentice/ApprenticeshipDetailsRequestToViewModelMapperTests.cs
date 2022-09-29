using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetApprenticeshipUpdatesResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfProviderChainResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ApprenticeshipDetailsRequestToViewModelMapperTests
    {
        private Fixture autoFixture = new Fixture();
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<IApprovalsApiClient> _approvalsApiClient;
        private Mock<IEncodingService> _mockEncodingService;
        private GetApprenticeshipResponse _apprenticeshipResponse;
        private GetPriceEpisodesResponse _priceEpisodesResponse;
        private GetApprenticeshipUpdatesResponse _apprenticeshipUpdatesResponse;
        private GetDataLocksResponse _dataLocksResponse;
        private GetChangeOfPartyRequestsResponse _changeOfPartyRequestsResponse;

        private GetOverlappingTrainingDateRequestResponce _overlappingTrainingDateRequestResponce;

        private GetTrainingProgrammeResponse _getTrainingProgrammeByStandardUId;
        private GetNewerTrainingProgrammeVersionsResponse _newerTrainingProgrammeVersionsResponse;
        private GetTrainingProgrammeResponse _getTrainingProgrammeResponse;
        private GetChangeOfProviderChainResponse _changeOfProviderChainReponse;
        private ApprenticeshipDetailsRequest _request;
        private ApprenticeshipDetailsRequestToViewModelMapper _mapper;

        private const long ApprenticeshipIdFirst = 456;
        private const long ApprenticeshipIdMiddle = 356;
        private const long ApprenticeshipIdLast = 256;
        private const string ApprenticeshipEmail = "a@a.com";

        private GetApprenticeshipDetailsResponse _apprenticeshipDetailsResponse;

        [SetUp]
        public void SetUp()
        {
            //Arrange
            _request = autoFixture.Build<ApprenticeshipDetailsRequest>()
                .With(x => x.AccountHashedId, $"A123")
                .With(x => x.ApprenticeshipHashedId, $"A{ApprenticeshipIdFirst}")
                .Create();
            _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                .With(x => x.Id, ApprenticeshipIdFirst)
                .With(x => x.CourseCode, "123")
                .With(x => x.StandardUId, "ST0001_1.0")
                .With(x => x.Version, "1.0")
                .With(x => x.DateOfBirth, autoFixture.Create<DateTime>())
                .Create();
            _priceEpisodesResponse = autoFixture.Build<GetPriceEpisodesResponse>()
                 .With(x => x.PriceEpisodes, new List<PriceEpisode> {
                    new PriceEpisode { Cost = 1000, ToDate = DateTime.Now.AddMonths(-1)}})
                .Create();

            _overlappingTrainingDateRequestResponce = autoFixture.Create<GetOverlappingTrainingDateRequestResponce>();

            _apprenticeshipUpdatesResponse = autoFixture.Build<GetApprenticeshipUpdatesResponse>()
                .With(x => x.ApprenticeshipUpdates, new List<ApprenticeshipUpdate> {
                    new ApprenticeshipUpdate { OriginatingParty = Party.Employer } })
                .Create();
            _dataLocksResponse = autoFixture.Build<GetDataLocksResponse>().Create();
            _changeOfPartyRequestsResponse = autoFixture.Build<GetChangeOfPartyRequestsResponse>().Create();

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
            _mockCommitmentsApiClient.Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_apprenticeshipResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_priceEpisodesResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeshipUpdates(It.IsAny<long>(), It.IsAny<GetApprenticeshipUpdatesRequest>(), CancellationToken.None))
                .ReturnsAsync(_apprenticeshipUpdatesResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeshipDatalocksStatus(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_dataLocksResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetChangeOfPartyRequests(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_changeOfPartyRequestsResponse);
            _mockCommitmentsApiClient.Setup(t => t.GetNewerTrainingProgrammeVersions(It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(_newerTrainingProgrammeVersionsResponse);
            _mockCommitmentsApiClient.Setup(t => t.GetChangeOfProviderChain(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_changeOfProviderChainReponse);
            _mockCommitmentsApiClient.Setup(c => c.GetTrainingProgrammeVersionByStandardUId(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(_getTrainingProgrammeByStandardUId);
            _mockCommitmentsApiClient.Setup(c => c.GetTrainingProgramme(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(_getTrainingProgrammeResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetOverlappingTrainingDateRequest(It.IsAny<long>(), CancellationToken.None))
               .ReturnsAsync(_overlappingTrainingDateRequestResponce);

            _mockEncodingService = new Mock<IEncodingService>();
            _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), It.IsAny<EncodingType>()))
                .Returns((long value, EncodingType encodingType) => $"A{value}");
            _mockEncodingService.Setup(t => t.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
                .Returns((string value, EncodingType encodingType) => long.Parse(Regex.Replace(value, "[A-Za-z ]", "")));

            _apprenticeshipDetailsResponse = autoFixture.Create<GetApprenticeshipDetailsResponse>();

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetApprenticeshipDetails(_apprenticeshipResponse.ProviderId, _apprenticeshipResponse.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_apprenticeshipDetailsResponse);

            _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());
        }

        [Test]
        public async Task GetNewerTrainingProgrammeVersionsIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetNewerTrainingProgrammeVersions(_apprenticeshipResponse.StandardUId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetApprenticeshipIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetPriceEpisodesIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetApprenticeshipUpdatesIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetApprenticeshipUpdates(It.IsAny<long>(), It.IsAny<GetApprenticeshipUpdatesRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetApprenticeshipDatalocksStatusIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetApprenticeshipDatalocksStatus(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetChangeOfPartyRequestsIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetChangeOfPartyRequests(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetChangeOfProviderChainIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetChangeOfProviderChain(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task HashedApprenticeshipId_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_request.ApprenticeshipHashedId, result.HashedApprenticeshipId);
        }

        [Test]
        public async Task AccountHashedId_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_request.AccountHashedId, result.AccountHashedId);
        }

        [Test]
        public async Task ApprenticeName_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.FirstName + " " + _apprenticeshipResponse.LastName, result.ApprenticeName);
        }

        [Test]
        public async Task ULN_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.Uln, result.ULN);
        }

        [Test]
        public async Task DateOfBirth_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.DateOfBirth, result.DateOfBirth);
        }

        [Test]
        public async Task StartDate_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.StartDate, result.StartDate);
        }

        [Test]
        public async Task EndDate_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.EndDate, result.EndDate);
        }

        [Test]
        public async Task StopDate_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.StopDate, result.StopDate);
        }

        [Test]
        public async Task CanResendInvitationLink_IsFalse_WhenStopped()
        {
            //Act
            _apprenticeshipResponse.Status = ApprenticeshipStatus.Stopped;
            var result = await _mapper.Map(_request);

            //Assert
            Assert.IsFalse(result.CanResendInvitation);
        }

        [Test]
        public async Task PauseDate_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.PauseDate, result.PauseDate);
        }

        [Test]
        public async Task CompletionDate_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.CompletionDate, result.CompletionDate);
        }

        [Test]
        public async Task TrainingName_IsMapped()
        {
            //Arrange
            var expectedVersion = _getTrainingProgrammeByStandardUId.TrainingProgramme;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expectedVersion.Name, result.TrainingName);
        }

        [Test]
        public async Task Option_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.Option, result.Option);
        }

        [Test]
        public async Task Price_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_priceEpisodesResponse.PriceEpisodes.First().Cost, result.Cost);
        }

        [TestCase(DeliveryModel.PortableFlexiJob, DeliveryModel.PortableFlexiJob)]
        [TestCase(DeliveryModel.Regular, null)]
        public async Task DeliveryModel_IsMapped(DeliveryModel dm, DeliveryModel expected)
        {
            _apprenticeshipResponse.DeliveryModel = dm;
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expected, result.DeliveryModel);
        }

        [Test]
        public async Task EmploymentEndDate_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.EmploymentEndDate, result.EmploymentEndDate);
        }

        [Test]
        public async Task EmploymentPrice_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.EmploymentPrice, result.EmploymentPrice);
        }

        [TestCase(ApprenticeshipStatus.Live, "Live")]
        [TestCase(ApprenticeshipStatus.Paused, "Paused")]
        [TestCase(ApprenticeshipStatus.WaitingToStart, "Waiting to start")]
        [TestCase(ApprenticeshipStatus.Stopped, "Stopped")]
        [TestCase(ApprenticeshipStatus.Completed, "Completed")]
        public async Task StatusText_IsMapped(ApprenticeshipStatus status, string statusText)
        {
            //Arrange
            _apprenticeshipResponse.Status = status;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(statusText, result.ApprenticeshipStatus.GetDescription());
        }

        [Test]
        public async Task ProviderName_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.ProviderName, result.ProviderName);
        }

        [Test]
        public async Task PendingChanges_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(PendingChanges.WaitingForApproval, result.PendingChanges);
        }

        [Test]
        public async Task EmployerReference_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.EmployerReference, result.EmployerReference);
        }

        [Test]
        public async Task CohortReference_IsMapped()
        {
            //Arrange
            var CohortReference = string.Empty;
            _mockEncodingService
              .Setup(service => service.Encode(_apprenticeshipResponse.CohortId, EncodingType.CohortReference))
              .Returns(CohortReference);

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(CohortReference, result.CohortReference);
        }

        [TestCase(DataLockErrorCode.Dlock03, false)]
        [TestCase(DataLockErrorCode.Dlock04, false)]
        [TestCase(DataLockErrorCode.Dlock05, false)]
        [TestCase(DataLockErrorCode.Dlock06, false)]
        public async Task EnableEdit_HasDataLockCourseChangeTriaged_IsMapped(DataLockErrorCode dataLockErrorCode, bool expectedTriageOption)
        {
            //Arrange
            _apprenticeshipUpdatesResponse.ApprenticeshipUpdates = new List<ApprenticeshipUpdate>
            {
                new ApprenticeshipUpdate()
                {
                    OriginatingParty = Party.None
                }
            };
            _dataLocksResponse.DataLocks = new List<DataLock>
            { new DataLock
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
            Assert.AreEqual(expectedTriageOption, result.EnableEdit);
        }

        [TestCase(DataLockErrorCode.Dlock07, false)]
        public async Task EnableEdit_HasDataLockPriceTriaged_IsMapped(DataLockErrorCode dataLockErrorCode, bool expectedTriageOption)
        {
            //Arrange
            _apprenticeshipUpdatesResponse.ApprenticeshipUpdates = new List<ApprenticeshipUpdate>
            {
                new ApprenticeshipUpdate()
                {
                    OriginatingParty = Party.None
                }
            };
            _dataLocksResponse.DataLocks = new List<DataLock>
            { new DataLock
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
            Assert.AreEqual(expectedTriageOption, result.EnableEdit);
        }

        [TestCase(DataLockErrorCode.Dlock03, false)]
        [TestCase(DataLockErrorCode.Dlock04, false)]
        [TestCase(DataLockErrorCode.Dlock05, false)]
        [TestCase(DataLockErrorCode.Dlock06, false)]
        [TestCase(DataLockErrorCode.Dlock07, false)]
        public async Task DataLock_TriageStatus_Mapped(DataLockErrorCode dataLockErrorCode, bool expectedTriageOption)
        {
            //Arrange
            _dataLocksResponse.DataLocks = new List<DataLock>
            { new DataLock
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
            Assert.AreEqual(expectedTriageOption, result.EnableEdit);
        }

        [TestCase(ApprenticeshipStatus.Live, true)]
        [TestCase(ApprenticeshipStatus.Paused, true)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, true)]
        [TestCase(ApprenticeshipStatus.Stopped, false)]
        [TestCase(ApprenticeshipStatus.Completed, false)]
        public async Task CanEditStaus_IsMapped(ApprenticeshipStatus status, bool expectedAllowEditApprentice)
        {
            //Arrange
            _apprenticeshipResponse.Status = status;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expectedAllowEditApprentice, result.CanEditStatus);
        }

        [Test]
        public async Task EndpointAssessorName_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.EndpointAssessorName, result.EndpointAssessorName);
        }

        [Test]
        public async Task TrainingType_IsMapped()
        {
            //Arrange
            var expectedVersion = _getTrainingProgrammeByStandardUId.TrainingProgramme;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expectedVersion.ProgrammeType, result.TrainingType);
        }

        [TestCase(ChangeOfPartyRequestStatus.Approved, false)]
        [TestCase(ChangeOfPartyRequestStatus.Rejected, false)]
        [TestCase(ChangeOfPartyRequestStatus.Withdrawn, false)]
        [TestCase(ChangeOfPartyRequestStatus.Pending, true)]
        public async Task HasPendingChangeOfProviderRequest_IsMapped(ChangeOfPartyRequestStatus changeOfPartyRequestStatus, bool pendingChangeRequest)
        {
            //Arrange
            _changeOfPartyRequestsResponse.ChangeOfPartyRequests = new List<GetChangeOfPartyRequestsResponse.ChangeOfPartyRequest>()
            {
                new GetChangeOfPartyRequestsResponse.ChangeOfPartyRequest
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
            Assert.AreEqual(pendingChangeRequest, result.HasPendingChangeOfProviderRequest);
        }

        [TestCase(12345, false)]
        [TestCase(null, true)]
        public async Task ShowChangeTrainingProviderLink_IsMapped_When_HasContinuation(long? continuedBy, bool expected)
        {
            //Arrange
            _apprenticeshipResponse.Status = ApprenticeshipStatus.Stopped;
            _apprenticeshipResponse.ContinuedById = continuedBy;
            _apprenticeshipResponse.DeliveryModel = DeliveryModel.Regular;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expected, result.ShowChangeTrainingProviderLink);
        }

        [TestCase(ApprenticeshipStatus.Stopped, true)]
        [TestCase(ApprenticeshipStatus.Paused, true)]
        [TestCase(ApprenticeshipStatus.Live, true)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, true)]
        [TestCase(ApprenticeshipStatus.Completed, false)]
        public async Task ShowChangeProviderLink_IsMapped_For_ApprenticeshipStatus(ApprenticeshipStatus apprenticeshipStatus, bool expected)
        {
            //Arrange
            _apprenticeshipResponse.Status = apprenticeshipStatus;
            _apprenticeshipResponse.ContinuedById = null;
            _apprenticeshipResponse.DeliveryModel = DeliveryModel.Regular;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expected, result.ShowChangeTrainingProviderLink);
        }

        [TestCase(DeliveryModel.PortableFlexiJob, false)]
        [TestCase(DeliveryModel.Regular, true)]
        public async Task ShowChangeTrainingProviderLink_IsMapped_When_DeliveryModelIsSet(DeliveryModel dm, bool expected)
        {
            //Arrange
            _apprenticeshipResponse.Status = ApprenticeshipStatus.Stopped;
            _apprenticeshipResponse.ContinuedById = null;
            _apprenticeshipResponse.DeliveryModel = dm;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expected, result.ShowChangeTrainingProviderLink);
        }

        [TestCase(ApprenticeshipIdFirst, 0, 3)]
        [TestCase(ApprenticeshipIdMiddle, 1, 3)]
        [TestCase(ApprenticeshipIdLast, 2, 3)]
        public async Task TrainingProviderHistory_IsMapped_When_Change_Of_Provider_Chain(long apprenticeshipId, int linkHidden, int historyCount)
        {
            //Arrange
            DateTime now = DateTime.Now;
            _request.ApprenticeshipHashedId = $"A{apprenticeshipId}";
            _apprenticeshipResponse.Id = apprenticeshipId;
            _apprenticeshipResponse.Status = ApprenticeshipStatus.WaitingToStart;
            _changeOfProviderChainReponse.ChangeOfProviderChain = new List<ChangeOfProviderLink>
            {
                new ChangeOfProviderLink
                {
                    ApprenticeshipId = ApprenticeshipIdFirst,
                    StartDate = now,
                    EndDate = now.AddDays(10),
                    StopDate = null,
                    ProviderName = "ProviderFirst",
                    CreatedOn = now.AddDays(-12)
                },
                new ChangeOfProviderLink
                {
                    ApprenticeshipId = ApprenticeshipIdMiddle,
                    StartDate = now.AddDays(-10),
                    EndDate = now.AddDays(-5),
                    StopDate = now.AddDays(-10),
                    ProviderName = "ProviderMiddle",
                    CreatedOn = now.AddDays(-22)
                },
                new ChangeOfProviderLink
                {
                    ApprenticeshipId = ApprenticeshipIdLast,
                    StartDate = now.AddDays(-20),
                    EndDate = now.AddDays(-15),
                    StopDate = now.AddDays(-20),
                    ProviderName = "ProviderLast",
                    CreatedOn = now.AddDays(-30)
                }
            };

            _apprenticeshipDetailsResponse = autoFixture.Create<GetApprenticeshipDetailsResponse>();

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetApprenticeshipDetails(_apprenticeshipResponse.ProviderId, _apprenticeshipResponse.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_apprenticeshipDetailsResponse);

            _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(result.TrainingProviderHistory.Count, historyCount);
            for (int counter = 0; counter < result.TrainingProviderHistory.Count; counter++)
            {
                if (counter == linkHidden)
                    Assert.IsFalse(result.TrainingProviderHistory[counter].ShowLink);
                else
                    Assert.IsTrue(result.TrainingProviderHistory[counter].ShowLink);
            }
        }

        [TestCase(null)]
        [TestCase(ConfirmationStatus.Unconfirmed)]
        [TestCase(ConfirmationStatus.Confirmed)]
        [TestCase(ConfirmationStatus.Overdue)]
        public async Task GetApprenticeshipConfirmationStatus_IsMappedCorrectly(ConfirmationStatus? confirmationStatus)
        {
            // Arrange
            _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                .With(x => x.Id, ApprenticeshipIdFirst)
                .With(x => x.CourseCode, "ABC")
                .With(x => x.Version, "1.0")
                .With(x => x.DateOfBirth, autoFixture.Create<DateTime>())
                .With(x => x.ConfirmationStatus, confirmationStatus).Create();

            _mockCommitmentsApiClient.Setup(x => x.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_apprenticeshipResponse);

            _apprenticeshipDetailsResponse = autoFixture.Create<GetApprenticeshipDetailsResponse>();

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetApprenticeshipDetails(_apprenticeshipResponse.ProviderId, _apprenticeshipResponse.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_apprenticeshipDetailsResponse);

            _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

            // Act
            var result = await _mapper.Map(_request);

            // Assert
            Assert.AreEqual(result.ConfirmationStatus, confirmationStatus);
        }

        [Test]
        public async Task CheckEmailIsMappedCorrectly()
        {
            _apprenticeshipResponse.Email = ApprenticeshipEmail;

            var result = await _mapper.Map(_request);

            Assert.AreEqual(ApprenticeshipEmail, result.Email);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task CheckEmailShouldBePresentIsMappedCorrectly(bool expected)
        {
            _apprenticeshipResponse.EmailShouldBePresent = expected;

            var result = await _mapper.Map(_request);

            Assert.AreEqual(expected, result.EmailShouldBePresent);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task HasNewerVersionsIsMappedCorrectly(bool hasNewerVersions)
        {
            if (hasNewerVersions == false)
                _newerTrainingProgrammeVersionsResponse.NewerVersions = new List<TrainingProgramme>();

            var result = await _mapper.Map(_request);

            Assert.AreEqual(hasNewerVersions, result.HasNewerVersions);
        }

        [Test]
        public async Task VersionOptionsAreMappedCorrectly()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_getTrainingProgrammeByStandardUId.TrainingProgramme.Options, result.VersionOptions);
        }

        [Test]
        public async Task RecognisePriorLearning_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.RecognisePriorLearning, result.RecognisePriorLearning);
        }

        [Test]
        public async Task PriceReducedByIsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.PriceReducedBy, result.PriceReducedBy);
        }

        [Test]
        public async Task DurationReducedByIsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.DurationReducedBy, result.DurationReducedBy);
        }

        [Test]
        public async Task HasPendingOverlappingTrainingDateRequestIsMappedWhenStatusIsPending()
        {
            //Arrange
            _overlappingTrainingDateRequestResponce = autoFixture.Create<GetOverlappingTrainingDateRequestResponce>();
            _overlappingTrainingDateRequestResponce.OverlappingTrainingDateRequest.First().Status = OverlappingTrainingDateRequestStatus.Pending;

            _mockCommitmentsApiClient
                .Setup(c => c.GetOverlappingTrainingDateRequest(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_overlappingTrainingDateRequestResponce);

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetApprenticeshipDetails(_apprenticeshipResponse.ProviderId, _apprenticeshipResponse.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_apprenticeshipDetailsResponse);

            _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(true, result.HasPendingOverlappingTrainingDateRequest);
        }

        [Test]
        public async Task HasPendingOverlappingTrainingDateRequestIsMappedWhenNooverlappingTrainingDateRequest()
        {
            //Arrange
            _overlappingTrainingDateRequestResponce = null;

            _mockCommitmentsApiClient
              .Setup(c => c.GetOverlappingTrainingDateRequest(It.IsAny<long>(), CancellationToken.None))
              .ReturnsAsync(_overlappingTrainingDateRequestResponce);

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetApprenticeshipDetails(_apprenticeshipResponse.ProviderId, _apprenticeshipResponse.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_apprenticeshipDetailsResponse);

            _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(false, result.HasPendingOverlappingTrainingDateRequest);
        }

        [Test]
        public async Task HasPendingOverlappingTrainingDateRequestIsMappedWhenStatusIsRejected()
        {
            //Arrange
            _overlappingTrainingDateRequestResponce = autoFixture.Create<GetOverlappingTrainingDateRequestResponce>();
            foreach (var request in _overlappingTrainingDateRequestResponce.OverlappingTrainingDateRequest)
            {
                request.Status = OverlappingTrainingDateRequestStatus.Rejected;
            }

            _mockCommitmentsApiClient
              .Setup(c => c.GetOverlappingTrainingDateRequest(It.IsAny<long>(), CancellationToken.None))
              .ReturnsAsync(_overlappingTrainingDateRequestResponce);

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetApprenticeshipDetails(_apprenticeshipResponse.ProviderId, _apprenticeshipResponse.Id, It.IsAny<CancellationToken>())).ReturnsAsync(_apprenticeshipDetailsResponse);

            _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, _approvalsApiClient.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>());

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(false, result.HasPendingOverlappingTrainingDateRequest);
        }

        [TestCase(OverlappingTrainingDateRequestStatus.Pending, false)]
        [TestCase(OverlappingTrainingDateRequestStatus.Resolved, true)]
        [TestCase(OverlappingTrainingDateRequestStatus.Rejected, true)]
        public async Task CheckEnableEdit_WhenMappingOverlappingTrainingDateRequest_IsMapped(OverlappingTrainingDateRequestStatus status, bool expected)
        {
            //Arrange
            _apprenticeshipUpdatesResponse.ApprenticeshipUpdates = new List<ApprenticeshipUpdate>
            {
                new ApprenticeshipUpdate()
                {
                    OriginatingParty = Party.None
                }
            };

            _dataLocksResponse.DataLocks = new List<DataLock>();

            _apprenticeshipResponse.Status = ApprenticeshipStatus.Live;

            _overlappingTrainingDateRequestResponce = autoFixture.Create<GetOverlappingTrainingDateRequestResponce>();

            foreach (var request in _overlappingTrainingDateRequestResponce.OverlappingTrainingDateRequest)
            {
                request.Status = status;
            }

            _mockCommitmentsApiClient.Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
             .ReturnsAsync(_apprenticeshipResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeshipUpdates(It.IsAny<long>(), It.IsAny<GetApprenticeshipUpdatesRequest>(), CancellationToken.None))
                .ReturnsAsync(_apprenticeshipUpdatesResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeshipDatalocksStatus(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_dataLocksResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetOverlappingTrainingDateRequest(It.IsAny<long>(), CancellationToken.None))
               .ReturnsAsync(_overlappingTrainingDateRequestResponce);

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expected, result.EnableEdit);
        }
    }

    public static class MockCommitmentsApiExtensions
    {
        public static Mock<ICommitmentsApiClient> WithEditableState(this Mock<ICommitmentsApiClient> client)
        {
            client.Setup(c => c.GetApprenticeshipUpdates(It.IsAny<long>(), It.IsAny<GetApprenticeshipUpdatesRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetApprenticeshipUpdatesResponse { ApprenticeshipUpdates = new ApprenticeshipUpdate[] { } });
            client.Setup(c => c.GetApprenticeshipDatalocksStatus(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(new GetDataLocksResponse { DataLocks = new DataLock[] { } });
            return client;
        }
    }
}