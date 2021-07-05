using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetApprenticeshipUpdatesResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfPartyRequestsResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfProviderChainResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ApprenticeshipDetailsRequestToViewModelMapperTests
    {
        Fixture autoFixture = new Fixture();
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<IEncodingService> _mockEncodingService;
        private Mock<IAuthorizationService> _authorizationService;
        private GetApprenticeshipResponse _apprenticeshipResponse;
        private GetPriceEpisodesResponse _priceEpisodesResponse;
        private GetApprenticeshipUpdatesResponse _apprenticeshipUpdatesResponse;
        private GetDataLocksResponse _dataLocksResponse;
        private GetChangeOfPartyRequestsResponse _changeOfPartyRequestsResponse;
        private GetTrainingProgrammeResponse _trainingProgrammeResponse;
        private GetChangeOfProviderChainResponse _changeOfProviderChainReponse;
        private ApprenticeshipDetailsRequest _request;
        private ApprenticeshipDetailsRequestToViewModelMapper _mapper;
        
        private const long ApprenticeshipIdFirst = 456;
        private const long ApprenticeshipIdMiddle = 356;
        private const long ApprenticeshipIdLast = 256;

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
                .With(x => x.CourseCode, "ABC")
                .With(x => x.DateOfBirth, autoFixture.Create<DateTime>())
                .Create();            
            _priceEpisodesResponse = autoFixture.Build<GetPriceEpisodesResponse>()
                 .With(x => x.PriceEpisodes, new List<PriceEpisode> {
                    new PriceEpisode { Cost = 1000, ToDate = DateTime.Now.AddMonths(-1)}})
                .Create();            
            _apprenticeshipUpdatesResponse = autoFixture.Build<GetApprenticeshipUpdatesResponse>()
                .With(x => x.ApprenticeshipUpdates, new List<ApprenticeshipUpdate> { 
                    new ApprenticeshipUpdate { OriginatingParty = Party.Employer } })
                .Create();       
            _dataLocksResponse = autoFixture.Build<GetDataLocksResponse>().Create();
            _changeOfPartyRequestsResponse = autoFixture.Build<GetChangeOfPartyRequestsResponse>().Create();
            _trainingProgrammeResponse = autoFixture.Build<GetTrainingProgrammeResponse>().Create();
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
            _mockCommitmentsApiClient.Setup(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode, It.IsAny<CancellationToken>()))
               .ReturnsAsync(_trainingProgrammeResponse);
            _mockCommitmentsApiClient.Setup(t => t.GetChangeOfProviderChain(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_changeOfProviderChainReponse);

            _mockEncodingService = new Mock<IEncodingService>();
            _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), It.IsAny<EncodingType>()))
                .Returns((long value, EncodingType encodingType) => $"A{value}");
            _mockEncodingService.Setup(t => t.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
                .Returns((string value, EncodingType encodingType) => long.Parse(Regex.Replace(value, "[A-Za-z ]", "")));

            _authorizationService = new Mock<IAuthorizationService>();

            _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object, Mock.Of<ILogger<ApprenticeshipDetailsRequestToViewModelMapper>>(), _authorizationService.Object);
        }

       [Test]
        public async Task GetTrainingProgrammeIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode, It.IsAny<CancellationToken>()), Times.Once());
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
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_trainingProgrammeResponse.TrainingProgramme.Name, result.TrainingName);
        }

        [Test]
        public async Task Price_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_priceEpisodesResponse.PriceEpisodes.First().Cost, result.Cost);
        }

        [TestCase(ApprenticeshipStatus.Live, "Live")]
        [TestCase(ApprenticeshipStatus.Paused, "Paused")]
        [TestCase(ApprenticeshipStatus.WaitingToStart, "Waiting to start")]
        [TestCase(ApprenticeshipStatus.Stopped, "Stopped")]
        [TestCase(ApprenticeshipStatus.Completed, "Completed")]
        public async Task StatusText_IsMapped(ApprenticeshipStatus status, string  statusText)
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
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_trainingProgrammeResponse.TrainingProgramme.ProgrammeType, result.TrainingType);
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

        [TestCase(ChangeOfPartyRequestStatus.Approved, false)]
        [TestCase(ChangeOfPartyRequestStatus.Rejected, false)]
        [TestCase(ChangeOfPartyRequestStatus.Withdrawn, false)]
        [TestCase(ChangeOfPartyRequestStatus.Pending, true)]
        public async Task HasPendingChangeOfEmployerRequest_IsMapped(ChangeOfPartyRequestStatus changeOfPartyRequestStatus, bool pendingChangeRequest)
        {
            //Arrange
            _changeOfPartyRequestsResponse.ChangeOfPartyRequests = new List<GetChangeOfPartyRequestsResponse.ChangeOfPartyRequest>()
            {
                new GetChangeOfPartyRequestsResponse.ChangeOfPartyRequest
                {
                    Id = 1,
                    ChangeOfPartyType = ChangeOfPartyRequestType.ChangeEmployer,
                    OriginatingParty = Party.Employer,
                    Status = changeOfPartyRequestStatus,
                    WithParty = Party.Provider
                }
            };

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(pendingChangeRequest, result.HasPendingChangeOfEmployerRequest);
        }

        [TestCase(ChangeOfPartyRequestStatus.Approved, ChangeOfPartyRequestType.ChangeProvider, false)]
        [TestCase(ChangeOfPartyRequestStatus.Pending, ChangeOfPartyRequestType.ChangeProvider, false)]
        [TestCase(ChangeOfPartyRequestStatus.Rejected, ChangeOfPartyRequestType.ChangeProvider, true)]
        [TestCase(ChangeOfPartyRequestStatus.Withdrawn, ChangeOfPartyRequestType.ChangeProvider, true)]
        [TestCase(ChangeOfPartyRequestStatus.Approved, ChangeOfPartyRequestType.ChangeEmployer, false)]
        [TestCase(ChangeOfPartyRequestStatus.Pending, ChangeOfPartyRequestType.ChangeEmployer, false)]
        [TestCase(ChangeOfPartyRequestStatus.Rejected, ChangeOfPartyRequestType.ChangeEmployer, true)]
        [TestCase(ChangeOfPartyRequestStatus.Withdrawn, ChangeOfPartyRequestType.ChangeEmployer, true)]
        public async Task ShowChangeTrainingProviderLink_IsMapped_When_Change_Of_Party(ChangeOfPartyRequestStatus changeOfPartyRequestStatus, ChangeOfPartyRequestType changeOfPartyRequestType, bool flag)
        {
            //Arrange 
            _apprenticeshipResponse.Status = ApprenticeshipStatus.Stopped;
            _changeOfPartyRequestsResponse.ChangeOfPartyRequests = new List<ChangeOfPartyRequest>
            {
               new ChangeOfPartyRequest
                    {
                        Status = changeOfPartyRequestStatus,
                        ChangeOfPartyType = changeOfPartyRequestType,
                        NewApprenticeshipId = _apprenticeshipResponse.Id + 1
                    }
            };

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(result.ShowChangeTrainingProviderLink, flag);
        }

        [TestCase(ApprenticeshipStatus.Stopped, true)]
        [TestCase(ApprenticeshipStatus.Paused, true)]
        [TestCase(ApprenticeshipStatus.Live, true)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, true)]
        public async Task ShowChangeProviderLink_IsMapped_When_No_Change_Of_Party(ApprenticeshipStatus apprenticeshipStatus, bool expected)
        {
            //Arrange
            _apprenticeshipResponse.Status = apprenticeshipStatus;
            _changeOfPartyRequestsResponse.ChangeOfPartyRequests = new List<ChangeOfPartyRequest>();

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(result.ShowChangeTrainingProviderLink, expected);
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

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(result.TrainingProviderHistory.Count, historyCount);
            for(int counter=0; counter < result.TrainingProviderHistory.Count; counter++)
            {
                if(counter == linkHidden)
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
                .With(x => x.DateOfBirth, autoFixture.Create<DateTime>())
                .With(x => x.ConfirmationStatus, confirmationStatus).Create();

            _mockCommitmentsApiClient.Setup(x => x.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_apprenticeshipResponse);

            // Act 
            var result = await _mapper.Map(_request);

            // Assert
            Assert.AreEqual(result.ConfirmationStatus, confirmationStatus);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task CheckShowConfirmationColumnIsMappedCorrectly(bool show)
        {
            _authorizationService.Setup(x => x.IsAuthorizedAsync(EmployerFeature.ApprenticeEmail))
                    .ReturnsAsync(show);

            var result = await _mapper.Map(_request);

            Assert.AreEqual(show, result.ShowApprenticeConfirmationColumn);
        }
    }
}
