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
            mockMapper.Setup(x => x.Map(request)).Returns(viewModel);

            controller.SelectProvider(request);

            mockMapper.Verify(x => x.Map(It.IsAny<SelectProviderRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public void ThenReturnsView(SelectProviderRequest request, CreateCohortController controller)
        {
            var viewName = "SelectProvider";

            var result = controller.SelectProvider(request) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(viewName, result.ViewName);

        }
    }
}
