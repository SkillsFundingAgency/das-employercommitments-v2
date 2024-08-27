using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenCallingPostAssign
{
    [Test, MoqAutoData]
    public async Task And_Employer_Adding_Apprentices_Then_Redirect_To_Add_Apprentice(
        AssignViewModel viewModel,
        [Greedy] CohortController controller)
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
            viewModel.EncodedPledgeApplicationId,
            Origin = viewModel.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices
        });
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

        var result = await controller.Assign(viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("Apprentice");
        result.ControllerName.Should().Be("Cohort");
        result.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
    }

    [Test, MoqAutoData]
    public async Task And_Provider_Adding_Apprentices_Then_Redirect_To_Finish(
        AssignViewModel viewModel,
        CreateCohortWithOtherPartyRequest createCohortRequest,
        CommitmentsV2.Api.Types.Responses.CreateCohortResponse createCohortResponse,
        [Frozen] Mock<IModelMapper> modelMapper,
        [Frozen] Mock<ICommitmentsApiClient> apiClient,
        [Greedy] CohortController controller)
    {
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Provider;
        modelMapper.Setup(x => x.Map<CreateCohortWithOtherPartyRequest>(viewModel)).ReturnsAsync(createCohortRequest);
        apiClient.Setup(x => x.CreateCohort(createCohortRequest, It.IsAny<CancellationToken>())).ReturnsAsync(createCohortResponse).Verifiable();
        var result = await controller.Assign(viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("Finished");

        apiClient.Verify();
    }

    [Test, MoqAutoData]
    public async Task And_Unknown_Adding_Apprentices_Then_Redirect_To_Error(
        AssignViewModel viewModel,
        [Greedy] CohortController controller)
    {
        viewModel.WhoIsAddingApprentices = (WhoIsAddingApprentices)55;

        var result = await controller.Assign(viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("Error");
        result.ControllerName.Should().Be("Error");
    }

    [Test, MoqAutoData]
    public async Task And_Employer_Adding_Apprentices_And_No_Reservation_Then_Redirect_To_Reservation_Selection(
        [Frozen] Mock<ILinkGenerator> linkGenerator,
        AssignViewModel viewModel,
        [Greedy] CohortController controller)
    {
        const string reservationsUrl = "RESERVATIONS-URL";
        linkGenerator.Setup(x => x.ReservationsLink(It.IsAny<string>())).Returns(reservationsUrl);
        viewModel.ReservationId = null;
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

        var result = await controller.Assign(viewModel) as RedirectResult;

        result.Url.Should().Be(reservationsUrl);
    }

    [Test, MoqAutoData]
    public async Task And_Employer_Adding_Apprentices_And_No_Reservation_And_Has_Reached_Reservation_Limit_Then_Redirect_To_Reservation_Limit_Page(
        [Frozen] Mock<IReservationsService> reservationsService,
        AssignViewModel viewModel,
        [Greedy] CohortController controller)
    {
        reservationsService.Setup(x => x.GetAccountReservationsStatus(It.IsAny<long>(), It.IsAny<long?>()))
            .ReturnsAsync(new GetAccountReservationsStatusResponse
            {
                CanAutoCreateReservations = false, HasReachedReservationsLimit = true,
                UnallocatedPendingReservations = 0
            });
        viewModel.ReservationId = null;
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

        var result = await controller.Assign(viewModel) as RedirectToActionResult;

        result.ActionName.Should().Be("ReservationLimitReached");
        result.ControllerName.Should().Be("Cohort");
        result.VerifyRouteValue("AccountHashedId", viewModel.AccountHashedId);
    }

    [Test, MoqAutoData]
    public async Task And_Employer_Adding_Apprentices_And_No_Reservation_And_Can_Auto_Create_Reservations_Then_Redirect_To_Reservation_Selection(
        [Frozen] Mock<ILinkGenerator> linkGenerator,
        [Frozen] Mock<IReservationsService> reservationsService,
        AssignViewModel viewModel,
        [Greedy] CohortController controller)
    {
        const string reservationsUrl = "RESERVATIONS-URL";
        linkGenerator.Setup(x => x.ReservationsLink(It.IsAny<string>())).Returns(reservationsUrl);

        reservationsService.Setup(x => x.GetAccountReservationsStatus(It.IsAny<long>(), It.IsAny<long?>()))
            .ReturnsAsync(new GetAccountReservationsStatusResponse
            {
                CanAutoCreateReservations = true,
                HasReachedReservationsLimit = true,
                UnallocatedPendingReservations = 0
            });
        viewModel.ReservationId = null;
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

        var result = await controller.Assign(viewModel) as RedirectResult;

        result.Url.Should().Be(reservationsUrl);
    }

    [Test, MoqAutoData]
    public async Task And_Employer_Adding_Apprentices_And_No_Reservation_And_Has_Unallocated_Reservations_Then_Redirect_To_Reservation_Selection(
        [Frozen] Mock<ILinkGenerator> linkGenerator,
        [Frozen] Mock<IReservationsService> reservationsService,
        AssignViewModel viewModel,
        [Greedy] CohortController controller)
    {
        const string reservationsUrl = "RESERVATIONS-URL";
        linkGenerator.Setup(x => x.ReservationsLink(It.IsAny<string>())).Returns(reservationsUrl);

        reservationsService.Setup(x => x.GetAccountReservationsStatus(It.IsAny<long>(), It.IsAny<long?>()))
            .ReturnsAsync(new GetAccountReservationsStatusResponse
            {
                CanAutoCreateReservations = false,
                HasReachedReservationsLimit = false,
                UnallocatedPendingReservations = 10
            });
        viewModel.ReservationId = null;
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

        var result = await controller.Assign(viewModel) as RedirectResult;

        result.Url.Should().Be(reservationsUrl);
    }

}