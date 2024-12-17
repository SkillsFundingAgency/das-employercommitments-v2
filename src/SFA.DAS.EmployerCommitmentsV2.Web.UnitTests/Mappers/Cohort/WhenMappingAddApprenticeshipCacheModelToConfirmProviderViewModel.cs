using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingAddApprenticeshipCacheModelToConfirmProviderViewModel
{
    [Test, MoqAutoData]
    public async Task Then_Maps_AccountHashedId(
        AddApprenticeshipCacheModel request,
        ConfirmProviderViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_LegalEntityName(
        AddApprenticeshipCacheModel request,
        ConfirmProviderViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.LegalEntityName.Should().Be(request.LegalEntityName);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_ProviderId(
        AddApprenticeshipCacheModel request,
        ConfirmProviderViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.ProviderId.Should().Be(request.ProviderId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_ProviderName(
        AddApprenticeshipCacheModel request,
        [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
        GetProviderResponse provider,
        ConfirmProviderViewModelMapper mapper)
    {
        commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(provider);
        var viewModel = await mapper.Map(request);
        viewModel.ProviderName.Should().Be(provider.Name);
    }
}