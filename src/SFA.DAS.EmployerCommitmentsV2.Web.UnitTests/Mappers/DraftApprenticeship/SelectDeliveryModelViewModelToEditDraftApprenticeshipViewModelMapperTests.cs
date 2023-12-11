using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class SelectDeliveryModelViewModelToEditDraftApprenticeshipViewModelMapperTests
{
    private SelectDeliveryModelViewModelToEditDraftApprenticeshipViewModelMapper _mapper;
    private EditDraftApprenticeshipViewModel _result;
    private SelectDeliveryModelViewModel _source;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _source = autoFixture.Build<SelectDeliveryModelViewModel>().Without(x => x.DeliveryModels).Create();

        _mapper = new SelectDeliveryModelViewModelToEditDraftApprenticeshipViewModelMapper();

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_source.AccountHashedId));
    }

    [Test]
    public void CohortIdIsMappedCorrectly()
    {
        Assert.That(_result.CohortId, Is.EqualTo(_source.CohortId));
    }

    [Test]
    public void CohortReferenceIsMappedCorrectly()
    {
        Assert.That(_result.CohortReference, Is.EqualTo(_source.CohortReference));
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

    public void DraftApprenticeshipHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.DraftApprenticeshipHashedId, Is.EqualTo(_source.DraftApprenticeshipHashedId));
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
}