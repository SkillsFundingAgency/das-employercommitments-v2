using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class MessageViewModelMapperTests
{
    private MessageViewModelMapper _mapper;
    private MessageRequest _source;
    private MessageViewModel _result;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private GetProviderResponse _providerResponse;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _source = autoFixture.Create<MessageRequest>();
        _source.StartMonthYear = "062020";

        _providerResponse = autoFixture.Create<GetProviderResponse>();

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_providerResponse);


        _mapper = new MessageViewModelMapper(_commitmentsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void ThenReservationIdIsMapperCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void ThenStartMonthYearIsMapperCorrectly()
    {
        Assert.That(_result.StartMonthYear, Is.EqualTo(_source.StartMonthYear));
    }
    [Test]
    public void ThenAccountHashedIdIsMapperCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_source.AccountHashedId));
    }
    [Test]
    public void ThenAccountLegalEntityHashedIdIsMapperCorrectly()
    {
        Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_source.AccountLegalEntityHashedId));
    }
    [Test]
    public void ThenLegalEntityNameIsMappedCorrectly()
    {
        Assert.That(_source.LegalEntityName, Is.EqualTo(_source.LegalEntityName));
    }
    [Test]
    public void ThenCourseCodeIsMapperCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }
    [Test]
    public void ThenProviderIdIsMapperCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
    }

    [Test]
    public void ThenProviderNameIsMapperCorrectly()
    {
        Assert.That(_result.ProviderName, Is.EqualTo(_providerResponse.Name));
    }

    [Test]
    public void ThenTransferSenderIdIsMappedCorrectly()
    {
        Assert.That(_result.TransferSenderId, Is.EqualTo(_source.TransferSenderId));
    }
}