using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGettingConfirmProvider
{
    [Test, MoqAutoData]
    public async Task Then_The_View_Is_Returned(
        int providerId,
        AddApprenticeshipCacheModel cacheModel,
        ConfirmProviderRequest confirmProviderRequest,
        GetProviderResponse getProviderResponse,
        ConfirmProviderViewModel viewModel,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.AddApprenticeshipCacheKey))
           .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(mapper => mapper.Map<ConfirmProviderViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        confirmProviderRequest.ProviderId = providerId;
        mockApiClient
            .Setup(x => x.GetProvider(providerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(getProviderResponse);

        var result = await controller.ConfirmProvider(cacheModel.AddApprenticeshipCacheKey) as ViewResult;

        result.ViewName.Should().BeNull();
    }
}