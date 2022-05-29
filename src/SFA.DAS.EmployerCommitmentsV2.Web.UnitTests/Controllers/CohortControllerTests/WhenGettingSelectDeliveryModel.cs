using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenGettingSelectDeliveryModel
    {
        [Test, MoqAutoData]
        public async Task WithDeliveryModelsThenReturnsView(
            ApprenticeRequest request,
            SelectDeliveryModelViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Greedy] CohortController controller)
        {
            viewModel.DeliveryModels = new Fixture().CreateMany<DeliveryModel>().ToArray();

            mockMapper
                .Setup(mapper => mapper.Map<SelectDeliveryModelViewModel>(request))
                .ReturnsAsync(viewModel);

            var result = await controller.SelectDeliveryModel(request) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreSame("SelectDeliveryModel", result.ViewName);
            Assert.AreSame(viewModel, result.Model);
        }

        [Test, MoqAutoData]
        public async Task WithOutDeliveryModelsThenReturnsRedirect(
            ApprenticeRequest request,
            SelectDeliveryModelViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Greedy] CohortController controller)
        {
            viewModel.DeliveryModels = new DeliveryModel[0];

            mockMapper
                .Setup(mapper => mapper.Map<SelectDeliveryModelViewModel>(request))
                .ReturnsAsync(viewModel);

            var result = await controller.SelectDeliveryModel(request) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("AddDraftApprenticeship", result.ActionName);
        }
    }
}
