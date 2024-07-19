using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class AssignViewModelToCreateCohortWithOtherPartyRequestMapperTests
{
    private CreateCohortWithOtherPartyRequestMapper _mapper;
    private AssignViewModel _source;
    private CreateCohortWithOtherPartyRequest _result;

    [SetUp]
    public async Task Arrange()
    {
        var fixture = new Fixture();

        _mapper = new CreateCohortWithOtherPartyRequestMapper();

        _source = fixture.Build<AssignViewModel>().Create();

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void ThenAccountLegalEntityIsMappedCorrectly()
    {
        _result.AccountLegalEntityId.Should().Be(_source.AccountLegalEntityId);
    }

    [Test]
    public void ThenProviderIdIsMappedCorrectly()
    {
        _result.ProviderId.Should().Be(_source.ProviderId);
    }

    [Test]
    public void ThenMessageIsMappedCorrectly()
    {
        _result.Message.Should().Be(_source.Message);
    }

    [Test]
    public void ThenAccountIdIsMappedCorrectly()
    {
        _result.AccountId.Should().Be(_source.AccountId);
    }


    [Test]
    public void ThenTransferSenderIdIsMappedCorrectly()
    {
        _result.TransferSenderId.Should().Be(_source.DecodedTransferSenderId);
    }

    [Test]
    public void ThenPledgeApplicationIdIsMappedCorrectly()
    {
        _result.PledgeApplicationId.Should().Be(_source.PledgeApplicationId);
    }
}