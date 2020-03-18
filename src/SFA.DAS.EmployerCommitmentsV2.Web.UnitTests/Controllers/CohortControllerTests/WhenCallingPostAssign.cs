using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenCallingPostAssign
    {
        [Test, MoqAutoData]
        public void And_Employer_Adding_Apprentices_Then_Redirect_To_Add_Apprentice(
            AssignViewModel viewModel,
            CohortController controller)
        {
            var expectedRouteValues = new RouteValueDictionary(new
            {
                viewModel.AccountHashedId,
                viewModel.AccountLegalEntityHashedId,
                viewModel.ReservationId,
                viewModel.StartMonthYear,
                viewModel.CourseCode,
                viewModel.ProviderId,
                viewModel.TransferSenderId,
                Origin = viewModel.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices
            });
            viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

            var result = controller.Assign(viewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Apprentice");
            result.ControllerName.Should().Be("Cohort");
            result.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
        }

        [Test, MoqAutoData]
        public void And_Provider_Adding_Apprentices_Then_Redirect_To_Message(
            AssignViewModel viewModel,
            CohortController controller)
        {
            var expectedRouteValues = new RouteValueDictionary(new
            {
                viewModel.AccountHashedId,
                viewModel.AccountLegalEntityHashedId,
                viewModel.ReservationId,
                viewModel.StartMonthYear,
                viewModel.CourseCode,
                viewModel.ProviderId,
                viewModel.TransferSenderId,
                Origin = viewModel.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices
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
            CohortController controller)
        {
            viewModel.WhoIsAddingApprentices = (WhoIsAddingApprentices)55;

            var result = controller.Assign(viewModel) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.ActionName.Should().Be("Error");
            result.ControllerName.Should().Be("Error");
        }

        [Test, MoqAutoData]
        public void And_Employer_Adding_Apprentices_And_No_Reservation_Then_Redirect_To_Reservation_Selection(
            [Frozen] Mock<ILinkGenerator> linkGenerator,
            AssignViewModel viewModel,
            CohortController controller)
        {
            const string reservationsUrl = "RESERVATIONS-URL";
            linkGenerator.Setup(x => x.ReservationsLink(It.IsAny<string>())).Returns(reservationsUrl);
            viewModel.ReservationId = null;
            viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

            var result = controller.Assign(viewModel) as RedirectResult;

            result.Url.Should().Be(reservationsUrl);
        }
    }
}