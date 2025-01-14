using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class AssignViewModelToCreateCohortWithOtherPartyRequestMapperTests
{
    private CreateCohortWithOtherPartyRequestMapper _mapper;
    private AddApprenticeshipCacheModel _source;
    private CreateCohortWithOtherPartyRequest _result;
    private Mock<IEncodingService> _mockEncodingService;

    [SetUp]
    public async Task Arrange()
    {
        var fixture = new Fixture();
        _source = fixture.Build<AddApprenticeshipCacheModel>().Create();

        _mockEncodingService = new Mock<IEncodingService>();

        _mockEncodingService.Setup(x => x.TryDecode(_source.TransferSenderId, EncodingType.PublicAccountId, out It.Ref<long>.IsAny))
            .Returns((string id, EncodingType type, out long decodedId) =>
            {
                decodedId = 123;
                return true;
            });

        _mockEncodingService.Setup(x => x.TryDecode(_source.EncodedPledgeApplicationId, EncodingType.PledgeApplicationId, out It.Ref<long>.IsAny))
            .Returns((string id, EncodingType type, out long decodedId) =>
            {
                decodedId = 456;
                return true;
            });

        _mapper = new CreateCohortWithOtherPartyRequestMapper(_mockEncodingService.Object);

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
        _result.TransferSenderId.Should().Be(123);
    }

    [Test]
    public void ThenPledgeApplicationIdIsMappedCorrectly()
    {
        _result.PledgeApplicationId.Should().Be(456);
    }

    [Test]
    public async Task ThenTransferSenderIdMapsToNullWhenDecodedValueIsZero()
    {
        _mockEncodingService.Setup(x => x.TryDecode(_source.TransferSenderId, EncodingType.PublicAccountId, out It.Ref<long>.IsAny))
            .Returns((string id, EncodingType type, out long decodedId) =>
            {
                decodedId = 0;
                return true;
            });

        _result = await _mapper.Map(_source);

        _result.TransferSenderId.Should().BeNull();
    }

    [Test]
    public async Task ThenTransferSenderIdIsNullWhenTransferIdIsNull()
    {
        _source.TransferSenderId = null;
        _result = await _mapper.Map(_source);

        _result.TransferSenderId.Should().BeNull();
    }

    [Test]
    public async Task ThenPledgeApplicationIdMapsToNullWhenDecodedValueIsZero()
    {
        _mockEncodingService.Setup(x => x.TryDecode(_source.EncodedPledgeApplicationId, EncodingType.PledgeApplicationId, out It.Ref<long>.IsAny))
            .Returns((string id, EncodingType type, out long decodedId) =>
            {
                decodedId = 0;
                return true;
            });

        _result = await _mapper.Map(_source);

        _result.PledgeApplicationId.Should().BeNull();
    }

    public async Task ThenPledgeApplicationIdIsNullWhenEncodedPledgeApplicationIdIsNull()
    {
        _source.EncodedPledgeApplicationId = null;

        _result = await _mapper.Map(_source);

        _result.PledgeApplicationId.Should().BeNull();
    }
}