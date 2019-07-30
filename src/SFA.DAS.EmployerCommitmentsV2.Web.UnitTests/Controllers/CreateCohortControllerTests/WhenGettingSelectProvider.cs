using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CreateCohortControllerTests
{
    [TestFixture]
    public class WhenGettingSelectProvider
    {
        [Test, MoqAutoData]
        public void ThenMapsTheRequestToViewModel(
            SelectProviderRequest request,
            SelectProviderViewModel viewModel,
            [Frozen] Mock<IMapper<SelectProviderRequest, SelectProviderViewModel>> mockMapper,
            CreateCohortController controller)
        {
            controller.SelectProvider(request);

            mockMapper.Verify(x => x.Map(It.IsAny<SelectProviderRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public void ThenReturnsView(
            SelectProviderRequest request,
            SelectProviderViewModel viewModel,
            [Frozen] Mock<IMapper<SelectProviderRequest, SelectProviderViewModel>> mockMapper,
            CreateCohortController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map(request))
                .Returns(viewModel);

            var result = controller.SelectProvider(request) as ViewResult;

            Assert.Null(result.ViewName);
            Assert.AreSame(viewModel, result.Model);
        }

        [Test, MoqAutoData]
        public void ThenIfModelIsInvalidRedirectToErrorPage(
            SelectProviderRequest request,
            SelectProviderViewModel viewModel,
            CreateCohortController controller)
        {
            controller.ModelState.AddModelError(nameof(request.EmployerAccountLegalEntityPublicHashedId), "Must be set");

            var result = controller.SelectProvider(request) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.AreEqual("Error", result.ControllerName);
            Assert.AreEqual("Error", result.ActionName);
            Assert.AreEqual(400, (int) result.RouteValues["StatusCode"]);
        }
    }
}
