using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class CreateCohortRequestMapperTests
{
    private CreateCohortRequestMapper _mapper;
    private ApprenticeViewModel _source;
    private CreateCohortApimRequest _result;

    [SetUp]
    public async Task Arrange()
    {
        var fixture = new Fixture();

        var birthDate = fixture.Create<DateTime?>();
        var startDate = fixture.Create<DateTime?>();
        var endDate = fixture.Create<DateTime?>();
        var employmentEndDate = fixture.Create<DateTime?>();

        _mapper = new CreateCohortRequestMapper(Mock.Of<IAuthenticationService>());

        _source = fixture.Build<ApprenticeViewModel>()
            .With(x => x.BirthDay, birthDate?.Day)
            .With(x => x.BirthMonth, birthDate?.Month)
            .With(x => x.BirthYear, birthDate?.Year)
            .With(x => x.Cost, birthDate?.Year)
            .With(x => x.EndMonth, endDate?.Month)
            .With(x => x.EndYear, endDate?.Year)
            .With(x => x.StartMonth, startDate?.Month)
            .With(x => x.StartYear, startDate?.Year)
            .With(x => x.EmploymentEndMonth, employmentEndDate?.Month)
            .With(x => x.EmploymentEndYear, employmentEndDate?.Year)
            .Without(x => x.StartDate)
            .Without(x => x.Courses)
            .Create();

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void ThenReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void ThenFirstNameIsMappedCorrectly()
    {
        Assert.That(_result.FirstName, Is.EqualTo(_source.FirstName));
    }

    [Test]
    public void ThenDateOfBirthIsMappedCorrectly()
    {
        Assert.That(_result.DateOfBirth, Is.EqualTo(_source.DateOfBirth.Date));
    }

    [Test]
    public void ThenUniqueLearnerNumberIsMappedCorrectly()
    {
        Assert.That(_result.Uln, Is.EqualTo(_source.Uln));
    }

    [Test]
    public void ThenEmailIsMappedCorrectly()
    {
        Assert.That(_result.Email, Is.EqualTo(_source.Email));
    }

    [Test]
    public void ThenDeliveryModelIsMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
    }

    [Test]
    public void ThenCourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void ThenCostIsMappedCorrectly()
    {
        Assert.That(_result.Cost, Is.EqualTo(_source.Cost));
    }

    [Test]
    public void ThenEmploymentPriceIsMappedCorrectly()
    {
        Assert.That(_result.EmploymentPrice, Is.EqualTo(_source.EmploymentPrice));
    }

    [Test]
    public void ThenStartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartDate, Is.EqualTo(_source.StartDate.Date));
    }

    [Test]
    public void ThenEndDateIsMappedCorrectly()
    {
        Assert.That(_result.EndDate, Is.EqualTo(_source.EndDate.Date));
    }

    [Test]
    public void ThenEmploymentEndDateIsMappedCorrectly()
    {
        Assert.That(_result.EmploymentEndDate, Is.EqualTo(_source.EmploymentEndDate.Date));
    }

    [Test]
    public void ThenOriginatorReferenceIsMappedCorrectly()
    {
        Assert.That(_result.OriginatorReference, Is.EqualTo(_source.Reference));
    }

    [Test]
    public void ThenProviderIdIsMappedCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
    }

    [Test]
    public void ThenAccountIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountId, Is.EqualTo(_source.AccountId));
    }

    [Test]
    public void ThenTransferSenderIdIsMappedCorrectly()
    {
        Assert.That(_result.TransferSenderId, Is.EqualTo(_source.DecodedTransferSenderId));
    }

    [Test]
    public void ThenPledgeApplicationIdIsMappedCorrectly()
    {
        Assert.That(_result.PledgeApplicationId, Is.EqualTo(_source.PledgeApplicationId));
    }

    [Test]
    public void ThenRequestingPartyIsNotMissing()
    {
        Assert.That(_result.RequestingParty, Is.EqualTo(Party.Employer));
    }
}