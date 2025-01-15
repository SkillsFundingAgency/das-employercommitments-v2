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
    private AddApprenticeshipCacheModel _source;
    private SelectFundingViewModel _result;
    private GetSelectFundingOptionsResponse _fundingOptionsResponse;
    private Mock<IApprovalsApiClient> _outerApiMock;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();
        _outerApiMock = new Mock<IApprovalsApiClient>();
        _fundingOptionsResponse = autoFixture.Create<GetSelectFundingOptionsResponse>();
        _mapper = new SelectFundingViewModelMapper(_outerApiMock.Object, Mock.Of<ILogger<SelectFundingViewModelMapper>>());
        _source = autoFixture.Create<AddApprenticeshipCacheModel>();
        _outerApiMock.Setup(x => x.GetSelectFundingOptions(_source.AccountId, CancellationToken.None)).ReturnsAsync(_fundingOptionsResponse);
        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        _result.AccountHashedId.Should().Be(_source.AccountHashedId);
    }

    [Test]
    public void FundingTypeIsMappedCorrectly()
    {
        _result.FundingType.Should().Be(_source.FundingType);
    }

    [Test]
    public void ApprenticeshipSessionKeyIsMappedCorrectly()
    {
        _result.ApprenticeshipSessionKey.Should().Be(_source.ApprenticeshipSessionKey);
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

    [Test]
    public void HasLtmTransfersAvailableIsMappedCorrectly()
    {
        _result.HasLtmTransfersAvailable.Should().Be(_fundingOptionsResponse.HasLtmTransfersAvailable);
    }
}