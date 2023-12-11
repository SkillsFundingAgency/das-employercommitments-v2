using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingSelectProviderRequestToViewModel
{
    private SelectProviderRequest _request;
    private Mock<ICommitmentsApiClient> _commitmentsApiClientMock;
    private AccountLegalEntityResponse _commitmentsApiClientResponse;
    private SelectProviderViewModelMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var autoFixture = new Fixture();
        _request = autoFixture.Create<SelectProviderRequest>();
        _commitmentsApiClientResponse = autoFixture.Create<AccountLegalEntityResponse>();

        _commitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClientMock
            .Setup(x => x.GetAccountLegalEntity(_request.AccountLegalEntityId, CancellationToken.None))
            .ReturnsAsync(_commitmentsApiClientResponse);

        _mapper = new SelectProviderViewModelMapper(_commitmentsApiClientMock.Object);
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

        Assert.That(result.LegalEntityName, Is.EqualTo(_commitmentsApiClientResponse.LegalEntityName));
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
    public async Task ThenMapsTransferSenderId()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.TransferSenderId, Is.EqualTo(_request.TransferSenderId));
    }

    [Test]
    public async Task ThenMapsOrigin()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.Origin, Is.EqualTo(_request.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices));
    }
}