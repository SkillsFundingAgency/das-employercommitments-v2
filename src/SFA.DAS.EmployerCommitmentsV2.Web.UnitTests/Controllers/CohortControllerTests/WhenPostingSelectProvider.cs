using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Http;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenPostingSelectProvider
{
    [Test, MoqAutoData]
    public async Task ThenCallsApi(
        SelectProviderViewModel viewModel,
        AddApprenticeshipCacheModel cacheModel,
        long providerId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        GetProviderResponse apiResponse,
        [Greedy] CohortController controller)
    {
        viewModel.ProviderId = providerId.ToString();

        cacheModel.ApprenticeshipSessionKey = viewModel.ApprenticeshipSessionKey.Value;
        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(cacheModel);

        cacheStorageService
         .Setup(x => x.SaveToCache(cacheModel.ApprenticeshipSessionKey, It.IsAny<AddApprenticeshipCacheModel>(), 1))
         .Returns(Task.CompletedTask);

        mockApiClient
           .Setup(x => x.GetProvider(long.Parse(viewModel.ProviderId), CancellationToken.None))
           .ReturnsAsync(apiResponse);


        await controller.SelectProvider(viewModel);

        mockApiClient.Verify(x => x.GetProvider(providerId, CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndApiThrowsNotFoundException_ThenReturnsRedirectResult(
        SelectProviderViewModel viewModel,
        long providerId,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        HttpResponseMessage error,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = viewModel.ApprenticeshipSessionKey.Value;
        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(cacheModel);

        error.StatusCode = HttpStatusCode.NotFound;
        viewModel.ProviderId = providerId.ToString();
        mockApiClient
            .Setup(x => x.GetProvider(providerId, CancellationToken.None))
            .ThrowsAsync(new RestHttpClientException(error, error.ReasonPhrase));

        var result = await controller.SelectProvider(viewModel) as RedirectToActionResult;

        result.ActionName.Should().Be(nameof(CohortController.SelectProvider));
        controller.ModelState.IsValid.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task AndUnexpectedExceptionThrown_ThenReturnsErrorView(
        SelectProviderViewModel viewModel,
        long providerId,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        HttpResponseMessage error,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = viewModel.ApprenticeshipSessionKey.Value;
        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(cacheModel);

        error.StatusCode = HttpStatusCode.NetworkAuthenticationRequired;
        viewModel.ProviderId = providerId.ToString();
        mockApiClient
            .Setup(x => x.GetProvider(providerId, CancellationToken.None))
            .ThrowsAsync(new RestHttpClientException(error, error.ReasonPhrase));

        var result = await controller.SelectProvider(viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ControllerName.Should().Be("Error");
    }


    [Test, MoqAutoData]
    public async Task ThenRedirectsConfirmProvider(
        SelectProviderViewModel viewModel,
        long providerId,
        ValidationResult validationResult,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IValidator<SelectProviderViewModel>> mockValidator,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        GetProviderResponse apiResponse,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = viewModel.ApprenticeshipSessionKey.Value;
        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(cacheModel);

        viewModel.ProviderId = providerId.ToString();
        mockValidator
            .Setup(x => x.Validate(viewModel))
            .Returns(validationResult);

        mockApiClient
            .Setup(x => x.GetProvider(long.Parse(viewModel.ProviderId), CancellationToken.None))
            .ReturnsAsync(apiResponse);

        var result = await controller.SelectProvider(viewModel);

        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("ConfirmProvider");
        redirectResult.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        redirectResult.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);
    }
}