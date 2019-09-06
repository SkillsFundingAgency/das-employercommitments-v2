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

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _cohort = autoFixture.Create<GetCohortResponse>();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_cohort);
            
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
    }
}
