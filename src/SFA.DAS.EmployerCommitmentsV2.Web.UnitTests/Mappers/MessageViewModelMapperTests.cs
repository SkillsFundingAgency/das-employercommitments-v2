using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class MessageViewModelMapperTests
    {
        private MessageViewModelMapper _mapper;
        private MessageRequest _source;
        private MessageViewModel _result;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private GetProviderResponse _providerResponse;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _source = autoFixture.Create<MessageRequest>();
            _source.StartMonthYear = "062020";

            _providerResponse = autoFixture.Create<GetProviderResponse>();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_providerResponse);


            _mapper = new MessageViewModelMapper(_commitmentsApiClient.Object);

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void ThenReservationIdIsMapperCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void ThenStartMonthYearIsMapperCorrectly()
        {
            Assert.AreEqual(_source.StartMonthYear, _result.StartMonthYear);
        }
        [Test]
        public void ThenAccountHashedIdIsMapperCorrectly()
        {
            Assert.AreEqual(_source.AccountHashedId, _result.AccountHashedId);
        }
        [Test]
        public void ThenAccountLegalEntityHashedIdIsMapperCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityHashedId, _result.AccountLegalEntityHashedId);
        }
        [Test]
        public void ThenCourseCodeIsMapperCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }
        [Test]
        public void ThenProviderIdIsMapperCorrectly()
        {
            Assert.AreEqual(_source.ProviderId, _result.ProviderId);
        }

        [Test]
        public void ThenProviderNameIsMapperCorrectly()
        {
            Assert.AreEqual(_providerResponse.Name, _result.ProviderName);
        }
    }
}
