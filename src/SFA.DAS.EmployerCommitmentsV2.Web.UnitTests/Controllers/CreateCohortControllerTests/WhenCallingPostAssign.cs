using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CreateCohortControllerTests
{
    public class WhenCallingPostAssign
    {
        [Test, MoqAutoData]
        public void And_ModelState_Invalid_Then_Returns_View(
            AssignViewModel viewModel,
            string errorKey,
            string errorMessage,
            CreateCohortController controller)
        {
            controller.ModelState.AddModelError(errorKey, errorMessage);

            var result = controller.Assign(viewModel) as ViewResult;

            result.Should().NotBeNull();
            result.ViewName.Should().BeNull();
            result.Model.Should().BeSameAs(viewModel);
        }

        [Test, MoqAutoData]
        public void And_Employer_Adding_Apprentices_Then_Redirect_To_Add_Apprentice(
            AssignViewModel viewModel,
            CreateCohortController controller)
        {
            var expectedRouteValues = new RouteValueDictionary(new
            {
                viewModel.AccountHashedId,
                viewModel.AccountLegalEntityPublicHashedId,
                viewModel.ReservationId,
                viewModel.StartMonthYear,
                viewModel.CourseCode,
                viewModel.ProviderId
            });
            viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

            var result = controller.Assign(viewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("AddDraftApprenticeship");
            result.ControllerName.Should().Be("CreateCohortWithDraftApprenticeship");
            result.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
        }

        [Test, MoqAutoData]
        public void And_Provider_Adding_Apprentices_Then_Redirect_To_Message(
            AssignViewModel viewModel,
            CreateCohortController controller)
        {
            var expectedRouteValues = new RouteValueDictionary(new
            {
                viewModel.AccountHashedId,
                viewModel.AccountLegalEntityPublicHashedId,
                viewModel.ReservationId,
                viewModel.StartMonthYear,
                viewModel.CourseCode,
                viewModel.ProviderId
            });
            viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Provider;

            var result = controller.Assign(viewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Message");
            result.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
        }

        [Test, MoqAutoData]
        public void And_Unknown_Adding_Apprentices_Then_Redirect_To_Error(
            AssignViewModel viewModel,
            CreateCohortController controller)
        {
            viewModel.WhoIsAddingApprentices = (WhoIsAddingApprentices)55;

            var result = controller.Assign(viewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Error");
            result.ControllerName.Should().Be("Error");
        }
    }
}