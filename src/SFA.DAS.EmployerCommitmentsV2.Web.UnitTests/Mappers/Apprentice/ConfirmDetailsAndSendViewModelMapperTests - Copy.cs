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

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapperTests
    {
        ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapperTestsFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapperTestsFixture();
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
            fixture.source.EmployerReference = "Abc";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.EmployerReference, fixture._apprenticeshipResponse.EmployerReference);
            Assert.AreEqual(fixture.source.EmployerReference, result.EmployerReference);
            Assert.AreEqual(fixture._apprenticeshipResponse.EmployerReference, result.OriginalApprenticeship.EmployerReference);
        }

        [Test]
        public async Task WhenFirstNameIsChanged()
        {
            fixture.source.FirstName = "Abc";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.FirstName, fixture._apprenticeshipResponse.FirstName);
            Assert.AreEqual(fixture.source.FirstName, result.FirstName);
            Assert.AreEqual(fixture._apprenticeshipResponse.FirstName, result.OriginalApprenticeship.FirstName);
        }

        [Test]
        public async Task WhenLastNameIsChanged()
        {
            fixture.source.LastName = "Abc";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.LastName, fixture._apprenticeshipResponse.LastName);
            Assert.AreEqual(fixture.source.LastName, result.LastName);
            Assert.AreEqual(fixture._apprenticeshipResponse.LastName, result.OriginalApprenticeship.LastName);
        }

        [Test]
        public async Task WhenDobIsChanged()
        {
            fixture.source.DateOfBirth = new CommitmentsV2.Shared.Models.DateModel(new DateTime(2000,12,31));

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.DateOfBirth.Day, fixture._apprenticeshipResponse.DateOfBirth.Day);
            Assert.AreEqual(fixture.source.DateOfBirth.Date, result.DateOfBirth);
            Assert.AreEqual(fixture._apprenticeshipResponse.DateOfBirth, result.OriginalApprenticeship.DateOfBirth);
        }

        [Test]
        public async Task WhenStartDateIsChanged()
        {
            var newStartDate = fixture._apprenticeshipResponse.StartDate.AddMonths(-1);
            fixture.source.StartDate = new CommitmentsV2.Shared.Models.MonthYearModel(newStartDate.Month.ToString() + newStartDate.Year);

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.StartDate.Date, fixture._apprenticeshipResponse.StartDate);
            Assert.AreEqual(fixture.source.StartDate.Date, result.StartDate);
            Assert.AreEqual(fixture._apprenticeshipResponse.StartDate, result.OriginalApprenticeship.StartDate);
        }

        [Test]
        public async Task WhenEndDateIsChanged()
        {
            var newEndDate = fixture._apprenticeshipResponse.EndDate.AddMonths(-1);
            fixture.source.EndDate = new CommitmentsV2.Shared.Models.MonthYearModel(newEndDate.Month.ToString() + newEndDate.Year);

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.EndDate.Date, fixture._apprenticeshipResponse.EndDate);
            Assert.AreEqual(fixture.source.EndDate.Date, result.EndDate);
            Assert.AreEqual(fixture._apprenticeshipResponse.EndDate, result.OriginalApprenticeship.EndDate);
        }

        [Test]
        public async Task WhenCourseIsChanged()
        {
            fixture.source.CourseCode = "Abc";

            var result = await fixture.Map();

            Assert.AreNotEqual(fixture.source.CourseCode, fixture._apprenticeshipResponse.CourseCode);
            Assert.AreEqual(fixture.source.CourseCode, result.CourseCode);
            Assert.AreEqual(fixture._apprenticeshipResponse.CourseCode, result.OriginalApprenticeship.CourseCode);
        }

        [Test]
        public async Task WhenMultipleAreChangedIsChanged()
        {
            fixture.source.CourseCode = "NewCourse";
            fixture.source.LastName = "NewLastName";

            var result = await fixture.Map();

            Assert.AreEqual(fixture.source.LastName, result.LastName);
            Assert.AreEqual(fixture.source.CourseCode, result.CourseCode);
        }


        [Test]
        public async Task NotChangedFieldsAreNull()
        {
            fixture.source.CourseCode = "Course";

            var result = await fixture.Map();
            Assert.IsNull(result.FirstName);
            Assert.IsNull(result.LastName);
            Assert.IsNull(result.EndMonth);
            Assert.IsNull(result.StartMonth);
            Assert.IsNull(result.EmployerReference);
            Assert.IsNull(result.BirthMonth);
        }
    }

    public class ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapperTestsFixture
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        public GetApprenticeshipResponse _apprenticeshipResponse;
        private GetPriceEpisodesResponse _priceEpisodeResponse;

        private ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapper _mapper;
        private TrainingProgramme _standardSummary;
        private Mock<IEncodingService> _encodingService;
        public EditApprenticeshipRequestViewModel source;
        public ConfirmEditApprenticeshipViewModel resultViewModl;

        public ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapperTestsFixture()
        {
            var autoFixture = new Fixture();

            _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
                .With(x => x.CourseCode, "ABC")
                .With(x => x.StartDate, new DateTime(2020, 1, 1))
                .With(x => x.EndDate, new DateTime(2021, 1, 1))
                .With(x => x.DateOfBirth, new DateTime(1990,1,1))
                .Create();

            source = new EditApprenticeshipRequestViewModel();
            source.ApprenticeshipId = _apprenticeshipResponse.Id;
            source.CourseCode = _apprenticeshipResponse.CourseCode;
            source.FirstName = _apprenticeshipResponse.FirstName;
            source.LastName = _apprenticeshipResponse.LastName;
            source.DateOfBirth = new CommitmentsV2.Shared.Models.DateModel(_apprenticeshipResponse.DateOfBirth);
            source.Cost = 1000;
            source.EmployerReference = _apprenticeshipResponse.EmployerReference;
            source.StartDate = new CommitmentsV2.Shared.Models.MonthYearModel(_apprenticeshipResponse.StartDate.Month.ToString() + _apprenticeshipResponse.StartDate.Year) ;
            source.EndDate = new CommitmentsV2.Shared.Models.MonthYearModel(_apprenticeshipResponse.EndDate.Month.ToString() + _apprenticeshipResponse.EndDate.Year);

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
                .ReturnsAsync(_apprenticeshipResponse);
            _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_priceEpisodeResponse);
            _mockCommitmentsApiClient.Setup(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTrainingProgrammeResponse
                {
                    TrainingProgramme = _standardSummary
                });

            _encodingService = new Mock<IEncodingService>();
            _encodingService.Setup(x => x.Decode(It.IsAny<string>(), It.IsAny<EncodingType>())).Returns(22);
            _mapper = new ConfirmEditApprenticehsipRequestToConfirmEditViewModelMapper(_mockCommitmentsApiClient.Object, _encodingService.Object);
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
            _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        internal void VerifyPriceEpisodeIsCalled()
        {
            _mockCommitmentsApiClient.Verify(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}

