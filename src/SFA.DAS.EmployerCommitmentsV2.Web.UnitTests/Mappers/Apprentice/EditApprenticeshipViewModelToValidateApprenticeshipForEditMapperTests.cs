using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class EditApprenticeshipViewModelToValidateApprenticeshipForEditMapperTests
{
    EditApprenticeshipRequestViewModel request;

    [SetUp]
    public void SetUp()
    {
        var fixture = new Fixture();
        fixture.Customize(new DateCustomisation());
        request = fixture.Create<EditApprenticeshipRequestViewModel>();
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipId_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeshipId, Is.EqualTo(request.ApprenticeshipId));
    }

    [Test, MoqAutoData]
    public async Task EmployerAccountId_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.EmployerAccountId, Is.EqualTo(request.AccountId));
    }

    [Test, MoqAutoData]
    public async Task FirstName_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.FirstName, Is.EqualTo(request.FirstName));
    }

    [Test, MoqAutoData]
    public async Task LastName_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.LastName, Is.EqualTo(request.LastName));
    }

    [Test, MoqAutoData]
    public async Task DateOfBirth_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.DateOfBirth, Is.EqualTo(request.DateOfBirth.Date));
    }

    [Test, MoqAutoData]
    public async Task ULN_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.ULN, Is.EqualTo(request.ULN));
    }

    [Test, MoqAutoData]
    public async Task Cost_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.Cost, Is.EqualTo(request.Cost));
    }

    [Test, MoqAutoData]
    public async Task EmployerReference_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.EmployerReference, Is.EqualTo(request.EmployerReference));
    }

    [Test, MoqAutoData]
    public async Task StartDate_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.StartDate, Is.EqualTo(request.StartDate.Date));
    }

    [Test, MoqAutoData]
    public async Task EndDate_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.EndDate, Is.EqualTo(request.EndDate.Date));
    }

    [Test, MoqAutoData]
    public async Task DeliveryModel_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.DeliveryModel, Is.EqualTo(request.DeliveryModel));
    }

    [Test, MoqAutoData]
    public async Task EmploymentEndDate_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.EmploymentEndDate, Is.EqualTo(request.EmploymentEndDate.Date));
    }

    [Test, MoqAutoData]
    public async Task EmploymentPrice_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.EmploymentPrice, Is.EqualTo(request.EmploymentPrice));
    }

    [Test, MoqAutoData]
    public async Task CourseCode_IsMapped(
        EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.TrainingCode, Is.EqualTo(request.CourseCode));
    }

    [Test, MoqAutoData]
    public async Task Version_IsMapped(EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.Version, Is.EqualTo(request.Version));
    }

    public class DateCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<EditApprenticeshipRequestViewModel>(composer =>
                composer.Without(p => p.DateOfBirth)
                    .Without(p => p.EndDate)
                    .Without(p => p.StartDate)
                    .Without(p => p.EmploymentEndDate)
                    .With(p => p.BirthDay, 1)
                    .With(p => p.BirthMonth, 12)
                    .With(p => p.BirthYear, 2000)
                    .With(p => p.StartMonth, 4)
                    .With(p => p.StartYear, 2019)
                    .With(p => p.EndMonth, 6)
                    .With(p => p.EndYear, 2021)
                    .With(p => p.EmploymentEndMonth, 6)
                    .With(p => p.EmploymentEndYear, 2020)
            );
        }
    }
}