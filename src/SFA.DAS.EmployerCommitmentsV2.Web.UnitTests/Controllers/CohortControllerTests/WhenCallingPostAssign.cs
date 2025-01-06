using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
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
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = viewModel.ApprenticeshipSessionKey.Value;
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
            .ReturnsAsync(cacheModel);

        var expectedRouteValues = new RouteValueDictionary(new
        {
            cacheModel.AccountHashedId,
            cacheModel.AccountLegalEntityHashedId,
            cacheModel.ReservationId,
            cacheModel.StartMonthYear,
            cacheModel.CourseCode,
            cacheModel.ProviderId,
            cacheModel.TransferSenderId,
            cacheModel.EncodedPledgeApplicationId,
            cacheModel.ApprenticeshipSessionKey
        });
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

        var result = await controller.Assign(viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("Apprentice");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);
    }

    [Test, MoqAutoData]
    public async Task And_FundingType_Is_AdditionalReservations_And_Employer_Adding_Apprentices_Then_Redirect_To_Add_Apprentice(
        AssignViewModel viewModel,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = viewModel.ApprenticeshipSessionKey.Value;
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
            .ReturnsAsync(cacheModel);

        viewModel.ReservationId = null;
        viewModel.FundingType = FundingType.AdditionalReservations;
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

        var expectedRouteValues = new RouteValueDictionary(new
        {
            cacheModel.AccountHashedId,
            cacheModel.ApprenticeshipSessionKey
        });

        var result = await controller.Assign(viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("Apprentice");
        result.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
    }

    [Test, MoqAutoData]
    public async Task And_Provider_Adding_Apprentices_Then_Redirect_To_Finish(
        AssignViewModel viewModel,
        CreateCohortWithOtherPartyRequest createCohortRequest,
        CreateCohortResponse createCohortResponse,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IModelMapper> modelMapper,
        [Frozen] Mock<ICommitmentsApiClient> apiClient,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = viewModel.ApprenticeshipSessionKey.Value;

        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Provider;
        modelMapper.Setup(x => x.Map<CreateCohortWithOtherPartyRequest>(cacheModel)).ReturnsAsync(createCohortRequest);

        apiClient.Setup(x => x.CreateCohort(createCohortRequest, It.IsAny<CancellationToken>())).ReturnsAsync(createCohortResponse).Verifiable();
        var result = await controller.Assign(viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("Finished");

        apiClient.Verify();
    }

    [Test, MoqAutoData]
    public async Task And_Unknown_Adding_Apprentices_Then_Redirect_To_Error(
        AssignViewModel viewModel,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {

        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

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
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.ReservationId = null;

        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

        const string reservationsUrl = "RESERVATIONS-URL";
        linkGenerator.Setup(x => x.ReservationsLink(It.IsAny<string>())).Returns(reservationsUrl);
        viewModel.ReservationId = null;
        viewModel.FundingType = null;
        viewModel.WhoIsAddingApprentices = WhoIsAddingApprentices.Employer;

        var result = await controller.Assign(viewModel) as RedirectResult;

        result.Url.Should().Be(reservationsUrl);
    }
}