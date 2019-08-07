using AutoFixture;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class MessageViewModelToCreateCohortWithOtherPartyRequestMapperTests
    {
        private MessageViewModelToCreateCohortWithOtherPartyRequestMapper _mapper;
        private MessageViewModel _source;
        private CreateCohortWithOtherPartyRequest _result;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            _mapper = new MessageViewModelToCreateCohortWithOtherPartyRequestMapper();

            _source = fixture.Build<MessageViewModel>().Create();

            _result = _mapper.Map(TestHelper.Clone(_source));
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
    }
}
