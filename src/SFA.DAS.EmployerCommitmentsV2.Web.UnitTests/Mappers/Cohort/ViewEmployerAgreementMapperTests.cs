using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class ViewEmployerAgreementMapperTests
    {
        [Test]
        public async Task AccountHashedIdMapsCorrectly()
        {
            var f = new ViewEmployerAgreementMapperTestFixture();
            var result = await f.Map();
            Assert.AreEqual(f.Source.AccountHashedId, result.AccountHashedId);
        }

        [Test]
        public async Task AgreementHashedIdMapsCorrectly()
        {
            var f = new ViewEmployerAgreementMapperTestFixture();
            var result = await f.Map();
            f.VerifyAgreementIdIsFetchedAndEncodedCorrectly(result.AgreementHashedId);
        }

        [Test]
        public async Task AgreementHashedIdMapsCorrectlyWhenThereAreNoAgreements()
        {
            var f = new ViewEmployerAgreementMapperTestFixture().NoAgreements();
            var result = await f.Map();
            Assert.IsNull(result.AgreementHashedId);
        }
    }

    public class ViewEmployerAgreementMapperTestFixture
    {
        public ViewEmployerAgreementMapperTestFixture()
        {
            var autoFixture = new Fixture();
            AccountLegalEntityId = autoFixture.Create<long>();
            AgreementId = autoFixture.Create<long>();

            Cohort = autoFixture.Build<GetCohortResponse>().With(x => x.AccountLegalEntityId, AccountLegalEntityId).Create();

            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            CommitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Cohort);
            CommitmentsApiClient.Setup(x => x.GetLatestAgreementId(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(AgreementId);

            EncodingService = new Mock<IEncodingService>();
            EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.AccountId))
                .Returns("XYZ");

            Source = autoFixture.Build<DetailsViewModel>().Without(x => x.Courses).Create();

            Sut = new ViewEmployerAgreementMapper(CommitmentsApiClient.Object, EncodingService.Object);
        }

        public long AccountLegalEntityId { get; set; }
        public long AgreementId { get; set; }
        public ViewEmployerAgreementMapper Sut { get; set; }
        public GetCohortResponse Cohort { get; set; }
        public ViewEmployerAgreementRequest Result { get; set; }
        public DetailsViewModel Source { get; set; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
        public Mock<IEncodingService> EncodingService { get; set; }

        public Task<ViewEmployerAgreementRequest> Map()
        {
            return Sut.Map(TestHelper.Clone(Source));
        }

        public ViewEmployerAgreementMapperTestFixture NoAgreements()
        {
            CommitmentsApiClient.Setup(x => x.GetLatestAgreementId(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((long?)null);

            return this;
        }

        public void VerifyAgreementIdIsFetchedAndEncodedCorrectly(string agreementHashedId)
        {
            CommitmentsApiClient.Verify(x=>x.GetCohort(Source.CohortId, It.IsAny<CancellationToken>()));
            CommitmentsApiClient.Verify(x=>x.GetLatestAgreementId(Cohort.AccountLegalEntityId, It.IsAny<CancellationToken>()));
            Assert.AreEqual("XYZ", agreementHashedId);
        }

    }

}
