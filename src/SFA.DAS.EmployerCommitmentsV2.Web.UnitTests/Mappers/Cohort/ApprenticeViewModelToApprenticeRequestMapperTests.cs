using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class ApprenticeViewModelToApprenticeRequestMapperTests
{
    private ApprenticeViewModelToApprenticeRequestMapper _mapper;
    private ApprenticeRequest _result;
    private ApprenticeViewModel _source;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        var birthDate = autoFixture.Create<DateTime?>();
        var startDate = autoFixture.Create<DateTime?>();
        var endDate = autoFixture.Create<DateTime?>();
        var employmentEndDate = autoFixture.Create<DateTime?>();

        _source = autoFixture.Build<ApprenticeViewModel>()
            .With(x => x.BirthDay, birthDate?.Day)
            .With(x => x.BirthMonth, birthDate?.Month)
            .With(x => x.BirthYear, birthDate?.Year)
            .With(x => x.Cost, 1600)
            .With(x => x.EndMonth, endDate?.Month)
            .With(x => x.EndYear, endDate?.Year)
            .With(x => x.StartMonth, startDate?.Month)
            .With(x => x.StartYear, startDate?.Year)
            .With(x => x.EmploymentEndMonth, employmentEndDate?.Month)
            .With(x => x.EmploymentEndYear, employmentEndDate?.Year)
            .Without(x => x.StartDate)
            .Without(x => x.Courses)
            .Create();

        _mapper = new ApprenticeViewModelToApprenticeRequestMapper();

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
    public void LegalEntityNameIsMappedCorrectly()
    {
        Assert.That(_result.LegalEntityName, Is.EqualTo(_source.LegalEntityName));
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void ProviderIdIsMappedCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
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

    [Test]
    public void OriginIsMappedCorrectly()
    {
        Assert.That(_result.Origin, Is.EqualTo(_source.Origin));
    }

    [Test]
    public void DeliveryModelIsMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
    }
}