using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Types;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;
using SFA.DAS.Encoding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types.Dtos;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTests
    {
        ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTestsFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTestsFixture();
        }

        [Test]
        public async Task CommitmentApiToGetApprenticeshipIsCalled()
        {
           await fixture.Map();

            fixture.VerifyCommitmentApiIsCalled();
        }

        [Test]
        public async Task CommitmentApiToGetPriceEpisodeIsCalled()
        {
            await fixture.Map();

            fixture.VerifyPriceEpisodeIsCalled();
        }

        [Test]
        public async Task WhenOnlyEmployerReferenceIsChanged()
        {
            fixture.source.EmployerReference = "EmployerRef";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.EmployerReference, fixture.ApprenticeshipResponse.EmployerReference);
            Assert.AreEqual(fixture.source.EmployerReference, result.EmployerReference);
            Assert.AreEqual(fixture.ApprenticeshipResponse.EmployerReference, result.OriginalApprenticeship.EmployerReference);
        }

        [Test]
        public async Task WhenFirstNameIsChanged()
        {
            fixture.source.FirstName = "FirstName";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.FirstName, fixture.ApprenticeshipResponse.FirstName);
            Assert.AreEqual(fixture.source.FirstName, result.FirstName);
            Assert.AreEqual(fixture.ApprenticeshipResponse.FirstName, result.OriginalApprenticeship.FirstName);
        }

        [Test]
        public async Task WhenLastNameIsChanged()
        {
            fixture.source.LastName = "LastName";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.LastName, fixture.ApprenticeshipResponse.LastName);
            Assert.AreEqual(fixture.source.LastName, result.LastName);
            Assert.AreEqual(fixture.ApprenticeshipResponse.LastName, result.OriginalApprenticeship.LastName);
        }

        [Test]
        public async Task WhenEmailIsChanged()
        {
            fixture.source.Email = "Email";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.Email, fixture.ApprenticeshipResponse.Email);
            Assert.AreEqual(fixture.source.Email, result.Email);
            Assert.AreEqual(fixture.ApprenticeshipResponse.Email, result.OriginalApprenticeship.Email);
        }

        [Test]
        public async Task WhenDobIsChanged()
        {
            fixture.source.DateOfBirth = new CommitmentsV2.Shared.Models.DateModel(new DateTime(2000,12,31));

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.DateOfBirth.Day, fixture.ApprenticeshipResponse.DateOfBirth.Day);
            Assert.AreEqual(fixture.source.DateOfBirth.Date, result.DateOfBirth);
            Assert.AreEqual(fixture.ApprenticeshipResponse.DateOfBirth, result.OriginalApprenticeship.DateOfBirth);
        }

        [TestCase(DeliveryModel.Normal, DeliveryModel.Flexible)]
        [TestCase(DeliveryModel.Flexible, DeliveryModel.Normal)]
        public async Task WhenDeliveryModelIsChanged(DeliveryModel original, DeliveryModel changedTo)
        {
            fixture.ApprenticeshipResponse.DeliveryModel = new DeliveryModelDto(original);
            fixture.source.DeliveryModel = changedTo;

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.DeliveryModel, fixture.ApprenticeshipResponse.DeliveryModel);
            Assert.AreEqual(fixture.source.DeliveryModel, result.DeliveryModel);
            Assert.AreEqual(fixture.ApprenticeshipResponse.DeliveryModel.Code, result.OriginalApprenticeship.DeliveryModel);
        }

        [Test]
        public async Task WhenStartDateIsChanged()
        {
            var newStartDate = fixture.ApprenticeshipResponse.StartDate.AddMonths(-1);
            fixture.source.StartDate = new CommitmentsV2.Shared.Models.MonthYearModel(newStartDate.Month.ToString() + newStartDate.Year);

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.StartDate.Date, fixture.ApprenticeshipResponse.StartDate);
            Assert.AreEqual(fixture.source.StartDate.Date, result.StartDate);
            Assert.AreEqual(fixture.ApprenticeshipResponse.StartDate, result.OriginalApprenticeship.StartDate);
        }

        [Test]
        public async Task WhenEndDateIsChanged()
        {
            var newEndDate = fixture.ApprenticeshipResponse.EndDate.AddMonths(-1);
            fixture.source.EndDate = new CommitmentsV2.Shared.Models.MonthYearModel(newEndDate.Month.ToString() + newEndDate.Year);

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.EndDate.Date, fixture.ApprenticeshipResponse.EndDate);
            Assert.AreEqual(fixture.source.EndDate.Date, result.EndDate);
            Assert.AreEqual(fixture.ApprenticeshipResponse.EndDate, result.OriginalApprenticeship.EndDate);
        }

        [Test]
        public async Task WhenCourseIsChanged()
        {
            fixture.source.CourseCode = "Abc";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.CourseCode, fixture.ApprenticeshipResponse.CourseCode);
            Assert.AreEqual(fixture.source.CourseCode, result.CourseCode);
            Assert.AreEqual(fixture.ApprenticeshipResponse.CourseCode, result.OriginalApprenticeship.CourseCode);
        }

        [Test]
        public async Task WhenVersionIsChanged()
        {
            fixture.source.Version = "1.1";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.Version, fixture.ApprenticeshipResponse.Version);
            Assert.AreEqual(fixture.source.Version, result.Version);
        }

        [Test]
        public async Task WhenCourseCodeIsChangeButVersionIsNotChanged_ThenVersionIsMapped()
        {
            fixture.source.CourseCode = "123";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.Version, fixture.ApprenticeshipResponse.Version);
            Assert.AreEqual(fixture.source.Version, result.Version);
        }

        [Test]
        public async Task WhenOptionIsChanged()
        {
            fixture.source.Option = "NewOption";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.Option, fixture.ApprenticeshipResponse.Option);
            Assert.AreEqual(fixture.source.Option, result.Option);
        }

        [Test]
        public async Task When_VersionHasOptions_Then_ReturnToChangeOptionsIsTrue()
        {
            fixture.source.HasOptions = true;

            var result = await fixture.Map();

            Assert.True(result.ReturnToChangeOption);
        }

        [Test]
        public async Task When_VersionIsChangedDirectly_Then_ReturnToChangeVersionIsTrue()
        {
            fixture.source.Version = "NewVersion";

            var result = await fixture.Map();

            Assert.True(result.ReturnToChangeVersion);
        }

        [Test]
        public async Task When_VersionIsChangedByEditCourse_Then_ReturnToChangeVersionAndOptionAreFalse()
        {
            fixture.source.Version = "NewVersion";
            fixture.source.CourseCode = "NewCourseCode";

            var result = await fixture.Map();

            Assert.False(result.ReturnToChangeVersion);
            Assert.False(result.ReturnToChangeOption);
        }

        [Test]
        public async Task When_VersionIsChangedByEditStartDate_Then_ReturnToChangeVersionAndOptionAreFalse()
        {
            fixture.source.Version = "NewVersion";
            fixture.source.StartDate = new MonthYearModel(DateTime.Now.ToString("MMyyyy"));

            var result = await fixture.Map();

            Assert.False(result.ReturnToChangeVersion);
            Assert.False(result.ReturnToChangeOption);
        }

        [Test]
        public async Task WhenMultipleFieldsAreChanged_TheyAreChanged()
        {
            fixture.source.CourseCode = "NewCourse";
            fixture.source.LastName = "NewLastName";

            var result = await fixture.Map();

            Assert.AreEqual(fixture.source.LastName, result.LastName);
            Assert.AreEqual(fixture.source.CourseCode, result.CourseCode);
        }

        [Test]
        public async Task UnchangedFieldsAreNull()
        {
            fixture.source.CourseCode = "Course";

            var result = await fixture.Map();
            Assert.IsNull(result.FirstName);
            Assert.IsNull(result.LastName);
            Assert.IsNull(result.EndMonth);
            Assert.IsNull(result.StartMonth);
            Assert.IsNull(result.BirthMonth);
        }
    }

    public class ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTestsFixture
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        public GetApprenticeshipResponse ApprenticeshipResponse;
        private GetPriceEpisodesResponse _priceEpisodeResponse;

        private ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapper _mapper;
        private TrainingProgramme _standardSummary;
        private Mock<IEncodingService> _encodingService;
        public EditApprenticeshipRequestViewModel source;
        public ConfirmEditApprenticeshipViewModel resultViewModl;

        public ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTestsFixture()
        {
            var autoFixture = new Fixture();

            ApprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                .With(x => x.CourseCode, "ABC")
                .With(x => x.Version, "1.0")
                .With(x => x.StartDate, new DateTime(2020, 1, 1))
                .With(x => x.EndDate, new DateTime(2021, 1, 1))
                .With(x => x.DateOfBirth, new DateTime(1990,1,1))
                .Create();

            source = new EditApprenticeshipRequestViewModel();
            source.ApprenticeshipId = ApprenticeshipResponse.Id;
            source.CourseCode = ApprenticeshipResponse.CourseCode;
            source.FirstName = ApprenticeshipResponse.FirstName;
            source.LastName = ApprenticeshipResponse.LastName;
            source.Email = ApprenticeshipResponse.Email;
            source.DateOfBirth = new CommitmentsV2.Shared.Models.DateModel(ApprenticeshipResponse.DateOfBirth);
            source.Cost = 1000;
            source.EmployerReference = ApprenticeshipResponse.EmployerReference;
            source.StartDate = new CommitmentsV2.Shared.Models.MonthYearModel(ApprenticeshipResponse.StartDate.Month.ToString() + ApprenticeshipResponse.StartDate.Year) ;
            source.EndDate = new CommitmentsV2.Shared.Models.MonthYearModel(ApprenticeshipResponse.EndDate.Month.ToString() + ApprenticeshipResponse.EndDate.Year);

            _priceEpisodeResponse = autoFixture.Build<GetPriceEpisodesResponse>()
                .With(x => x.PriceEpisodes, new List<PriceEpisode> {
                    new PriceEpisode { Cost = 1000, FromDate = DateTime.Now.AddMonths(-1), ToDate = null}})
                .Create();

            _standardSummary = autoFixture.Create<TrainingProgramme>();
            _standardSummary.EffectiveFrom = new DateTime(2018, 1, 1);
            _standardSummary.EffectiveTo = new DateTime(2022, 1, 1);
            _standardSummary.FundingPeriods = SetPriceBand(1000);

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ApprenticeshipResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_priceEpisodeResponse);
            _mockCommitmentsApiClient.Setup(t => t.GetTrainingProgrammeVersionByCourseCodeAndVersion(source.CourseCode, source.Version, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTrainingProgrammeResponse
                {
                    TrainingProgramme = _standardSummary
                });

            _encodingService = new Mock<IEncodingService>();
            _encodingService.Setup(x => x.Decode(It.IsAny<string>(), EncodingType.ApprenticeshipId)).Returns(ApprenticeshipResponse.Id);
            _encodingService.Setup(x => x.Decode(It.IsAny<string>(), EncodingType.AccountId)).Returns(ApprenticeshipResponse.EmployerAccountId);

            _mapper = new ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapper(_mockCommitmentsApiClient.Object, _encodingService.Object);
        }

        public List<TrainingProgrammeFundingPeriod> SetPriceBand(int fundingCap)
        {
            return new List<TrainingProgrammeFundingPeriod>
            {
                new TrainingProgrammeFundingPeriod
                {
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = DateTime.Now.AddMonths(1),
                        FundingCap = fundingCap
                }
            };
        }

        public async Task<ConfirmEditApprenticeshipViewModel> Map()
        {
          resultViewModl =  await _mapper.Map(source);
            return resultViewModl;
        }

        internal void VerifyCommitmentApiIsCalled()
        {
            _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(ApprenticeshipResponse.Id, It.IsAny<CancellationToken>()), Times.Once());
        }

        internal void VerifyPriceEpisodeIsCalled()
        {
            _mockCommitmentsApiClient.Verify(c => c.GetPriceEpisodes(ApprenticeshipResponse.Id, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
