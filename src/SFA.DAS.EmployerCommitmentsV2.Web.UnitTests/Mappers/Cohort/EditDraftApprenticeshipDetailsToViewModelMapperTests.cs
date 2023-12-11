using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenIMapEditDraftApprenticeshipDetailsToViewModel
{
    private EditDraftApprenticeshipViewModelMapper _mapper;
    private EditDraftApprenticeshipDetails _source;
    private Func<Task<EditDraftApprenticeshipViewModel>> _act;

    [SetUp]
    public void Arrange()
    {
        var fixture = new Fixture();

        _mapper = new EditDraftApprenticeshipViewModelMapper();
        _source = fixture.Build<EditDraftApprenticeshipDetails>().Create();

        _act = async () => await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public async Task ThenDraftApprenticeshipIdIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.DraftApprenticeshipId, Is.EqualTo(_source.DraftApprenticeshipId));
    }

    [Test]
    public async Task ThenDraftApprenticeshipHashedIdIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.DraftApprenticeshipHashedId, Is.EqualTo(_source.DraftApprenticeshipHashedId));
    }

    [Test]
    public async Task ThenCohortIdIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.CohortId, Is.EqualTo(_source.CohortId));
    }

    [Test]
    public async Task ThenCohortReferenceIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.CohortReference, Is.EqualTo(_source.CohortReference));
    }

    [Test]
    public async Task ThenReservationIdIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public async Task ThenFirstNameIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.FirstName, Is.EqualTo(_source.FirstName));
    }

    [Test]
    public async Task ThenLastNameIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.LastName, Is.EqualTo(_source.LastName));
    }

    [Test]
    public async Task ThenEmailIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.Email, Is.EqualTo(_source.Email));
    }

    [Test]
    public async Task ThenUniqueLearnerNumberIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.Uln, Is.EqualTo(_source.UniqueLearnerNumber));
    }

    [Test]
    public async Task ThenDateOfBirthIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.DateOfBirth.Day, Is.EqualTo(_source.DateOfBirth?.Day));
        Assert.That(result.DateOfBirth.Month, Is.EqualTo(_source.DateOfBirth?.Month));
        Assert.That(result.DateOfBirth.Year, Is.EqualTo(_source.DateOfBirth?.Year));
    }

    [Test]
    public async Task ThenDeliveryModelIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
    }

    [Test]
    public async Task ThenCourseCodeIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public async Task ThenCostIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.Cost, Is.EqualTo(_source.Cost));
    }

    [Test]
    public async Task ThenStartDateIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.StartDate.Month, Is.EqualTo(_source.StartDate?.Month));
        Assert.That(result.StartDate.Year, Is.EqualTo(_source.StartDate?.Year));
    }

    [Test]
    public async Task ThenEndDateIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.EndDate.Month, Is.EqualTo(_source.EndDate?.Month));
        Assert.That(result.EndDate.Year, Is.EqualTo(_source.EndDate?.Year));
    }

    [Test]
    public async Task ThenOriginatorReferenceIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.Reference, Is.EqualTo(_source.OriginatorReference));
    }

    [Test]
    public async Task ThenProviderIdIsMappedCorrectly()
    {
        var result = await _act();
        Assert.That(result.ProviderId, Is.EqualTo(_source.ProviderId));
    }
}