using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetApprenticeshipUpdatesResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ApprenticeshipDetailsRequestToViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<IEncodingService> _mockEncodingService;

        private GetApprenticeshipResponse _apprenticeshipResponse;
        private GetPriceEpisodesResponse _priceEpisodesResponse;
        private GetApprenticeshipUpdatesResponse _apprenticeshipUpdatesResponse;
        private GetDataLocksResponse _dataLocksResponse;
        private GetChangeOfPartyRequestsResponse _changeOfPartyRequestsResponse;
        private GetTrainingProgrammeResponse _trainingProgrammeResponse;

        private ApprenticeshipDetailsRequest _request;
        private ApprenticeshipDetailsRequestToViewModelMapper _mapper;



        [SetUp]
        public void SetUp()
        {
            //Arrange
            var autoFixture = new Fixture();

            _request = autoFixture.Build<ApprenticeshipDetailsRequest>()
                .With(x => x.AccountHashedId, "123")
                .With(x => x.ApprenticeshipHashedId, "456")
                .Create();

            _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
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

            _mockEncodingService = new Mock<IEncodingService>();

            _mapper = new ApprenticeshipDetailsRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object);
        }

       [Test]
        public async Task GetFundingCapIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode, It.IsAny<CancellationToken>()), Times.Once());
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
        public async Task FirstName_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.FirstName, result.FirstName);
        }

        [Test]
        public async Task LastName_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.LastName, result.LastName);
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
        public async Task ThenStatusTextIsMappedCorrectly(ApprenticeshipStatus status, string  statusText)
        {
            //Arrange
            _apprenticeshipResponse.Status = status;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(statusText, result.Status);
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

        [Test]
        public async Task dataLockCourseTriaged_IsMapped()
        {
            //Arrange
            _dataLocksResponse.DataLocks = new List<GetDataLocksResponse.DataLock>
            { new GetDataLocksResponse.DataLock
                {
                    Id = 1,
                    TriageStatus = TriageStatus.Unknown,
                    DataLockStatus = Status.Fail,
                    IsResolved = false
                },
            };

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_apprenticeshipResponse.EmployerReference, result.EmployerReference);
        }


        [TestCase(false, DataLockErrorCode.Dlock03, false)]
        [TestCase(false, DataLockErrorCode.Dlock04, false)]
        [TestCase(false, DataLockErrorCode.Dlock05, false)]
        [TestCase(true, DataLockErrorCode.Dlock06, false)]        
        public async Task With_Single_Datalock_Then_AvailableTriageOption_Is_Mapped_Correctly(bool hasHadDataLockSuccess, DataLockErrorCode dataLockErrorCode, bool expectedTriageOption)
        {
            //Arrange
            _apprenticeshipResponse.HasHadDataLockSuccess = hasHadDataLockSuccess;
            _dataLocksResponse.DataLocks = new List<GetDataLocksResponse.DataLock>
            { new GetDataLocksResponse.DataLock
                {
                    Id = 1,
                    TriageStatus = TriageStatus.Unknown,
                    DataLockStatus = Status.Fail,
                    IsResolved = false,
                    ErrorCode = DataLockErrorCode.Dlock03
                },
            };

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expectedTriageOption, result.EnableEdit);
        }

        //[TestCase(false, DataLockErrorCode.Dlock03, false)]
        //[TestCase(false, DataLockErrorCode.Dlock04, false)]
        [TestCase(false, DataLockErrorCode.Dlock05, false)]
        [TestCase(true, DataLockErrorCode.Dlock06, false)]
        [TestCase(true, DataLockErrorCode.Dlock07, false)]
        public async Task With_Single_Datalock_Then_AvailableTriageOption_Is_Mapped_Correctly123(bool hasHadDataLockSuccess, DataLockErrorCode dataLockErrorCode, bool expectedTriageOption)
        {
            //Arrange
            _apprenticeshipResponse.HasHadDataLockSuccess = hasHadDataLockSuccess;
            _dataLocksResponse.DataLocks = new List<GetDataLocksResponse.DataLock>
            { new GetDataLocksResponse.DataLock
                {
                    Id = 1,
                    TriageStatus = TriageStatus.Change,
                    DataLockStatus = Status.Fail,
                    IsResolved = false,
                    ErrorCode = DataLockErrorCode.Dlock07
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

        [TestCase(ApprenticeshipStatus.Live, false)]
        [TestCase(ApprenticeshipStatus.Paused, false)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, false)]
        [TestCase(ApprenticeshipStatus.Stopped, true)]
        [TestCase(ApprenticeshipStatus.Completed, false)]
        public async Task CanEditStopDate_IsMapped(ApprenticeshipStatus status, bool expectedAllowEditApprentice)
        {
            //Arrange
            _apprenticeshipResponse.Status = status;

            //Act
            var result = await _mapper.Map(_request);

            //Assert            
            Assert.AreEqual(expectedAllowEditApprentice, result.CanEditStopDate);
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


        /*[TestCase(ApprenticeshipStatus.Live, true)]
        [TestCase(ApprenticeshipStatus.Paused, true)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, true)]
        [TestCase(ApprenticeshipStatus.Stopped, false)]
        [TestCase(ApprenticeshipStatus.Completed, false)]
        public async Task EnableEdit_IsMapped(ApprenticeshipStatus status, bool expectedAllowEditApprentice)
        {
            //Arrange
            _apprenticeshipResponse.Status = status;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(expectedAllowEditApprentice, result.EnableEdit);
        }*/

    }
}
