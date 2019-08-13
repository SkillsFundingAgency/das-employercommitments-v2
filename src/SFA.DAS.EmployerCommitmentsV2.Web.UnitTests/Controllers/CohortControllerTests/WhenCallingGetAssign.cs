using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenCallingGetAssign
    {
        [Test, MoqAutoData]
        public void And_ModelState_Invalid_Then_Redirect_To_Error(
            AssignRequest request,
            string errorKey,
            string errorMessage,
            CohortController controller)
        {
            controller.ModelState.AddModelError(errorKey, errorMessage);

            var result = controller.Assign(request) as RedirectToActionResult;

            result.ActionName.Should().Be("Error");
            result.ControllerName.Should().Be("Error");
        }

        [Test, MoqAutoData]
        public void Then_Returns_View_With_Correct_Model(
            AssignRequest request,
            AssignViewModel viewModel,
            [Frozen] Mock<IMapper<AssignRequest, AssignViewModel>> mockMapper,
            CohortController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map(request))
                .Returns(viewModel);

            var result = controller.Assign(request) as ViewResult;

            result.ViewName.Should().BeNull();
            var model = result.Model as AssignViewModel;
            model.Should().BeSameAs(viewModel);
        }
    }
}