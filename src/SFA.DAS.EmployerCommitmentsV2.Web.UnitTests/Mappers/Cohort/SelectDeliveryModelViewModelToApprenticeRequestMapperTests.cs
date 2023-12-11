using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class SelectDeliveryModelViewModelToApprenticeRequestMapperTests
{
    private SelectDeliveryModelViewModelToApprenticeRequestMapper _mapper;
    private ApprenticeRequest _result;
    private SelectDeliveryModelViewModel _source;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _source = autoFixture.Build<SelectDeliveryModelViewModel>().Without(x => x.DeliveryModels).Create();

        _mapper = new SelectDeliveryModelViewModelToApprenticeRequestMapper();

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_source.AccountHashedId));
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityId, Is.EqualTo(_source.AccountLegalEntityId));
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_source.AccountLegalEntityHashedId));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void DeliveryModelIsMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
    }

    [Test]
    public void ProviderIdIsMappedCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartMonthYear, Is.EqualTo(_source.StartMonthYear));
    }

    [Test]
    public void TransferSenderIdIsMappedCorrectly()
    {
        Assert.That(_result.TransferSenderId, Is.EqualTo(_source.TransferSenderId));
    }

    [Test]
    public void EncodedPledgeApplicationIdIsMappedCorrectly()
    {
        Assert.That(_result.EncodedPledgeApplicationId, Is.EqualTo(_source.EncodedPledgeApplicationId));
    }
}