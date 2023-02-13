using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
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
            _fixture.VerifyULNIsMapped();
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
        private Fixture AutoFixture;
        
        private DataLockRequestChangesRequest _request;
        
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private GetDataLockSummariesResponse _dataLockSummariesResponse;
        private GetPriceEpisodesResponse _priceEpisodesResponse;
        private GetApprenticeshipResponse _apprenticeshipResponse;
        private GetAllTrainingProgrammesResponse _allTrainingProgrammeResponse;

        private List<DataLock> DataLocksWithCourseMismatch = new List<DataLock>();
        private List<DataLock> DataLocksWithOnlyPriceMismatch = new List<DataLock>();
        private List<PriceEpisode> PriceEpisodes = new List<PriceEpisode>();
        private List<TrainingProgramme> TrainingProgrammes = new List<TrainingProgramme>();

        private DataLockRequestChangesRequestToViewModelMapper _mapper;
        private DataLockRequestChangesViewModel _viewModel;

        private DateTime CourseAndPriceChangeFromDate = DateTime.Now.Date.AddDays(15);
        private decimal CourseAndPriceChangeCost = 1500.00M;
        private string CourseAndPriceChangeCourseCode = "222";
        private string CourseAndPriceChangeCourseName = "Training 222";
        private TriageStatus CourseAndPriceChangeTriageStatus = TriageStatus.Change;

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
            Assert.AreEqual(_request.ApprenticeshipHashedId, _viewModel.ApprenticeshipHashedId);
        }
        
        internal void VerifyAccountHashedIdIsMapped()
        {
            Assert.AreEqual(_request.AccountHashedId, _viewModel.AccountHashedId);
        }

        internal void VerifyAccountIdIsMapped()
        {
            Assert.AreEqual(_request.AccountId, _viewModel.AccountId);
        }

        internal void VerifyApprenticeshipIdIsMapped()
        {
            Assert.AreEqual(_request.ApprenticeshipId, _viewModel.ApprenticeshipId);
        }

        internal void VerifyProviderNameIsMapped()
        {
            Assert.AreEqual(_apprenticeshipResponse.ProviderName, _viewModel.ProviderName);
        }

        internal void VerifyFirstNameIsMapped()
        {
            Assert.AreEqual(_apprenticeshipResponse.FirstName, _viewModel.OriginalApprenticeship.FirstName);
        }

        internal void VerifyLastNameIsMapped()
        {
            Assert.AreEqual(_apprenticeshipResponse.LastName, _viewModel.OriginalApprenticeship.LastName);
        }

        internal void VerifyDateOfBirthIsMapped()
        {
            Assert.AreEqual(_apprenticeshipResponse.DateOfBirth.Date, _viewModel.OriginalApprenticeship.DateOfBirth);
        }

        internal void VerifyULNIsMapped()
        {
            Assert.AreEqual(_apprenticeshipResponse.Uln, _viewModel.OriginalApprenticeship.ULN);
        }

        internal void VerifyStartDateIsMapped()
        {
            var startDate = new DateTime(_apprenticeshipResponse.StartDate.Value.Year, _apprenticeshipResponse.StartDate.Value.Month, 1);
            Assert.AreEqual(startDate, _viewModel.OriginalApprenticeship.StartDate);
        }

        internal void VerifyEndDateIsMapped()
        {
            var endDate = new DateTime(_apprenticeshipResponse.EndDate.Year, _apprenticeshipResponse.EndDate.Month, 1);
            Assert.AreEqual(endDate, _viewModel.OriginalApprenticeship.EndDate);
        }

        internal void VerifyCourseCodeIsMapped()
        {
            Assert.AreEqual(_apprenticeshipResponse.CourseCode, _viewModel.OriginalApprenticeship.CourseCode);
        }

        internal void VerifyCourseNameIsMapped()
        {
            Assert.AreEqual(_apprenticeshipResponse.CourseName, _viewModel.OriginalApprenticeship.CourseName);
        }

        internal void VerifyCourseChangesIsMapped()
        {
            Assert.AreEqual(DateTime.Now.Date, _viewModel.CourseChanges[0].CurrentStartDate);
            Assert.AreEqual(DateTime.Now.Date.AddDays(100), _viewModel.CourseChanges[0].CurrentEndDate);
            Assert.AreEqual("Training 111", _viewModel.CourseChanges[0].CurrentTrainingProgram);
            Assert.AreEqual(CourseAndPriceChangeFromDate, _viewModel.CourseChanges[0].IlrStartDate);
            Assert.AreEqual(null, _viewModel.CourseChanges[0].IlrEndDate);
            Assert.AreEqual(CourseAndPriceChangeCourseName, _viewModel.CourseChanges[0].IlrTrainingProgram);
        }

        internal void VerifyPriceChangesIsMapped()
        {
            Assert.AreEqual(DateTime.Now.Date, _viewModel.PriceChanges[0].CurrentStartDate);
            Assert.AreEqual(null, _viewModel.PriceChanges[0].CurrentEndDate);
            Assert.AreEqual(1000.0M, _viewModel.PriceChanges[0].CurrentCost);
            Assert.AreEqual(CourseAndPriceChangeFromDate, _viewModel.PriceChanges[0].IlrStartDate);
            Assert.AreEqual(null, _viewModel.PriceChanges[0].IlrEndDate);
            Assert.AreEqual(CourseAndPriceChangeCost, _viewModel.PriceChanges[0].IlrCost);
        }

        internal void VerifyCourseChangesIsEmpty()
        {
            Assert.AreEqual(0, _viewModel.CourseChanges.Count);
        }

        internal void VerifyPriceChangesIsEmpty()
        {
            Assert.AreEqual(0, _viewModel.PriceChanges.Count);
        }

        public DataLockRequestChangesRequestToViewModelMapperTestsFixture()
        {
            // Arrange
            AutoFixture = new Fixture();
            _request = AutoFixture.Build<DataLockRequestChangesRequest>()
                .With(x => x.AccountHashedId, "123")
                .With(x => x.ApprenticeshipHashedId, "456")
                .Create();

            _dataLockSummariesResponse = AutoFixture.Build<GetDataLockSummariesResponse>()
                .With(x => x.DataLocksWithCourseMismatch, DataLocksWithCourseMismatch)
                .With(x => x.DataLocksWithOnlyPriceMismatch, DataLocksWithOnlyPriceMismatch)
                .Create();

            PriceEpisodes.Add(new PriceEpisode { FromDate = DateTime.Now.Date, ToDate = null, Cost = 1000.0M });
            _priceEpisodesResponse = AutoFixture.Build<GetPriceEpisodesResponse>()
                 .With(x => x.PriceEpisodes, PriceEpisodes)
                .Create();

            TrainingProgrammes.Add(new TrainingProgramme { CourseCode = "111", ProgrammeType = ProgrammeType.Standard, Name = "Training 111" });
            _allTrainingProgrammeResponse = AutoFixture.Build<GetAllTrainingProgrammesResponse>()
                .With(x => x.TrainingProgrammes, TrainingProgrammes)
                .Create();

            _apprenticeshipResponse = AutoFixture.Build<GetApprenticeshipResponse>()
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
            return WithCourseAndPriceDataLock(CourseAndPriceChangeFromDate, CourseAndPriceChangeCost, CourseAndPriceChangeCourseCode, CourseAndPriceChangeTriageStatus, DataLockErrorCode.Dlock03 | DataLockErrorCode.Dlock07)
                .WithTrainingProgramme(CourseAndPriceChangeCourseCode, ProgrammeType.Standard, CourseAndPriceChangeCourseName);
        }

        public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithCourseDataLock(DataLockErrorCode courseDataLockErrorCode)
        {
            DataLocksWithCourseMismatch.Add(
                new DataLock { IsResolved = false, DataLockStatus = Status.Fail, ErrorCode = courseDataLockErrorCode });

            _dataLockSummariesResponse = AutoFixture.Build<GetDataLockSummariesResponse>()
                .With(x => x.DataLocksWithCourseMismatch, DataLocksWithCourseMismatch)
                .With(x => x.DataLocksWithOnlyPriceMismatch, DataLocksWithOnlyPriceMismatch)
                .Create();

            _mockCommitmentsApiClient.Setup(r => r.GetApprenticeshipDatalockSummariesStatus(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_dataLockSummariesResponse);

            return this;
        }

        public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithPriceDataLock(DateTime ilrEffectiveFromDate)
        {
            DataLocksWithOnlyPriceMismatch.Add(
                new DataLock { IsResolved = false, DataLockStatus = Status.Fail, ErrorCode = DataLockErrorCode.Dlock07, IlrEffectiveFromDate = ilrEffectiveFromDate });

            _dataLockSummariesResponse = AutoFixture.Build<GetDataLockSummariesResponse>()
                .With(x => x.DataLocksWithCourseMismatch, DataLocksWithCourseMismatch)
                .With(x => x.DataLocksWithOnlyPriceMismatch, DataLocksWithOnlyPriceMismatch)
                .Create();

            _mockCommitmentsApiClient.Setup(r => r.GetApprenticeshipDatalockSummariesStatus(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_dataLockSummariesResponse);

            return this;
        }

        public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithCourseAndPriceDataLock(DateTime IlrEffectiveFromDate, decimal totalCost, string ilrTrainingCourseCode, TriageStatus triageStatus, DataLockErrorCode courseDataLockErrorCode)
        {
            DataLocksWithCourseMismatch.Add(
                new DataLock 
                { 
                    IsResolved = false, 
                    DataLockStatus = Status.Fail, 
                    IlrEffectiveFromDate = IlrEffectiveFromDate,
                    IlrTotalCost = totalCost,
                    IlrTrainingCourseCode = ilrTrainingCourseCode,
                    TriageStatus = triageStatus,
                    ErrorCode = courseDataLockErrorCode | DataLockErrorCode.Dlock07 
                });

            _dataLockSummariesResponse = AutoFixture.Build<GetDataLockSummariesResponse>()
                .With(x => x.DataLocksWithCourseMismatch, DataLocksWithCourseMismatch)
                .With(x => x.DataLocksWithOnlyPriceMismatch, DataLocksWithOnlyPriceMismatch)
                .Create();

            _mockCommitmentsApiClient.Setup(r => r.GetApprenticeshipDatalockSummariesStatus(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_dataLockSummariesResponse);

            return this;
        }

        public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithPriceEpisode(DateTime fromDate, DateTime? toDate, decimal cost)
        {
            PriceEpisodes.Clear();
            PriceEpisodes.Add(
                new PriceEpisode { FromDate = fromDate, ToDate = toDate, Cost = cost });

            _priceEpisodesResponse = AutoFixture.Build<GetPriceEpisodesResponse>()
                 .With(x => x.PriceEpisodes, PriceEpisodes)
                .Create();

            _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_priceEpisodesResponse);

            return this;
        }

        public DataLockRequestChangesRequestToViewModelMapperTestsFixture WithTrainingProgramme(string courseCode, ProgrammeType programmeType, string name)
        {
            TrainingProgrammes.Add(
                new TrainingProgramme { CourseCode = courseCode, ProgrammeType = programmeType, Name = name });

            _allTrainingProgrammeResponse = AutoFixture.Build<GetAllTrainingProgrammesResponse>()
                .With(x => x.TrainingProgrammes, TrainingProgrammes)
                .Create();

            _mockCommitmentsApiClient.Setup(t => t.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(_allTrainingProgrammeResponse);

            return this;
        }
    }
}