using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingConfirmProviderViewModelToAssignRequest
{
    [Test, AutoData]
    public async Task ThenMapsReservationId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.ReservationId, Is.EqualTo(request.ReservationId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsAccountHashedId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsCourseCode(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.CourseCode, Is.EqualTo(request.CourseCode));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsStartMonthYear(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.StartMonthYear, Is.EqualTo(request.StartMonthYear));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.AccountLegalEntityHashedId, Is.EqualTo(request.AccountLegalEntityHashedId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsProviderId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.ProviderId, Is.EqualTo(request.ProviderId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsTransferSenderId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.TransferSenderId, Is.EqualTo(request.TransferSenderId));
    }
}