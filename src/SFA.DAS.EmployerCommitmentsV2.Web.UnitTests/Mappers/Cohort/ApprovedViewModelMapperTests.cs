using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ApprovedViewModelMapperTests
    {
        [Test]
        public async Task AccountHashedIdIsMappedCorrectly()
        {
            var fixture = new ApprovedViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.AccountHashedId, Is.EqualTo(fixture.Source.AccountHashedId));
        }

        [Test]
        public async Task WithPartyIsMappedCorrectly()
        {
            var fixture = new ApprovedViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.WithParty, Is.EqualTo(fixture.Cohort.WithParty));
        }

        [Test]
        public async Task LegalEntityNameIsMappedCorrectly()
        {
            var fixture = new ApprovedViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.LegalEntityName, Is.EqualTo(fixture.Cohort.LegalEntityName));
        }

        [Test]
        public async Task ProviderNameIsMappedCorrectly()
        {
            var fixture = new ApprovedViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.ProviderName, Is.EqualTo(fixture.Cohort.ProviderName));
        }

        [Test]
        public async Task MessageIsMappedCorrectly()
        {
            var fixture = new ApprovedViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.Message, Is.EqualTo(fixture.Cohort.LatestMessageCreatedByEmployer));
        }

        [Test]
        public async Task CohortReferenceIsMappedCorrectly()
        {
            var fixture = new ApprovedViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.That(result.CohortReference, Is.EqualTo(fixture.Source.CohortReference));
        }
    }

    public class ApprovedViewModelMapperTestsFixture
    {
        public ApprovedViewModelMapper Mapper;
        public ApprovedRequest Source;
        public ApprovedViewModel Result;
        public Mock<ICommitmentsApiClient> CommitmentsApiClient;
        public Mock<IEncodingService> EncodingService;
        public GetCohortResponse Cohort;
        public GetDraftApprenticeshipsResponse DraftApprenticeshipsResponse;
        private Fixture _autoFixture;

        public ApprovedViewModelMapperTestsFixture()
        {
            _autoFixture = new Fixture();

            Cohort = _autoFixture.Create<GetCohortResponse>();

            DraftApprenticeshipsResponse = _autoFixture.Create<GetDraftApprenticeshipsResponse>();

            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Cohort);

            CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(DraftApprenticeshipsResponse);

            EncodingService = new Mock<IEncodingService>();

            Mapper = new ApprovedViewModelMapper(CommitmentsApiClient.Object, EncodingService.Object);
            Source = _autoFixture.Create<ApprovedRequest>();
        }

        public ApprovedViewModelMapperTestsFixture SetCohortWithParty(Party party)
        {
            Cohort.WithParty = party;
            return this;
        }

        public Task<ApprovedViewModel> Map()
        {
            return Mapper.Map(TestHelper.Clone(Source));
        }
    }
}