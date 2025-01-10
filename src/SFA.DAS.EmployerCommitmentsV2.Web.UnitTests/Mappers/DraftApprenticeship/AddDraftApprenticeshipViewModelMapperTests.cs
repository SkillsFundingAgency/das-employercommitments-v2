using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class AddDraftApprenticeshipViewModelMapperTests
{
    private AddDraftApprenticeshipViewModelMapper _mapper;
    private AddDraftApprenticeshipRequest _source;
    private AddDraftApprenticeshipViewModel _result;
    private long _accountId;

    private Mock<IApprovalsApiClient> _client;
    private Mock<IEncodingService> _encodingService;
    private string _encodedTransferSenderId;
    private GetAddAnotherDraftApprenticeshipResponse _response;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();
        _accountId = autoFixture.Create<long>();
        _source = autoFixture.Create<AddDraftApprenticeshipRequest>();
        _source.StartMonthYear = "092020";

        _response = autoFixture.Create<GetAddAnotherDraftApprenticeshipResponse>();
        _client = new Mock<IApprovalsApiClient>();
        _client.Setup(x => x.GetAddAnotherDraftApprenticeshipDetails(_accountId, _source.CohortId, _source.CourseCode, It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_response);

        _encodedTransferSenderId = autoFixture.Create<string>();
        _encodingService = new Mock<IEncodingService>();
        _encodingService
            .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(e => e == EncodingType.PublicAccountId)))
            .Returns(_encodedTransferSenderId);
        _encodingService
            .Setup(x => x.Decode(_source.AccountHashedId, EncodingType.AccountId))
            .Returns(_accountId);

        _mapper = new AddDraftApprenticeshipViewModelMapper(_client.Object, _encodingService.Object);


        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        _result.CourseCode.Should().Be(_source.CourseCode);
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        _result.StartDate.Date.Should().Be(new MonthYearModel(_source.StartMonthYear).Date);
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
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        _result.AccountLegalEntityId.Should().Be(_source.AccountLegalEntityId);
    }

    [Test]
    public void CohortIdIsMappedCorrectly()
    {
        _result.CohortId.Should().Be(_source.CohortId);
    }

    [Test]
    public void CohortReferenceIsMappedCorrectly()
    {
        _result.CohortReference.Should().Be(_source.CohortReference);
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        _result.ReservationId.Should().Be(_source.ReservationId);
    }

    [Test]
    public void ProviderNameIsMappedCorrectly()
    {
        _result.ProviderName.Should().Be(_response.ProviderName);
    }

    [Test]
    public void LegalEntityNameIsMappedCorrectly()
    {
        _result.LegalEntityName.Should().Be(_response.LegalEntityName);
    }

    [Test]
    public void ThrowsWhenResponseNotReturned()
    {
        _client.Setup(x => x.GetAddAnotherDraftApprenticeshipDetails(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetAddAnotherDraftApprenticeshipResponse)null);

        Assert.ThrowsAsync<CohortEmployerUpdateDeniedException>(() => _mapper.Map(_source));
    }

    [Test]
    public void IsOnFlexiPaymentPilotIsFalse()
    {
        _result.IsOnFlexiPaymentPilot.Should().BeFalse();
    }

    [Test]
    public void IsStandardPageUrlIsMapped()
    {
        _result.StandardPageUrl.Should().Be(_response.StandardPageUrl);
    }

    [Test]
    public void IsFundingBandMaxIsMapped()
    {
        _result.FundingBandMax.Should().Be(_response.ProposedMaxFunding);
    }
}