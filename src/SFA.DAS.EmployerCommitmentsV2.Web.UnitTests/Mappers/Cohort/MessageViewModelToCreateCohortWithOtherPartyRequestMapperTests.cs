using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

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
        Assert.That(_result.AccountLegalEntityId, Is.EqualTo(_source.AccountLegalEntityId));
    }

    [Test]
    public void ThenProviderIdIsMappedCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
    }

    [Test]
    public void ThenMessageIsMappedCorrectly()
    {
        Assert.That(_result.Message, Is.EqualTo(_source.Message));
    }

    [Test]
    public void ThenAccountIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountId, Is.EqualTo(_source.AccountId));
    }


    [Test]
    public void ThenTransferSenderIdIsMappedCorrectly()
    {
        Assert.That(_result.TransferSenderId, Is.EqualTo(_source.DecodedTransferSenderId));
    }

    [Test]
    public void ThenPledgeApplicationIdIsMappedCorrectly()
    {
        Assert.That(_result.PledgeApplicationId, Is.EqualTo(_source.PledgeApplicationId));
    }
}