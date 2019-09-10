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
    public class DetailsViewModelMapperTests
    {
        private DetailsViewModelMapper _mapper;
        private DetailsRequest _source;
        private DetailsViewModel _result;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private GetCohortResponse _cohort;
        private IReadOnlyCollection<DraftApprenticeshipDto> _draftApprenticeships;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _cohort = autoFixture.Create<GetCohortResponse>();
            _draftApprenticeships = autoFixture.Create<List<DraftApprenticeshipDto>>();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_cohort);

            _commitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_draftApprenticeships);

            _mapper = new DetailsViewModelMapper(_commitmentsApiClient.Object);
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
        public void DraftApprenticeshipsAreMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeships.Count, _result.DraftApprenticeships.Count);

            foreach (var draftApprenticeship in _draftApprenticeships)
            {
                var draftApprenticeshipResult =
                    _result.DraftApprenticeships.Single(x => x.Id == draftApprenticeship.Id);

                AssertEquality(draftApprenticeship, draftApprenticeshipResult);
            }
        }

        private void AssertEquality(DraftApprenticeshipDto source, CohortDraftApprenticeshipViewModel result)
        {
            Assert.AreEqual(source.Id, result.Id);
            Assert.AreEqual(source.FirstName, result.FirstName);
            Assert.AreEqual(source.LastName, result.LastName);
            Assert.AreEqual(source.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(source.CourseCode, result.CourseCode);
            Assert.AreEqual(source.CourseName, result.CourseName);
            Assert.AreEqual(source.Cost, result.Cost);
            Assert.AreEqual(source.StartDate, result.StartDate);
            Assert.AreEqual(source.EndDate, result.EndDate);
        }
    }
}
