using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTests
{
    private AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper _mapper;
    private AddDraftApprenticeshipRequest _result;
    private AddDraftApprenticeshipViewModel _source;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        var birthDate = autoFixture.Create<DateTime?>();
        var startDate = autoFixture.Create<DateTime?>();
        var endDate = autoFixture.Create<DateTime?>();
        var employmentEndDate = autoFixture.Create<DateTime?>();

        _source = autoFixture.Build<AddDraftApprenticeshipViewModel>()
            .With(x => x.BirthDay, birthDate?.Day)
            .With(x => x.BirthMonth, birthDate?.Month)
            .With(x => x.BirthYear, birthDate?.Year)
            .With(x => x.EndMonth, endDate?.Month)
            .With(x => x.EndYear, endDate?.Year)
            .With(x => x.StartMonth, startDate?.Month)
            .With(x => x.StartYear, startDate?.Year)
            .With(x => x.EmploymentEndMonth, employmentEndDate?.Month)
            .With(x => x.EmploymentEndYear, employmentEndDate?.Year)
            .Without(x => x.StartDate)
            .Without(x => x.Courses)
            .Create();            

        _mapper = new AddDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper();

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
    public void CostIsMappedCorrectly()
    {
        Assert.That(_result.Cost, Is.EqualTo(_source.Cost));
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
    public void EmploymentPriceIsMappedCorrectly()
    {
        Assert.That(_result.EmploymentPrice, Is.EqualTo(_source.EmploymentPrice));
    }

    [Test]
    public void EmploymentEndDateIsMappedCorrectly()
    {
        Assert.That(_result.EmploymentEndDate.Value, Is.EqualTo(_source.EmploymentEndDate.Date));
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