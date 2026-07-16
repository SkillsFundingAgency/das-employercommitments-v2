using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingChangeStatus : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockModelMapper = new Mock<IModelMapper>();
        MockCookieStorageService = new Mock<Interfaces.ICookieStorageService<IndexRequest>>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        CacheStorageService = new Mock<ICacheStorageService>();
        ApprovalsApiClient =  new Mock<IApprovalsApiClient>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            MockCookieStorageService.Object,
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>(),
            ApprovalsApiClient.Object);
    }

    [Test, MoqAutoData]
    public void AndStopIsSelectedForLiveApprentice_ThenRedirectToWhyStopApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
    {
        viewModel.SelectedStatusChange = ChangeStatusType.Stop;
        viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.Live;

        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        response.ActionName.Should().Be("WhyStopApprenticeship");
    }

    [Test, MoqAutoData]
    public void AndGoBackIsSelected_ThenRedirectToWhyStopApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
    {
        viewModel.SelectedStatusChange = ChangeStatusType.GoBack;

        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        response.ActionName.Should().Be("ApprenticeshipDetails");
    }

    [Test, MoqAutoData]
    public void AndStopIsSelected_ThenRedirectToV1WhenToApplyStopAction(ChangeStatusRequestViewModel viewModel)
    {
        viewModel.SelectedStatusChange = ChangeStatusType.Stop;
        viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.Live;

        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        response.ActionName.Should().Be("WhyStopApprenticeship");
    }

    [Test, MoqAutoData]
    public void AndStopIsSelected_ThenRedirectToV1WhenToHasTheApprenticeBeenMadeRedundant(ChangeStatusRequestViewModel viewModel)
    {
        viewModel.SelectedStatusChange = ChangeStatusType.Stop;
        viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart;

        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        response.ActionName.Should().Be("HasTheApprenticeBeenMadeRedundant");
    }

    [Test, MoqAutoData]
    public void AndPauseIsSelected_ThenRedirectToApprenticeshipDetailsAction(ChangeStatusRequestViewModel viewModel)
    {
        viewModel.SelectedStatusChange = ChangeStatusType.GoBack;

        var response = Controller.ChangeStatus(viewModel) as RedirectToActionResult;

        response.ActionName.Should().Be("ApprenticeshipDetails");
    }
}
