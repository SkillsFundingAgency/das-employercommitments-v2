using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingAddApprenticeshipCacheModelToSelectProviderViewModel
{
    private AddApprenticeshipCacheModel _request;
    private Mock<IApprovalsApiClient> _approvalsApiClientMock;
    private GetSelectProviderDetailsResponse _selectProvidersResponse;
    private SelectProviderViewModelMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var autoFixture = new Fixture();
        _request = autoFixture.Create<AddApprenticeshipCacheModel>();
        _selectProvidersResponse = autoFixture.Create<GetSelectProviderDetailsResponse>();

        _approvalsApiClientMock = new Mock<IApprovalsApiClient>();
        _approvalsApiClientMock
            .Setup(x => x.GetSelectProviderDetails(_request.AccountId, _request.AccountLegalEntityId, CancellationToken.None))
            .ReturnsAsync(_selectProvidersResponse);

        _mapper = new SelectProviderViewModelMapper(_approvalsApiClientMock.Object, Mock.Of<ILogger<SelectProviderViewModelMapper>>());
    }

    [Test]
    public async Task ThenMapsReservationId()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ReservationId, Is.EqualTo(_request.ReservationId));
    }

    [Test]
    public async Task ThenMapsAccountHashedId()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
    }

    [Test]
    public async Task ThenMapsLegalEntityName()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.LegalEntityName, Is.EqualTo(_selectProvidersResponse.AccountLegalEntity.LegalEntityName));
    }

    [Test]
    public async Task ThenMapsProviders()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.Providers, Is.EqualTo(_selectProvidersResponse.Providers));
    }

    [Test]
    public async Task ThenMapsCourseCode()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.CourseCode, Is.EqualTo(_request.CourseCode));
    }

    [Test]
    public async Task ThenMapsStartMonthYear()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.StartMonthYear, Is.EqualTo(_request.StartMonthYear));
    }

    [Test]
    public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.AccountLegalEntityHashedId, Is.EqualTo(_request.AccountLegalEntityHashedId));
    }

    [Test]
    public async Task ThenMapsOrigin()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.Origin, Is.EqualTo(_request.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices));
    }
}