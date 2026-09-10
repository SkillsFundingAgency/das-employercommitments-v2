using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingApprovalRequestAlertAcknowledgeTests : ApprenticeControllerTestBase
{
    private Fixture _autoFixture;
    private ApprenticeshipApprovalRequestAlertsViewModel _viewModel;

    [SetUp]
    public void Arrange()
    {
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        MockModelMapper = new Mock<IModelMapper>();
        ApprovalsApiClient = new Mock<IApprovalsApiClient>();
        CacheStorageService = new Mock<Interfaces.ICacheStorageService>();
        ApprovalsApiClient = new Mock<IApprovalsApiClient>();

        _autoFixture = new Fixture();
        _viewModel = _autoFixture.Create<ApprenticeshipApprovalRequestAlertsViewModel>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>(),
            ApprovalsApiClient.Object);
    }

    [Test]
    public async Task AndSubmit_ThenApprovalsApiUpdateApprovalRequestAlertsIsCalled()
    {
        //Act
        await Controller.ViewApprovalRequestAlerts(_viewModel);

        //Assert
        ApprovalsApiClient.Verify(p =>
            p.UpdateApprovalRequestAlertAcknowledge(It.Is<long>(t => t == _viewModel.AccountId), It.Is<long>(t => t == _viewModel.ApprenticeshipId), It.IsAny<UpdateApprovalRequestAlertAcknowledgeRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task AndAfterAlertsAcknowledgeUpdate_ThenRedirectToApprenticeIndexPage()
    {
        //Act
        var result = await Controller.ViewApprovalRequestAlerts(_viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }
}