using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class MessageViewModelToCreateCohortWithOtherPartyRequestMapperTests
    {
        private CreateCohortWithOtherPartyRequestMapper _mapper;
        private MessageViewModel _source;
        private CreateCohortWithOtherPartyRequest _result;

        [SetUp]
        public async Task Arrange()
        {
            var fixture = new Fixture();

            _mapper = new CreateCohortWithOtherPartyRequestMapper();

            _source = fixture.Build<MessageViewModel>().Create();

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void ThenAccountLegalEntityIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityId, _result.AccountLegalEntityId);
        }

        [Test]
        public void ThenProviderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ProviderId, _result.ProviderId);
        }

        [Test]
        public void ThenMessageIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Message, _result.Message);
        }

        [Test]
        public void ThenAccountIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountId, _result.AccountId);
        }


        [Test]
        public void ThenTransferSenderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DecodedTransferSenderId, _result.TransferSenderId);
        }
    }
}
