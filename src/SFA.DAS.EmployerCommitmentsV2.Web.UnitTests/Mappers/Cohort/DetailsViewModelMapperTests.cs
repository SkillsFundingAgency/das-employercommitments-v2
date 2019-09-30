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
        public async Task IsApprovedByProviderIsMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture();
            var result = await fixture.Map();
            Assert.AreEqual(fixture.Cohort.IsApprovedByProvider, result.IsApprovedByProvider);
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
        public async Task DraftApprenticeshipsAreMappedCorrectly()
        {
            var fixture = new DetailsViewModelMapperTestsFixture().SetEncodingOfApprenticeIds();
            var result = await fixture.Map();

            Assert.AreEqual(fixture.DraftApprenticeshipsResponse.DraftApprenticeships.Count, result.DraftApprenticeships.Count);

            foreach (var draftApprenticeship in fixture.DraftApprenticeshipsResponse.DraftApprenticeships)
            {
                var draftApprenticeshipResult =
                    result.DraftApprenticeships.Single(x => x.Id == draftApprenticeship.Id);

                fixture.AssertEquality(draftApprenticeship, draftApprenticeshipResult);
            }
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
            DraftApprenticeshipsResponse = _autoFixture.Create<GetDraftApprenticeshipsResponse>();

            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Cohort);

            CommitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(DraftApprenticeshipsResponse);

            EncodingService = new Mock<IEncodingService>();

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
            Assert.AreEqual(source.CourseCode, result.CourseCode);
            Assert.AreEqual(source.CourseName, result.CourseName);
            Assert.AreEqual(source.Cost, result.Cost);
            Assert.AreEqual(source.StartDate, result.StartDate);
            Assert.AreEqual(source.EndDate, result.EndDate);
            Assert.AreEqual($"X{source.Id}X", result.DraftApprenticeshipHashedId);
        }
    }
}