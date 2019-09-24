using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class DetailsViewModelMapperTests
    {
        private DetailsViewModelMapper _mapper;
        private DetailsRequest _source;
        private DetailsViewModel _result;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private Mock<ITrainingProgrammeApiClient> _trainingProgrammeApiClient;
        private Mock<IEncodingService> _encodingService;
        private GetCohortResponse _cohort;
        private GetDraftApprenticeshipsResponse _draftApprenticeshipsResponse;
        private string _apprenticeshipHashedId;
        private ITrainingProgramme _trainingProgramme;
        private List<FundingPeriod> _fundingPeriods;
        private DateTime _startFundingPeriod = new DateTime(2019, 10, 1);
        private DateTime _endFundingPeriod = new DateTime(2019, 10, 30);
        private DateTime _defaultStartDate = new DateTime(2019, 10, 1);

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _cohort = autoFixture.Create<GetCohortResponse>();
            var draftApprenticeships = CreateDraftApprenticeshipDtos(autoFixture);
            autoFixture.Register(() => draftApprenticeships);
            _draftApprenticeshipsResponse = autoFixture.Create<GetDraftApprenticeshipsResponse>();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_cohort);

            _commitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_draftApprenticeshipsResponse);

            _fundingPeriods = new List<FundingPeriod>
            {
                new FundingPeriod{ EffectiveFrom = _startFundingPeriod, EffectiveTo = _endFundingPeriod, FundingCap = 1000},
                new FundingPeriod{ EffectiveFrom = _startFundingPeriod.AddMonths(1), EffectiveTo = _endFundingPeriod.AddMonths(1), FundingCap = 500}
            };
            _trainingProgramme = new Standard { EffectiveFrom = _defaultStartDate, EffectiveTo = _defaultStartDate.AddYears(1), FundingPeriods = _fundingPeriods };

            _trainingProgrammeApiClient = new Mock<ITrainingProgrammeApiClient>();
            _trainingProgrammeApiClient.Setup(x => x.GetTrainingProgramme(It.IsAny<string>()))
                .ReturnsAsync(_trainingProgramme);

            _apprenticeshipHashedId = autoFixture.Create<string>();
            _encodingService = new Mock<IEncodingService>();
            _encodingService.Setup(x => x.Encode(It.IsAny<long>(),
                    It.Is<EncodingType>(et => et == EncodingType.ApprenticeshipId)))
                .Returns(_apprenticeshipHashedId);

            _mapper = new DetailsViewModelMapper(_commitmentsApiClient.Object, _encodingService.Object, _trainingProgrammeApiClient.Object);
            _source = autoFixture.Create<DetailsRequest>();
            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountHashedId, _result.AccountHashedId);
        }

        [Test]
        public void WithPartyIsMappedCorrectly()
        {
            Assert.AreEqual(_cohort.WithParty, _result.WithParty);
        }

        [Test]
        public void LegalEntityNameIsMappedCorrectly()
        {
            Assert.AreEqual(_cohort.LegalEntityName,_result.LegalEntityName);
        }

        [Test]
        public void ProviderNameIsMappedCorrectly()
        {
            Assert.AreEqual(_cohort.ProviderName, _result.ProviderName);
        }

        [Test]
        public void MessageIsMappedCorrectly()
        {
            Assert.AreEqual(_cohort.LatestMessageCreatedByProvider, _result.Message);
        }

        [Test]
        public void CohortReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CohortReference, _result.CohortReference);
        }

        [Test]
        public void DraftApprenticeshipTotalCountIsReportedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeshipsResponse.DraftApprenticeships.Count, _result.DraftApprenticeshipsCount);
        }

        [Test]
        public void DraftApprenticeshipCourseCountIsReportedCorrectly()
        {
            foreach (var course in _result.Courses)
            {
                var expectedCount = _draftApprenticeshipsResponse.DraftApprenticeships
                                        .Count(a =>a.CourseCode == course.CourseCode && a.CourseName == course.CourseName);

                Assert.AreEqual(expectedCount, course.Count);
            }
        }

        [Test]
        public void DraftApprenticeshipCourseOrderIsByCourseName()
        {
            var expectedSequence = _draftApprenticeshipsResponse.DraftApprenticeships
                .Select(c => new { c.CourseName, c.CourseCode })
                .Distinct()
                .OrderBy(c => c.CourseName).ThenBy(c => c.CourseCode)
                .ToList();

            var actualSequence = _result.Courses
                .Select(c => new { c.CourseName, c.CourseCode })
                .OrderBy(c => c.CourseName).ThenBy(c => c.CourseCode)
                .ToList();
                
            AssertSequenceOrder(expectedSequence, actualSequence, (e,a) => e.CourseName == a.CourseName && e.CourseCode == a.CourseCode);
        }

        [Test]
        public void DraftApprenticesOrderIsByApprenticeName()
        {
            foreach (var course in _result.Courses)
            {
                var expectedSequence = _draftApprenticeshipsResponse.DraftApprenticeships
                    .Where(a => a.CourseName == course.CourseName && a.CourseCode == course.CourseCode)
                    .Select(a => $"{a.FirstName} {a.LastName}")
                    .OrderBy(a => a)
                    .ToList();

                var actualSequence = course.DraftApprenticeships.Select(a => a.DisplayName).ToList();

                AssertSequenceOrder(expectedSequence, actualSequence, (e,a) => e == a);
            }
        }

        [Test]
        public void DraftApprenticeshipsAreMappedCorrectly()
        {
            foreach (var draftApprenticeship in _draftApprenticeshipsResponse.DraftApprenticeships)
            {
                var draftApprenticeshipResult =
                    _result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

                AssertEquality(draftApprenticeship, draftApprenticeshipResult);
            }
        }

        [Test]
        public void FundingBandCapsAreMappedCorrectlyForCoursesStartingOnDefaultdate()
        {
            foreach (var draftApprenticeship in _draftApprenticeshipsResponse.DraftApprenticeships.Where(x=>x.StartDate == _defaultStartDate))
            {
                var draftApprenticeshipResult =
                    _result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

                Assert.AreEqual(1000, draftApprenticeshipResult.FundingBandCap);
            }
        }

        [Test]
        public void FundingBandCapsAreNullForCoursesStarting2MonthsAhead()
        {
            foreach (var draftApprenticeship in _draftApprenticeshipsResponse.DraftApprenticeships.Where(x => x.StartDate == _defaultStartDate.AddMonths(2)))
            {
                var draftApprenticeshipResult =
                    _result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

                Assert.AreEqual(null, draftApprenticeshipResult.FundingBandCap);
            }
        }

        [Test]
        public void FundingBandExcessModelShowsTwoApprenticeshipsExceedingTheBandForCourse1()
        {
            var excessModel = _result.Courses.FirstOrDefault(x => x.CourseCode == "C1").FundingBandExcess;
            Assert.AreEqual(2, excessModel.ApprenticesExceedingBand);
        }

        [Test]
        public void FundingBandExcessModelHasNullOnSingleFundingCapExceeded()
        {
            var excessModel = _result.Courses.FirstOrDefault(x => x.CourseCode == "C1").FundingBandExcess;
            Assert.IsNull(excessModel.SingleFundingBandCapExceeded);
        }

        [Test]
        public void FundingBandExcessModelShowsOneApprenticeshipExceedingTheBandForCourse2()
        {
            var excessModel = _result.Courses.FirstOrDefault(x => x.CourseCode == "C2").FundingBandExcess;
            Assert.AreEqual(1, excessModel.ApprenticesExceedingBand);
        }

        [Test]
        public void FundingBandExcessModelShowsTheSingleFundingBandCapExceeded()
        {
            var excessModel = _result.Courses.FirstOrDefault(x => x.CourseCode == "C2").FundingBandExcess;
            Assert.AreEqual(1000, excessModel.SingleFundingBandCapExceeded);
        }

        [Test]
        public void FundingBandExcessModelIsNullForCourse3()
        {
            var excessModel = _result.Courses.FirstOrDefault(x => x.CourseCode == "C3").FundingBandExcess;
            Assert.IsNull(excessModel);
        }

        private void AssertEquality(DraftApprenticeshipDto source, CohortDraftApprenticeshipViewModel result)
        {
            Assert.AreEqual(source.Id, result.Id);
            Assert.AreEqual(source.FirstName, result.FirstName);
            Assert.AreEqual(source.LastName, result.LastName);
            Assert.AreEqual(source.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(source.Cost, result.Cost);
            Assert.AreEqual(source.StartDate, result.StartDate);
            Assert.AreEqual(source.EndDate, result.EndDate);
            Assert.AreEqual(_apprenticeshipHashedId, result.DraftApprenticeshipHashedId);
        }

        private IReadOnlyCollection<DraftApprenticeshipDto> CreateDraftApprenticeshipDtos(Fixture autoFixture)
        {
            var draftApprenticeships = autoFixture.CreateMany<DraftApprenticeshipDto>(6).ToArray();
            SetCourseDetails(draftApprenticeships[0], "Course1", "C1", 1000);
            SetCourseDetails(draftApprenticeships[1], "Course1", "C1", 2100);
            SetCourseDetails(draftApprenticeships[2], "Course1", "C1", 2000, _defaultStartDate.AddMonths(1));

            SetCourseDetails(draftApprenticeships[3], "Course2", "C2", 1500);
            SetCourseDetails(draftApprenticeships[4], "Course2", "C2", null);

            SetCourseDetails(draftApprenticeships[5], "Course3", "C3", null, _defaultStartDate.AddMonths(2));

            return draftApprenticeships;
        }

        private void SetCourseDetails(DraftApprenticeshipDto draftApprenticeship, string courseName, string courseCode, int? cost, DateTime? startDate = null)
        {
            startDate = startDate ?? _defaultStartDate;

            draftApprenticeship.CourseName = courseName;
            draftApprenticeship.CourseCode = courseCode;
            draftApprenticeship.Cost = cost;
            draftApprenticeship.StartDate = startDate;
        }

        public void AssertSequenceOrder<T>(List<T> expected, List<T> actual, Func<T, T, bool> evaluator)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Expected and actual sequences are different lengths");

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.IsTrue(evaluator(expected[i], actual[i]), "Actual sequence are not in same order as expected");
            }
        }
    }
}
