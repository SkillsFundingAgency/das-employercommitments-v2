using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingConfirmEditApprenticeshipTests : ApprenticeControllerTestBase
{
    private const string EditApprenticeUpdated = "You have updated apprentice details";
    private const string FlashMessageBody = "FlashMessageBody";
    private const string FlashMessageLevel = "FlashMessageLevel";
    private EditApprenticeshipResponse _response;

    [SetUp]
    public void Arrange()
    {
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        MockModelMapper = new Mock<IModelMapper>();
        CacheStorageService = new Mock<ICacheStorageService>();

        _response = new EditApprenticeshipResponse { NeedReapproval = true };

        MockCommitmentsApiClient.Setup(x => x.EditApprenticeship(It.IsAny<EditApprenticeshipApiRequest>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(_response));

        Controller = new ApprenticeController(MockModelMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>());
        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public async Task AndTheEditApprenticeship_IsConfirmed_WithIntermediateUpdate_ThenRedirectToApprenticeshipDetails(ConfirmEditApprenticeshipViewModel viewModel)
    {
        viewModel.ConfirmChanges = true;
        var result = await Controller.ConfirmEditApprenticeship(viewModel);

        var redirect = result.VerifyReturnsRedirectToActionResult();
        
        Assert.Multiple(() =>
        {
            Assert.That(redirect.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));
            Assert.That(Controller.TempData.Values.Contains($"Your suggested changes have been sent to {viewModel.ProviderName} for approval."), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
        });
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_IsEdit_WithImmediateUpdate_ThenRedirectToApprenticeshipDetails(ConfirmEditApprenticeshipViewModel viewModel)
    {
        _response.NeedReapproval = false;
        var result = await Controller.ConfirmEditApprenticeship(viewModel);

        var redirect = result.VerifyReturnsRedirectToActionResult();
        Assert.Multiple(() =>
        {
            Assert.That(redirect.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));
            Assert.That(Controller.TempData.Values.Contains(EditApprenticeUpdated), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
        });
    }

    [Test, MoqAutoData]
    public async Task AndTheEditApprenticeship_IsConfirmed_CommitmentApi_IsCalled(ConfirmEditApprenticeshipViewModel viewModel)
    {
        //Arrange
        viewModel.ConfirmChanges = true;

        //Act
        await Controller.ConfirmEditApprenticeship(viewModel);

        //Assert
        MockCommitmentsApiClient.Verify(x => x.EditApprenticeship(It.IsAny<EditApprenticeshipApiRequest>(), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndTheEditApprenticeship_IsNotConfirmed_CommitmentApi_IsNotCalled(ConfirmEditApprenticeshipViewModel viewModel)
    {
        //Arrange
        viewModel.ConfirmChanges = false;

        //Act
        await Controller.ConfirmEditApprenticeship(viewModel);

        //Assert
        MockCommitmentsApiClient.Verify(x => x.EditApprenticeship(It.IsAny<EditApprenticeshipApiRequest>(), CancellationToken.None), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task AndTheEditApprenticeship_IsConfirmed_and_Mapper_IsCalled(ConfirmEditApprenticeshipViewModel viewModel)
    {
        //Arrange
        viewModel.ConfirmChanges = true;

        //Act
        await Controller.ConfirmEditApprenticeship(viewModel);

        //Assert
        MockModelMapper.Verify(x => x.Map<EditApprenticeshipApiRequest>(viewModel), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndTheEditApprenticeship_IsNotConfirmed__and_Mapper_IsNotCalled(ConfirmEditApprenticeshipViewModel viewModel)
    {
        //Arrange
        viewModel.ConfirmChanges = false;

        //Act
        await Controller.ConfirmEditApprenticeship(viewModel);

        //Assert
        MockModelMapper.Verify(x => x.Map<EditApprenticeshipApiRequest>(viewModel), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task AndTheEditApprenticeship_IsNotConfirmed_ThenRedirectsToApprenticeshipDetails(ConfirmEditApprenticeshipViewModel viewModel)
    {
        //Arrange
        viewModel.ConfirmChanges = false;

        //Act
        var result = await Controller.ConfirmEditApprenticeship(viewModel) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }
}