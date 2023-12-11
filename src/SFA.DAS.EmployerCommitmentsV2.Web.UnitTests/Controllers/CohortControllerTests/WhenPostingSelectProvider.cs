using System.Net;
using System.Net.Http;
using FluentValidation;
using FluentValidation.Results;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
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
        long providerId,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        GetProviderResponse apiResponse,
        [Greedy] CohortController controller)
    {
        viewModel.ProviderId = providerId.ToString();
            
        await controller.SelectProvider(viewModel);

        mockApiClient.Verify(x => x.GetProvider(providerId, CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndApiThrowsNotFoundException_ThenReturnsRedirectResult(
        SelectProviderViewModel viewModel,
        long providerId,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        HttpResponseMessage error,
        [Frozen] Mock<IModelMapper> modelMapper,
        [Greedy] CohortController controller)
    {
        modelMapper.Setup(x => x.Map<SelectProviderRequest>(viewModel))
            .ReturnsAsync(new SelectProviderRequest());

        error.StatusCode = HttpStatusCode.NotFound;
        viewModel.ProviderId = providerId.ToString();
        mockApiClient
            .Setup(x => x.GetProvider(providerId, CancellationToken.None))
            .ThrowsAsync(new RestHttpClientException(error,error.ReasonPhrase));

        var result = await controller.SelectProvider(viewModel) as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            Assert.That(result.ActionName, Is.EqualTo(nameof(CohortController.SelectProvider)));
            Assert.That(controller.ModelState.IsValid, Is.False);
        });
    }

    [Test, MoqAutoData]
    public async Task AndUnexpectedExceptionThrown_ThenReturnsErrorView(
        SelectProviderViewModel viewModel,
        long providerId,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        GetProviderResponse apiResponse,
        HttpResponseMessage error,
        [Greedy] CohortController controller)
    {
        error.StatusCode = HttpStatusCode.NetworkAuthenticationRequired;
        viewModel.ProviderId = providerId.ToString();
        mockApiClient
            .Setup(x => x.GetProvider(providerId, CancellationToken.None))
            .ThrowsAsync(new RestHttpClientException(error, error.ReasonPhrase));

        var result = await controller.SelectProvider(viewModel) as RedirectToActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ActionName, Is.EqualTo("Error"));
            Assert.That(result.ControllerName, Is.EqualTo("Error"));
        });
    }


    [Test, MoqAutoData]
    public async Task ThenMapsConfirmProviderRequest(
        SelectProviderViewModel viewModel,
        long providerId,
        ValidationResult validationResult,
        [Frozen] Mock<IValidator<SelectProviderViewModel>> mockValidator,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        [Frozen] Mock<IModelMapper> mockMapper,
        GetProviderResponse apiResponse,
        [Greedy] CohortController controller)
    {
        viewModel.ProviderId = providerId.ToString();
        mockValidator
            .Setup(x => x.Validate(viewModel))
            .Returns(validationResult);
        mockApiClient
            .Setup(x => x.GetProvider(long.Parse(viewModel.ProviderId), CancellationToken.None))
            .ReturnsAsync(apiResponse);

        await controller.SelectProvider(viewModel);

        mockMapper.Verify(x => x.Map<ConfirmProviderRequest>(viewModel), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task ThenRedirectsToConfirmProvider(
        SelectProviderViewModel viewModel,
        long providerId,
        ValidationResult validationResult,
        [Frozen] Mock<IValidator<SelectProviderViewModel>> mockValidator,
        [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
        [Frozen] Mock<IModelMapper> mockMapper,
        GetProviderResponse apiResponse,
        ConfirmProviderRequest confirmProviderRequest,
        [Greedy] CohortController controller)
    {
        var actionName = "ConfirmProvider";
        viewModel.ProviderId = providerId.ToString();
        mockValidator
            .Setup(x => x.Validate(viewModel))
            .Returns(validationResult);
        mockApiClient
            .Setup(x => x.GetProvider(long.Parse(viewModel.ProviderId), CancellationToken.None))
            .ReturnsAsync(apiResponse);
        mockMapper
            .Setup(x => x.Map<ConfirmProviderRequest>(viewModel))
            .ReturnsAsync(confirmProviderRequest);

        var result = await controller.SelectProvider(viewModel) as RedirectToActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo(actionName));
    }
}