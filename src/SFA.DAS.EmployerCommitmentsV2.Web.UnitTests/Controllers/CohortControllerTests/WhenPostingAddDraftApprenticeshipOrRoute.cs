using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using CreateCohortResponse = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.CreateCohortResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingAddDraftApprenticeshipOrRoute
{
    [Test, MoqAutoData]
    public async Task WithoutEditThenReturnsRedirect(
        CreateCohortApimRequest request,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
        [Frozen] Mock<IApprovalsApiClient> approvalsApiClient)
    {            
        var viewModel = new ApprenticeViewModel();
        var autoFixture = new Fixture();

        var createCohortResponse = autoFixture.Create<CreateCohortResponse>();
        var getDraftApprenticeshipsResponse = autoFixture.Build<GetDraftApprenticeshipsResponse>()
            .With(x => x.DraftApprenticeships, new DraftApprenticeshipDto[1] { autoFixture.Create<DraftApprenticeshipDto>() })
            .Create();

        approvalsApiClient.Setup(x => x.CreateCohort(It.IsAny<CreateCohortApimRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createCohortResponse);
        commitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getDraftApprenticeshipsResponse);

        mockMapper.Setup(mapper => mapper.Map<CreateCohortApimRequest>(viewModel))
            .ReturnsAsync(request);

        var controller = new CohortController(
            commitmentsApiClient.Object,
            Mock.Of<ILogger<CohortController>>(),
            Mock.Of<ILinkGenerator>(),
            mockMapper.Object,
            Mock.Of<IEncodingService>(),
            approvalsApiClient.Object,
            Mock.Of<IReservationsService>());

        var result = await controller.AddDraftApprenticeshipOrRoute("", "", viewModel) as RedirectToActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ActionName, Is.EqualTo("SelectOption"));
            Assert.That(result.ControllerName, Is.EqualTo("DraftApprenticeship"));
        });
    }

    [Test, MoqAutoData]
    public async Task WithEditCourseThenReturnsRedirect(
        ApprenticeRequest request,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        var viewModel = new ApprenticeViewModel();
        controller.TempData = new Mock<ITempDataDictionary>().Object;

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeRequest>(viewModel))
            .ReturnsAsync(request);

        var result = await controller.AddDraftApprenticeshipOrRoute("Edit", "", viewModel) as RedirectToActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("SelectCourse"));
    }

    [Test, MoqAutoData]
    public async Task WithEditDeliveryModelThenReturnsRedirect(
        ApprenticeRequest request,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        var viewModel = new ApprenticeViewModel();
        controller.TempData = new Mock<ITempDataDictionary>().Object;

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeRequest>(viewModel))
            .ReturnsAsync(request);

        var result = await controller.AddDraftApprenticeshipOrRoute("", "Edit", viewModel) as RedirectToActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("SelectDeliveryModel"));
    }
}