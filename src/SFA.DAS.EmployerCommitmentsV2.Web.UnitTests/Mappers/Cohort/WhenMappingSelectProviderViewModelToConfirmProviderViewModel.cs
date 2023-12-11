using System.Threading.Tasks;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingSelectProviderViewModelToConfirmProviderViewModel
{
    [Test, AutoData]
    public async Task ThenMapsReservationId(
        ConfirmProviderViewModel request,
        SelectProviderViewModelFromConfirmProviderMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.ReservationId, Is.EqualTo(request.ReservationId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsAccountHashedId(
        ConfirmProviderViewModel request,
        SelectProviderViewModelFromConfirmProviderMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsCourseCode(
        ConfirmProviderViewModel request,
        SelectProviderViewModelFromConfirmProviderMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.CourseCode, Is.EqualTo(request.CourseCode));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsStartMonthYear(
        ConfirmProviderViewModel request,
        SelectProviderViewModelFromConfirmProviderMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.StartMonthYear, Is.EqualTo(request.StartMonthYear));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId(
        ConfirmProviderViewModel request,
        SelectProviderViewModelFromConfirmProviderMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.AccountLegalEntityHashedId, Is.EqualTo(request.AccountLegalEntityHashedId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsTransferSenderId(
        ConfirmProviderViewModel request,
        SelectProviderViewModelFromConfirmProviderMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.TransferSenderId, Is.EqualTo(request.TransferSenderId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsOrigin(
        ConfirmProviderViewModel request,
        SelectProviderViewModelFromConfirmProviderMapper mapper)
    {
        var result = await mapper.Map(request);

        Assert.That(result.Origin, Is.EqualTo(request.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices));
    }
}