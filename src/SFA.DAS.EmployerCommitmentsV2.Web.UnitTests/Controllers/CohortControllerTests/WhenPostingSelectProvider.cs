using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Http;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    public class WhenPostingSelectProvider
    {
        [Test, MoqAutoData]
        public async Task ThenCallsApi(
            SelectProviderViewModel viewModel,
            long providerId,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetProviderResponse apiResponse,
            CohortController controller)
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
            CohortController controller)
        {
            modelMapper.Setup(x => x.Map<SelectProviderRequest>(viewModel))
                .ReturnsAsync(new SelectProviderRequest());

            error.StatusCode = HttpStatusCode.NotFound;
            viewModel.ProviderId = providerId.ToString();
            mockApiClient
                .Setup(x => x.GetProvider(providerId, CancellationToken.None))
                .ThrowsAsync(new RestHttpClientException(error,error.ReasonPhrase));

            var result = await controller.SelectProvider(viewModel) as RedirectToActionResult;

            Assert.AreEqual(nameof(CohortController.SelectProvider), result.ActionName);
            Assert.False(controller.ModelState.IsValid);
        }

        [Test, MoqAutoData]
        public async Task AndUnexpectedExceptionThrown_ThenReturnsErrorView(
            SelectProviderViewModel viewModel,
            long providerId,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetProviderResponse apiResponse,
            HttpResponseMessage error,
            CohortController controller)
        {
            error.StatusCode = HttpStatusCode.NetworkAuthenticationRequired;
            viewModel.ProviderId = providerId.ToString();
            mockApiClient
                .Setup(x => x.GetProvider(providerId, CancellationToken.None))
                .ThrowsAsync(new RestHttpClientException(error, error.ReasonPhrase));

            var result = await controller.SelectProvider(viewModel) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.AreEqual("Error",result.ActionName);
            Assert.AreEqual("Error",result.ControllerName);
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
            CohortController controller)
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
            CohortController controller)
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

            Assert.NotNull(result);
            Assert.AreEqual(actionName,result.ActionName);
        }
    }
}
