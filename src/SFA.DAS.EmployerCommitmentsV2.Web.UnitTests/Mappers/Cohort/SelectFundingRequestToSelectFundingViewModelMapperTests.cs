using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class SelectFundingRequestToSelectFundingViewModelMapperTests
{
    private SelectFundingViewModelMapper _mapper;
    private SelectFundingRequest _source;
    private SelectFundingViewModel _result;
    private GetSelectFundingOptionsResponse _fundingOptionsResponse;
    private Mock<IApprovalsApiClient> _outerApiMock;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();
        _outerApiMock = new Mock<IApprovalsApiClient>();
        _fundingOptionsResponse = autoFixture.Create<GetSelectFundingOptionsResponse>();
        _mapper = new SelectFundingViewModelMapper(_outerApiMock.Object);
        _source = autoFixture.Create<SelectFundingRequest>();
        _outerApiMock.Setup(x => x.GetSelectFundingOptions(_source.AccountId, CancellationToken.None)).ReturnsAsync(_fundingOptionsResponse);
        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        _result.AccountHashedId.Should().Be(_source.AccountHashedId);
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        _result.AccountLegalEntityHashedId.Should().Be(_source.AccountLegalEntityHashedId);
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        _result.ReservationId.Should().Be(_source.ReservationId);
    }

    [Test]
    public void TransferSenderIdIsMappedCorrectly()
    {
        _result.TransferSenderId.Should().Be(_source.TransferSenderId);
    }

    [Test]
    public void EncodedPledgeApplicationIdIsMappedCorrectly()
    {
        _result.EncodedPledgeApplicationId.Should().Be(_source.EncodedPledgeApplicationId);
    }

    [Test]
    public void IsLevyAccountIsMappedCorrectly()
    {
        _result.IsLevyAccount.Should().Be(_fundingOptionsResponse.IsLevyAccount);
    }

    [Test]
    public void HasDirectTransfersAvailableIsMappedCorrectly()
    {
        _result.HasDirectTransfersAvailable.Should().Be(_fundingOptionsResponse.HasDirectTransfersAvailable);
    }

    [Test]
    public void HasUnallocatedReservationsAvailableIsMappedCorrectly()
    {
        _result.HasUnallocatedReservationsAvailable.Should().Be(_fundingOptionsResponse.HasUnallocatedReservationsAvailable);
    }

    [Test]
    public void HasAdditionalReservationFundsAvailableIsMappedCorrectly()
    {
        _result.HasAdditionalReservationFundsAvailable.Should().Be(_fundingOptionsResponse.HasAdditionalReservationFundsAvailable);
    }
}