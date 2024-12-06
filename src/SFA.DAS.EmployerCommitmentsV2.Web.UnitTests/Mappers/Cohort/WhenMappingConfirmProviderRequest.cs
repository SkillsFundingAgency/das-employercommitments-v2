using FluentAssertions;
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

        result.ReservationId.Should().Be(viewModel.ReservationId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsCourseCode(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        result.CourseCode.Should().Be(viewModel.CourseCode);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsAccountHashedId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        result.AccountHashedId.Should().Be(viewModel.AccountHashedId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        result.AccountLegalEntityHashedId.Should().Be(viewModel.AccountLegalEntityHashedId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsLegalEntityName(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        result.LegalEntityName.Should().Be(viewModel.LegalEntityName);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsStartMonthYear(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        result.StartMonthYear.Should().Be(viewModel.StartMonthYear);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsProviderId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        result.ProviderId.Should().Be(providerId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsTransferSenderId(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        result.TransferSenderId.Should().Be(viewModel.TransferSenderId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsFundingType(
        SelectProviderViewModel viewModel,
        long providerId,
        ConfirmProviderRequestMapper mapper)
    {
        viewModel.ProviderId = providerId.ToString();

        var result = await mapper.Map(viewModel);

        result.FundingType.Should().Be(viewModel.FundingType);
    }

}