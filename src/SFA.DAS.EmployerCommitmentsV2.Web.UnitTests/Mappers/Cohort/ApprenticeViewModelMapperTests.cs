using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class ApprenticeViewModelMapperTests
{
    private ApprenticeViewModelMapper _mapper;
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private GetAddFirstDraftApprenticeshipResponse _response;
    private ApprenticeRequest _source;
    private ApprenticeViewModel _result;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();
        _source = autoFixture.Create<ApprenticeRequest>();
        _source.TransferSenderId = string.Empty;
        _source.AccountId = 12345;
        var startDate = new DateTime(2020, 06, 01);
        _source.StartMonthYear = startDate.ToString("MMyyyy");


        _response = autoFixture.Create<GetAddFirstDraftApprenticeshipResponse>();
        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _approvalsApiClient.Setup(x => x.GetAddFirstDraftApprenticeshipDetails(_source.AccountId, _source.AccountLegalEntityId, _source.ProviderId, _source.CourseCode, startDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_response);

        _mapper = new ApprenticeViewModelMapper(_approvalsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        _result.AccountLegalEntityId.Should().Be(_source.AccountLegalEntityId);
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        _result.AccountLegalEntityHashedId.Should().Be(_source.AccountLegalEntityHashedId);
    }

    [Test]
    public void AccountLegalEntityNameIsMappedCorrectly()
    {
        _result.LegalEntityName.Should().Be(_response.LegalEntityName);
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        _result.StartDate.Date.Should().Be(new MonthYearModel(_source.StartMonthYear).Date);
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        _result.ReservationId.Should().Be(_source.ReservationId);
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        _result.CourseCode.Should().Be(_source.CourseCode);
    }

    [Test]
    public void ProviderIdIsMappedCorrectly()
    {
        _result.ProviderId.Should().Be(_source.ProviderId);
    }

    [Test]
    public void ProviderNameIsMappedCorrectly()
    {
        _result.ProviderName.Should().Be(_response.ProviderName);
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
    public void StandardPageUrlIsMappedCorrectly()
    {
        _result.StandardPageUrl.Should().Be(_response.StandardPageUrl);
    }

    [Test]
    public void FundingBandMaxIsMappedCorrectly()
    {
        _result.FundingBandMax.Should().Be(_response.ProposedMaxFunding);
    }
}