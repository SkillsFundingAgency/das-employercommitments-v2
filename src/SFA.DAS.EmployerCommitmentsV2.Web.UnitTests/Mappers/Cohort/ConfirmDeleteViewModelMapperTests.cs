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
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ConfirmDeleteViewModelMapperTests
    {

        [Test]
        public async Task AccountHashedIdIsMappedCorrectly()
        {
            var fixture = new ConfirmDeleteViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.AccountHashedId, Is.EqualTo(fixture.Source.AccountHashedId));
        }

        [Test]
        public async Task CohortReferenceIsMappedCorrectly()
        {
            var fixture = new ConfirmDeleteViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.CohortReference, Is.EqualTo(fixture.Source.CohortReference));
        }

        [Test]
        public async Task ProviderNameIsMappedCorrectly()
        {
            var fixture = new ConfirmDeleteViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.ProviderName, Is.EqualTo(fixture.Cohort.ProviderName));
        }

        [Test]
        public async Task LegalEntityNameIsMappedCorrectly()
        {
            var fixture = new ConfirmDeleteViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.LegalEntityName, Is.EqualTo(fixture.Cohort.LegalEntityName));
        }

        [Test]
        public async Task DraftApprenticeshipTotalCountIsReportedCorrectly()
        {
            var fixture = new ConfirmDeleteViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.DraftApprenticeshipsCount, Is.EqualTo(fixture.DraftApprenticeshipsResponse.DraftApprenticeships.Count));
        }

        [Test]
        public async Task DraftApprenticeshipCourseCountOfApprenticeshipsIsMappedCorrectly()
        {
            var fixture = new ConfirmDeleteViewModelMapperTestsFixture();
            var result = await fixture.Map();

            foreach (var course in result.Courses)
            {
                var expectedCount = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                    .Count(a => a.CourseName == course.CourseName);

                Assert.That(course.NumberOfDraftApprenticeships, Is.EqualTo(expectedCount));
            }
        }

        [Test]
        public async Task DraftApprenticeshipCourseOrderIsByCourseName()
        {
            var fixture = new ConfirmDeleteViewModelMapperTestsFixture();
            var result = await fixture.Map();

            var expectedSequence = fixture.DraftApprenticeshipsResponse.DraftApprenticeships
                .Select(c => c.CourseName )
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            var actualSequence = result.Courses
                .Select(c => c.CourseName)
                .ToList();

            fixture.AssertSequenceOrder(expectedSequence, actualSequence, (e, a) => e == a);
        }
    }

    public class ConfirmDeleteViewModelMapperTestsFixture
    {
        public ConfirmDeleteViewModelMapper Sut;
        public DetailsRequest Source;
        public ConfirmDeleteViewModel Result;
        public Mock<ICommitmentsApiClient> CommitmentsApiClient;
        public GetCohortResponse Cohort;
        public GetDraftApprenticeshipsResponse DraftApprenticeshipsResponse;

        private Fixture _autoFixture;

        public ConfirmDeleteViewModelMapperTestsFixture()
        {
            _autoFixture = new Fixture();

            Cohort = _autoFixture.Build<GetCohortResponse>().Create();

            var draftApprenticeships = CreateDraftApprenticeshipDtos(_autoFixture);
            _autoFixture.Register(() => draftApprenticeships);
            DraftApprenticeshipsResponse = _autoFixture.Create<GetDraftApprenticeshipsResponse>();

            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Cohort);
            CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(DraftApprenticeshipsResponse);

            Sut = new ConfirmDeleteViewModelMapper(CommitmentsApiClient.Object);
            Source = _autoFixture.Create<DetailsRequest>();
        }


        public ConfirmDeleteViewModelMapperTestsFixture CreateThisNumberOfApprenticeships(int numberOfApprenticeships)
        {
            var draftApprenticeships = _autoFixture.CreateMany<DraftApprenticeshipDto>(numberOfApprenticeships).ToArray();
            DraftApprenticeshipsResponse.DraftApprenticeships = draftApprenticeships;
            return this;
        }

        public Task<ConfirmDeleteViewModel> Map()
        {
            return Sut.Map(TestHelper.Clone(Source));
        }

        public void AssertEquality(DraftApprenticeshipDto source, CohortDraftApprenticeshipViewModel result)
        {
            Assert.That(result.Id, Is.EqualTo(source.Id));
            Assert.That(result.FirstName, Is.EqualTo(source.FirstName));
            Assert.That(result.LastName, Is.EqualTo(source.LastName));
            Assert.That(result.DateOfBirth, Is.EqualTo(source.DateOfBirth));
            Assert.That(result.Cost, Is.EqualTo(source.Cost));
            Assert.That(result.StartDate, Is.EqualTo(source.StartDate));
            Assert.That(result.EndDate, Is.EqualTo(source.EndDate));
            Assert.That(result.DraftApprenticeshipHashedId, Is.EqualTo($"X{source.Id}X"));
        }

        public void AssertSequenceOrder<T>(List<T> expected, List<T> actual, Func<T, T, bool> evaluator)
        {
            Assert.That(actual.Count, Is.EqualTo(expected.Count), "Expected and actual sequences are different lengths");

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.That(evaluator(expected[i], actual[i]), Is.True, "Actual sequence are not in same order as expected");
            }
        }

        private IReadOnlyCollection<DraftApprenticeshipDto> CreateDraftApprenticeshipDtos(Fixture autoFixture)
        {
            var draftApprenticeships = autoFixture.CreateMany<DraftApprenticeshipDto>(6).ToArray();
            SetCourseDetails(draftApprenticeships[0], "Course1", "C1", 1000);
            SetCourseDetails(draftApprenticeships[1], "Course1", "C1", 2100);
            SetCourseDetails(draftApprenticeships[2], "Course1", "C1", 2000);

            SetCourseDetails(draftApprenticeships[3], "Course2", "C2", 1500);
            SetCourseDetails(draftApprenticeships[4], "Course2", "C2", null);

            SetCourseDetails(draftApprenticeships[5], "Course3", "C3", null);

            return draftApprenticeships;
        }

        private void SetCourseDetails(DraftApprenticeshipDto draftApprenticeship, string courseName, string courseCode, int? cost)
        {
            draftApprenticeship.CourseName = courseName;
            draftApprenticeship.CourseCode = courseCode;
            draftApprenticeship.Cost = cost;
            draftApprenticeship.StartDate = new DateTime(2019, 10, 1);
        }
    }
}