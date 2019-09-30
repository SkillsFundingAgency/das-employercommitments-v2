using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DetailsViewModelMapperTests
    {
        [Test]
        public async Task AccountHashedIdIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Source.AccountHashedId, result.AccountHashedId);
        }

        [Test]
        public async Task WithPartyIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.WithParty, result.WithParty);
        }

        [Test]
        public async Task LegalEntityNameIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.LegalEntityName, result.LegalEntityName);
        }

        [Test]
        public async Task ProviderNameIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.ProviderName, result.ProviderName);
        }

        [Test]
        public async Task MessageIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.LatestMessageCreatedByProvider, result.Message);
        }

        [Test]
        public async Task CohortReferenceIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Source.CohortReference, result.CohortReference);
        }

        [Test]
        public async Task TransferSenderHashedIdIsEncodedCorrectlyWhenThereIsAValue()
        {
            var fixture = new DetailsViewModelMapperTestsFixture().SetTransferSenderIdAndItsExpectedHashedValue(123, "X123X");
            var result = await fixture.Map();
            Assert.AreEqual("X123X", result.TransferSenderHashedId);
        }

        [Test]
        public async Task TransferSenderHashedIdIsNullWhenThereIsNoValue()
        {
            var fixture = new DetailsViewModelMapperTestsFixture().SetTransferSenderIdAndItsExpectedHashedValue(null, null);
            var result = await fixture.Map();
            Assert.IsNull(result.TransferSenderHashedId);
        }

        [Test]
        public async Task DraftApprenticeshipTotalCountIsReportedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.DraftApprenticeshipsResponse.DraftApprenticeships.Count, result.DraftApprenticeshipsCount);
        }

        [Test]
        public async Task DraftApprenticeshipCourseCountIsReportedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            foreach (var course in result.Courses)
            {
                var expectedCount = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                    .Count(a => a.CourseCode == course.CourseCode && a.CourseName == course.CourseName);

                Assert.AreEqual(expectedCount, course.Count);
            }
        }

        [Test]
        public async Task DraftApprenticeshipCourseOrderIsByCourseName()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            var expectedSequence = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                .Select(c => new { c.CourseName, c.CourseCode })
                .Distinct()
                .OrderBy(c => c.CourseName).ThenBy(c => c.CourseCode)
                .ToList();

            var actualSequence = result.Courses
                .Select(c => new { c.CourseName, c.CourseCode })
                .OrderBy(c => c.CourseName).ThenBy(c => c.CourseCode)
                .ToList();

            fixture.AssertSequenceOrder(expectedSequence, actualSequence, (e, a) => e.CourseName == a.CourseName && e.CourseCode == a.CourseCode);
        }

        [Test]
        public async Task DraftApprenticesOrderIsByApprenticeName()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            foreach (var course in result.Courses)
            {
                var expectedSequence = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                    .Where(a => a.CourseName == course.CourseName && a.CourseCode == course.CourseCode)
                    .Select(a => $"{a.FirstName} {a.LastName}")
                    .OrderBy(a => a)
                    .ToList();

                var actualSequence = course.DraftApprenticeships.Select(a => a.DisplayName).ToList();

                fixture.AssertSequenceOrder(expectedSequence, actualSequence, (e, a) => e == a);
            }
        }

        [Test]
        public async Task DraftApprenticeshipsAreMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();

            foreach (var draftApprenticeship in fixture.DraftApprenticeshipsResponse.DraftApprenticeships)
            {
                var draftApprenticeshipResult =
                    result.Courses.SelectMany(c => c.DraftApprenticeships).Single(x => x.Id == draftApprenticeship.Id);

                fixture.AssertEquality(draftApprenticeship, draftApprenticeshipResult);
            }
        }

        [TestCase(0, "Approve 0 apprentices' details")]
        [TestCase(1, "Approve apprentice details")]
        [TestCase(2, "Approve 2 apprentices' details")]
        public async Task PageTitleIsSetCorrectlyForTheNumberOfApprenticeships(int numberOfApprenticeships, string expectedPageTitle)
        {
            var fixture = new DetailsViewModelMapperTestsFixture().CreateThisNumberOfApprenticeships(numberOfApprenticeships);
            var result = await fixture.Map();

            Assert.AreEqual(expectedPageTitle, result.PageTitle);
        }
    }

    public class DetailsViewModelMapperTestsFixture
    {
        public DetailsViewModelMapper Mapper;
        public DetailsRequest Source;
        public DetailsViewModel Result;
        public Mock<ICommitmentsApiClient> CommitmentsApiClient;
        public Mock<IEncodingService> EncodingService;
        public GetCohortResponse Cohort;
        public GetDraftApprenticeshipsResponse DraftApprenticeshipsResponse;
        private Fixture _autoFixture;

        public DetailsViewModelMapperTestsFixture()
        {
            _autoFixture = new Fixture();

            Cohort = _autoFixture.Create<GetCohortResponse>();

            var draftApprenticeships = CreateDraftApprenticeshipDtos(_autoFixture);
            _autoFixture.Register(() => draftApprenticeships);
            DraftApprenticeshipsResponse = _autoFixture.Create<GetDraftApprenticeshipsResponse>();

            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Cohort);

            CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(DraftApprenticeshipsResponse);

            EncodingService = new Mock<IEncodingService>();
            SetEncodingOfApprenticeIds();

            Mapper = new DetailsViewModelMapper(CommitmentsApiClient.Object, EncodingService.Object);
            Source = _autoFixture.Create<DetailsRequest>();
        }

        public DetailsViewModelMapperTestsFixture SetCohortWithParty(Party party)
        {
            Cohort.WithParty = party;
            return this;
        }

        public DetailsViewModelMapperTestsFixture SetTransferSenderIdAndItsExpectedHashedValue(long? transferSenderId, string expectedHashedId)
        {
            Cohort.TransferSenderId = transferSenderId;
            if (transferSenderId.HasValue)
            {
                EncodingService.Setup(x => x.Encode(transferSenderId.Value, EncodingType.PublicAccountId))
                    .Returns(expectedHashedId);
            }

            return this;
        }

        public DetailsViewModelMapperTestsFixture SetEncodingOfApprenticeIds()
        {
            EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.ApprenticeshipId))
                .Returns((long p, EncodingType t) => $"X{p}X" );

            return this;
        }

        public DetailsViewModelMapperTestsFixture CreateThisNumberOfApprenticeships(int numberOfApprenticeships)
        {
            var draftApprenticeships = _autoFixture.CreateMany<DraftApprenticeshipDto>(numberOfApprenticeships).ToArray();
            DraftApprenticeshipsResponse.DraftApprenticeships = draftApprenticeships;
            return this;
        }

        public Task<DetailsViewModel> Map()
        {
            return Mapper.Map(TestHelper.Clone(Source));
        }

        public void AssertEquality(DraftApprenticeshipDto source, CohortDraftApprenticeshipViewModel result)
        {
            Assert.AreEqual(source.Id, result.Id);
            Assert.AreEqual(source.FirstName, result.FirstName);
            Assert.AreEqual(source.LastName, result.LastName);
            Assert.AreEqual(source.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(source.Cost, result.Cost);
            Assert.AreEqual(source.StartDate, result.StartDate);
            Assert.AreEqual(source.EndDate, result.EndDate);
            Assert.AreEqual($"X{source.Id}X", result.DraftApprenticeshipHashedId);
        }

        public void AssertSequenceOrder<T>(List<T> expected, List<T> actual, Func<T, T, bool> evaluator)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Expected and actual sequences are different lengths");

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.IsTrue(evaluator(expected[i], actual[i]), "Actual sequence are not in same order as expected");
            }
        }

        private IReadOnlyCollection<DraftApprenticeshipDto> CreateDraftApprenticeshipDtos(Fixture autoFixture)
        {
            var draftApprenticeships = autoFixture.CreateMany<DraftApprenticeshipDto>(6).ToArray();
            SetCourseDetails(draftApprenticeships[0], "Course1", "C1");
            SetCourseDetails(draftApprenticeships[1], "Course1", "C1");
            SetCourseDetails(draftApprenticeships[2], "Course1", "C1");

            SetCourseDetails(draftApprenticeships[3], "Course2", "C2");
            SetCourseDetails(draftApprenticeships[4], "Course2", "C2");

            SetCourseDetails(draftApprenticeships[5], "Course3", "C3");

            return draftApprenticeships;
        }

        private void SetCourseDetails(DraftApprenticeshipDto draftApprenticeship, string courseName, string courseCode)
        {
            draftApprenticeship.CourseName = courseName;
            draftApprenticeship.CourseCode = courseCode;
        }
    }
}