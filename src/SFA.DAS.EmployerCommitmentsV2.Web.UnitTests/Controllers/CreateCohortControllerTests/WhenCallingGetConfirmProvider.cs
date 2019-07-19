using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Application.Queries.Providers;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CreateCohortControllerTests
{
    public class WhenCallingGetConfirmProvider
    {
        [Test, MoqAutoData]
        public async Task And_ModelState_Invalid_Then_Redirect_To_Error(
            ConfirmProviderRequest request,
            string errorKey,
            string errorMessage,
            CreateCohortController controller)
        {
            controller.ModelState.AddModelError(errorKey, errorMessage);

            var result = await controller.ConfirmProvider(request) as RedirectToActionResult;

            result.ActionName.Should().Be("Error");
            result.ControllerName.Should().Be("Error");
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_View_With_Correct_Model(
            ConfirmProviderRequest request,
            GetProviderResponse response,
            ConfirmProviderViewModel viewModel,
            [Frozen] Mock<IMapper<ConfirmProviderRequest, ConfirmProviderViewModel>> mockMapper,
            [Frozen] Mock<IMediator> mockMediator,
            CreateCohortController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map(request))
                .Returns(viewModel);
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetProviderRequest>(providerRequest => providerRequest.UkPrn == request.ProviderId), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await controller.ConfirmProvider(request) as ViewResult;

            result.ViewName.Should().BeNull();
            var model = result.Model as ConfirmProviderViewModel;
            model.Should().BeSameAs(viewModel);
            model.ProviderId.Should().Be(response.ProviderId);
            model.ProviderName.Should().Be(response.ProviderName);
        }
    }
}