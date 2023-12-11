using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingConfirmProviderRequest
{
    [Test, MoqAutoData]
    public async Task ThenMapsReservationId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        Assert.That(result.ReservationId, Is.EqualTo(viewModel.ReservationId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsCourseCode(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        Assert.That(result.CourseCode, Is.EqualTo(viewModel.CourseCode));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsAccountHashedId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        Assert.That(result.AccountHashedId, Is.EqualTo(viewModel.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        Assert.That(result.AccountLegalEntityHashedId, Is.EqualTo(viewModel.AccountLegalEntityHashedId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsLegalEntityName(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        Assert.That(result.LegalEntityName, Is.EqualTo(viewModel.LegalEntityName));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsStartMonthYear(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        Assert.That(result.StartMonthYear, Is.EqualTo(viewModel.StartMonthYear));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsProviderId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        Assert.That(result.ProviderId, Is.EqualTo(providerId));
    }

    [Test, MoqAutoData]
    public async Task ThenMapsTransferSenderId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        Assert.That(result.TransferSenderId, Is.EqualTo(viewModel.TransferSenderId));
    }
}