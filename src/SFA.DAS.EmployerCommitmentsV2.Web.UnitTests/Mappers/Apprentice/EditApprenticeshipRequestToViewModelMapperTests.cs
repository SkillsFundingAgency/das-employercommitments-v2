using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class EditApprenticeshipRequestToViewModelMapperTests
    {
        private EditApprenticeshipRequestToViewModelMapperTestsFixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new EditApprenticeshipRequestToViewModelMapperTestsFixture();
        }

        [Test]
        public async Task GetApprenticeshipIsCalled()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyGetApprenticeshipIsCalled();
        }

        [Test]
        public async Task GetPriceEpisodesIsCalled()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyGetPriceEpisodeIsCalled();
        }

        [Test]
        public async Task GetCohortIsCalled()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyGetCohortIsCalled();
        }

        [Test]
        public async Task GetAccountIsCalled()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyGetAccountIsCalled();
        }

        [Test]
        public async Task For_LevyEmployer_GetAllTrainingProgramme_IsCalled()
        {
            //Setup
            _fixture.SetUpLevyAccount().NotTransferSender();

            //Act
            await _fixture.Map();

            //Assert
            _fixture
                .VerifyGetAllTrainingProgrammeIsCalled()
                .VerifyGetAllTrainingProgrammeStandardsIsNotCalled();
        }

        [Test]
        public async Task For_TransferSender_FundedApprenticeship_GetAllTrainingProgrammeStandard_IsCalled()
        {
            //Setup
            _fixture.SetUpLevyAccount().AsTransferSender();

            //Act
            await _fixture.Map();

            //Assert
            _fixture
                 .VerifyGetAllTrainingProgrammeIsNotCalled()
                 .VerifyGetAllTrainingProgrammeStandardsIsCalled();
        }

        [Test]
        public async Task For_NonLevyEmployer_GetAllTrainingProgrammeStandards_IsCalled()
        {
            //Setup
            _fixture.SetUpNonLevyAccount().NotTransferSender();

            //Act
            await _fixture.Map();

            //Assert
            _fixture
                .VerifyGetAllTrainingProgrammeIsNotCalled()
                .VerifyGetAllTrainingProgrammeStandardsIsCalled();
        }

        [Test]
        public async Task HashedApprenticeshipId_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyHashedApprenticeshipIdIsMapped();
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
        public async Task Email_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyEmailIsMapped();
        }

        [Test]
        public async Task ApprenticeshipConfirmationStatus_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyEmailAddressConfirmedByApprentice();
        }

        [Test]
        public async Task EmailShouldBePresent_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyEmailShouldBePresent();
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
        public async Task CourseCode_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyCourseCodeIsMapped();
        }

        [TestCase(DeliveryModel.PortableFlexiJob)]
        [TestCase(DeliveryModel.Regular)]
        public async Task DeliveryModel_IsMapped(DeliveryModel dm)
        {
            _fixture.ApprenticeshipResponse.DeliveryModel = dm;

            //Act
            await _fixture.Map();

            _fixture.VerifyDeliveryModelIsMapped();
        }

        [Test]
        public async Task EmploymentPrice_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyEmploymentPriceIsMapped();
        }

        [Test]
        public async Task NullEmploymentEndDate_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyEmploymentEndDateIsMapped();
        }


        [Test]
        public async Task EmploymentEndDate_IsMapped()
        {
            _fixture.ApprenticeshipResponse.EmploymentEndDate = new DateTime(2021, 09, 01);

            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyEmploymentEndDateIsMapped();
        }

        [Test]
        public async Task Version_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyCourseCodeIsMapped();
        }

        [Test]
        public async Task Cost_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyCoursesAreMapped();
        }

        [Test]
        public async Task EmployerReference_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyCoursesAreMapped();
        }

        [Test]
        public async Task Courses_AreMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyCoursesAreMapped();
        }

        [Test]
        public async Task IsContinuationIsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyIsLockedForUpdateIsMapped();
        }

        [TestCase(ApprenticeshipStatus.Live, true, true)]
        [TestCase(ApprenticeshipStatus.Live, false, false)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, true, false)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, false, false)]
        public async Task IsLockedForUpdate_Is_Mapped_With_ApprenticeshipStatus_And_DataLockSuccess_Condition(ApprenticeshipStatus status, bool hasHadDataLockSuccess, bool expectedIsLockedForUpdated)
        {
            _fixture.NotTransferSender()
                .IsWithInFundingPeriod()
                .SetApprenticeshipStatus(status)
                .SetDataLockSuccess(hasHadDataLockSuccess);

            //Act
             var viewModel = await _fixture.Map();

            //Assert
            Assert.AreEqual(expectedIsLockedForUpdated, viewModel.IsLockedForUpdate);
        }

        [TestCase(ApprenticeshipStatus.Live, true)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, false)]
        [TestCase(ApprenticeshipStatus.Paused, false)]
        public async Task IsLockedForUpdate_Is_Mapped_With_ApprenticeshipStatus_And_IsNotWithInFundingPeriod_Condition(ApprenticeshipStatus status, bool expectedIsLockedForUpdated)
        {
            _fixture.NotTransferSender()
                .IsNotWithInFundingPeriod()
                .SetApprenticeshipStatus(status)
                .SetDataLockSuccess(false);

            //Act
            var viewModel = await _fixture.Map();

            //Assert
            Assert.AreEqual(expectedIsLockedForUpdated, viewModel.IsLockedForUpdate);
        }

        [TestCase(ApprenticeshipStatus.WaitingToStart, true, true)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, false, false)]
        public async Task IsLockedForUpdate_Is_Mapped_With_ApprenticeshipStatus_And_IsFundedByTransfer_And_HasDataLockSuccess_Condition(ApprenticeshipStatus status, bool hasHadDataLockSuccess, bool expectedIsLockedForUpdated)
        {
            _fixture.AsTransferSender()
                .IsWithInFundingPeriod()
                .SetApprenticeshipStatus(status)
                .SetDataLockSuccess(hasHadDataLockSuccess);

            //Act
            var viewModel = await _fixture.Map();

            //Assert
            Assert.AreEqual(expectedIsLockedForUpdated, viewModel.IsLockedForUpdate);
        }

        [TestCase(true, false, true)]
        [TestCase(true, true, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public async Task IsUpdateLockedForStartDateAndCourse_Is_Mapped(bool isTransferSenderFundedApprenticeship, bool hasHadDataLockSuccess, bool expectedIsUpdateLockedForStartDateAndCourse)
        {
            //Arrange
            (isTransferSenderFundedApprenticeship ? _fixture.AsTransferSender() : _fixture.NotTransferSender()).SetDataLockSuccess(hasHadDataLockSuccess);

            //Act
            var viewModel = await _fixture.Map();

            //Assert
            Assert.AreEqual(expectedIsUpdateLockedForStartDateAndCourse, viewModel.IsUpdateLockedForStartDateAndCourse);
        }

        [TestCase(ApprenticeshipStatus.WaitingToStart, true, true)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, false, false)]
        [TestCase(ApprenticeshipStatus.Live, true, false)] // This is the scenario need to be tested, if it is Live and it has received the datalock success, our code is going to make the end date editable - check on v1.
        [TestCase(ApprenticeshipStatus.Live, false, false)]
        public async Task IsEndDateLockedForUpdate_Is_Mapped(ApprenticeshipStatus status, bool hasHadDataLockSuccess, bool expectedIsEndDateLockedForUpdate)
        {
            _fixture
                .NotTransferSender()
                .SetApprenticeshipStatus(status)
                .SetDataLockSuccess(hasHadDataLockSuccess);

            //Act
            var viewModel = await _fixture.Map();

            //Assert
            Assert.AreEqual(expectedIsEndDateLockedForUpdate, viewModel.IsEndDateLockedForUpdate);
        }

        [Test]
        public async Task ProviderId_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyProviderIdIsMapped();
        }

        [Test]
        public async Task AccountLegalEntityId_IsMapped()
        {
            //Act
            await _fixture.Map();

            //Assert
            _fixture.VerifyAccountLegalEntityIdIsMapped();
        }


        [TestCase(true)]
        [TestCase(false)]
        public async Task HasMultipleDeliveryModelOptions_IsMapped(bool hasMultiple)
        {
            _fixture.WithMultipleDeliveryModels(hasMultiple);

            //Act
            await _fixture.Map();

            _fixture.VerifyHasMultipleDeliveryModelsIsMapped();
        }

    }

    public class EditApprenticeshipRequestToViewModelMapperTestsFixture
    {
        public EditApprenticeshipRequest _request;
        public GetApprenticeshipResponse ApprenticeshipResponse { get; set; }
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<IAcademicYearDateProvider> _mockAcademicYearDateProvider;
        private Mock<ICurrentDateTime> _mockCurrentDateTimeProvider;
        private Mock<IEncodingService> _mockEncodingService;
        private Mock<IApprovalsApiClient> _mockApprovalsOuterApiClient;


        private GetPriceEpisodesResponse _priceEpisodesResponse;
        private GetEditApprenticeshipResponse _getEditApprenticeshipResponse;
        private AccountResponse _accountResponse;
        private GetAllTrainingProgrammeStandardsResponse _allTrainingProgrammeStandardsResponse;
        private GetAllTrainingProgrammesResponse _allTrainingProgrammeResponse;
        private EditApprenticeshipRequestToViewModelMapper _mapper;
        private EditApprenticeshipRequestViewModel _viewModel;
        private IEnumerable<TrainingProgramme> _courses;

        public async Task<EditApprenticeshipRequestViewModel> Map()
        {
            _viewModel = await _mapper.Map(_request);
            return _viewModel;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture VerifyGetApprenticeshipIsCalled()
        {
            _mockCommitmentsApiClient.Verify(t => t.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture VerifyGetPriceEpisodeIsCalled()
        {
            _mockCommitmentsApiClient.Verify(t => t.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture VerifyGetCohortIsCalled()
        {
            _mockCommitmentsApiClient.Verify(t => t.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture VerifyGetAccountIsCalled()
        {
            _mockCommitmentsApiClient.Verify(t => t.GetAccount(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture SetUpLevyAccount()
        {
            _accountResponse.LevyStatus = CommitmentsV2.Types.ApprenticeshipEmployerType.Levy;
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture SetUpNonLevyAccount()
        {
            _accountResponse.LevyStatus = CommitmentsV2.Types.ApprenticeshipEmployerType.NonLevy;
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture NotTransferSender()
        {
            _getEditApprenticeshipResponse.IsFundedByTransfer = false;
            return this;
        }
        internal EditApprenticeshipRequestToViewModelMapperTestsFixture AsTransferSender()
        {
            _getEditApprenticeshipResponse.IsFundedByTransfer = true;
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture WithMultipleDeliveryModels(bool value)
        {
            _getEditApprenticeshipResponse.HasMultipleDeliveryModelOptions = value;
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture VerifyGetAllTrainingProgrammeIsCalled()
        {
            _mockCommitmentsApiClient.Verify(t => t.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()), Times.Once());
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture VerifyGetAllTrainingProgrammeIsNotCalled()
        {
            _mockCommitmentsApiClient.Verify(t => t.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()), Times.Never());
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture VerifyGetAllTrainingProgrammeStandardsIsNotCalled()
        {
            _mockCommitmentsApiClient.Verify(t => t.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()), Times.Never());
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture VerifyGetAllTrainingProgrammeStandardsIsCalled()
        {
            _mockCommitmentsApiClient.Verify(t => t.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()), Times.Once());
            return this;
        }

        internal void VerifyHashedApprenticeshipIdIsMapped()
        {
            Assert.AreEqual(_request.ApprenticeshipHashedId, _viewModel.HashedApprenticeshipId);
        }

        internal void VerifyLastNameIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.LastName, _viewModel.LastName);
        }

        internal void VerifyEmailIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.Email, _viewModel.Email);
        }

        internal void VerifyEmailAddressConfirmedByApprentice()
        {
            Assert.AreEqual(ApprenticeshipResponse.EmailAddressConfirmedByApprentice, _viewModel.EmailAddressConfirmedByApprentice);
        }

        internal void VerifyEmailShouldBePresent()
        {
            Assert.AreEqual(ApprenticeshipResponse.EmailShouldBePresent, _viewModel.EmailShouldBePresent);
        }

        internal void VerifyULNIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.Uln, _viewModel.ULN);
        }

        internal void VerifyCourseCodeIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.CourseCode, _viewModel.CourseCode);
        }

        internal void VerifyEmploymentPriceIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.EmploymentPrice, _viewModel.EmploymentPrice);
        }

        internal void VerifyEmploymentEndDateIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.EmploymentEndDate, _viewModel.EmploymentEndDate.Date);
        }

        internal void VerifyDeliveryModelIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.DeliveryModel, _viewModel.DeliveryModel);
        }

        internal void VerifyVersionIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.Version, _viewModel.Version);
        }

        internal void VerifyCoursesAreMapped()
        {
            Assert.AreEqual(_courses, _viewModel.Courses);
        }

        internal void VerifyIsLockedForUpdateIsMapped()
        {
         //   throw new NotImplementedException();
        }

        internal void VerifyFirstNameIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.FirstName, _viewModel.FirstName);
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture IsWithInFundingPeriod()
        {
            ApprenticeshipResponse.StartDate = DateTime.Now.AddYears(-1);
            ApprenticeshipResponse.EndDate = DateTime.Now.AddYears(1);

            _mockCurrentDateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.Now);

            // Make the start date later than CurrentAcademicYearStartDate
            _mockAcademicYearDateProvider.Setup(t => t.CurrentAcademicYearStartDate).Returns(ApprenticeshipResponse.StartDate.AddMonths(-1));

            // Make the DateTime Now earlier than LastAcademicYearFundingPeriod
            _mockAcademicYearDateProvider.Setup(t => t.LastAcademicYearFundingPeriod).Returns(DateTime.Now.AddMonths(2));

            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture IsNotWithInFundingPeriod()
        {
            ApprenticeshipResponse.StartDate = DateTime.Now.AddYears(-1);
            ApprenticeshipResponse.EndDate = DateTime.Now.AddYears(1);

            _mockCurrentDateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.Now);

            // Make the start date earlier than CurrentAcademicYearStartDate
            _mockAcademicYearDateProvider.Setup(t => t.CurrentAcademicYearStartDate).Returns(ApprenticeshipResponse.StartDate.AddMonths(1));

            // Make the DateTime Now later than LastAcademicYearFundingPeriod
            _mockAcademicYearDateProvider.Setup(t => t.LastAcademicYearFundingPeriod).Returns(DateTime.Now.AddMonths(-1));

            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture SetApprenticeshipStatus(ApprenticeshipStatus status)
        {
            ApprenticeshipResponse.Status = status;
            return this;
        }

        internal EditApprenticeshipRequestToViewModelMapperTestsFixture SetDataLockSuccess(bool hasHadDataLockSuccess)
        {
            ApprenticeshipResponse.HasHadDataLockSuccess = hasHadDataLockSuccess;
            return this;
        }

        public EditApprenticeshipRequestToViewModelMapperTestsFixture()
        {
            //Arrange
            var autoFixture = new Fixture();
            _request = autoFixture.Build<EditApprenticeshipRequest>()
                .With(x => x.AccountHashedId, "123")
                .With(x => x.ApprenticeshipHashedId, "456")
                .Create();
            ApprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                .With(x => x.CourseCode, "ABC")
                .With(x => x.Version, "1.0")
                .With(x => x.DateOfBirth, autoFixture.Create<DateTime>())
                .Without(x => x.EmploymentEndDate)
                .Create();
            _priceEpisodesResponse = autoFixture.Build<GetPriceEpisodesResponse>()
                 .With(x => x.PriceEpisodes, new List<PriceEpisode> {
                    new PriceEpisode { Cost = 1000, ToDate = DateTime.Now.AddMonths(-1)}})
                .Create();

            _getEditApprenticeshipResponse = autoFixture.Create<GetEditApprenticeshipResponse>();
            _accountResponse = autoFixture.Create<AccountResponse>();
            _allTrainingProgrammeStandardsResponse = autoFixture.Create<GetAllTrainingProgrammeStandardsResponse>();
            _allTrainingProgrammeResponse = autoFixture.Create<GetAllTrainingProgrammesResponse>();
   
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockCommitmentsApiClient.Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(ApprenticeshipResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_priceEpisodesResponse);

            _mockApprovalsOuterApiClient = new Mock<IApprovalsApiClient>();
            _mockApprovalsOuterApiClient.Setup(t => t.GetEditApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _getEditApprenticeshipResponse);

            _mockCommitmentsApiClient.Setup(t => t.GetAccount(_request.AccountId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _accountResponse);

            _mockCommitmentsApiClient.Setup(t => t.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
              .ReturnsAsync(() => {
                  _courses = _allTrainingProgrammeStandardsResponse.TrainingProgrammes;
                  return _allTrainingProgrammeStandardsResponse; 
              });

            _mockCommitmentsApiClient.Setup(t => t.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
              .ReturnsAsync(() => {
                  _courses = _allTrainingProgrammeResponse.TrainingProgrammes;
                  return _allTrainingProgrammeResponse;
              });

            _mockAcademicYearDateProvider = new Mock<IAcademicYearDateProvider>();

            _mockCurrentDateTimeProvider = new Mock<ICurrentDateTime>();

            _mockEncodingService = new Mock<IEncodingService>();
            _mockEncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.PublicAccountLegalEntityId))
                .Returns("ALEID");

            _mapper = new EditApprenticeshipRequestToViewModelMapper(_mockCommitmentsApiClient.Object, _mockAcademicYearDateProvider.Object, _mockCurrentDateTimeProvider.Object, _mockEncodingService.Object, _mockApprovalsOuterApiClient.Object);
        }

        internal void VerifyProviderIdIsMapped()
        {
            Assert.AreEqual(ApprenticeshipResponse.ProviderId, _viewModel.ProviderId);
        }

        internal void VerifyAccountLegalEntityIdIsMapped()
        {
            Assert.AreEqual("ALEID", _viewModel.AccountLegalEntityHashedId);
        }

        internal void VerifyHasMultipleDeliveryModelsIsMapped()
        {
            Assert.AreEqual(_getEditApprenticeshipResponse.HasMultipleDeliveryModelOptions, _viewModel.HasMultipleDeliveryModelOptions);
        }
    }
}