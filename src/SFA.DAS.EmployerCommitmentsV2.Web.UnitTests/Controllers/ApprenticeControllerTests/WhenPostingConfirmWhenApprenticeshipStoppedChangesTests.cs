using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingConfirmWhenApprenticeshipStoppedChangesTests : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        MockModelMapper = new Mock<IModelMapper>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            Mock.Of<ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            Mock.Of<ILogger<ApprenticeController>>());

        Controller.TempData =
            new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_StoppedIsWrong_ThenRedirectToApprenticeshipDetails(
        ConfirmWhenApprenticeshipStoppedViewModel viewModel)
    {
        viewModel.IsCorrectStopDate = false;

        var result = await Controller.ConfirmWhenApprenticeshipStopped(viewModel) as RedirectToActionResult;

        var redirect = result.VerifyReturnsRedirectToActionResult();

        Assert.Multiple(() =>
        {
            Assert.That(redirect.ActionName, Is.EqualTo("EditStopDate"));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));
            MockCommitmentsApiClient.Verify(
                x => x.ResolveOverlappingTrainingDateRequest(
                    It.IsAny<ResolveApprenticeshipOverlappingTrainingDateRequest>(), CancellationToken.None),
                Times.Never);
        });
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_StoppedIsCorrect_ThenRedirectToApprenticeshipDetails(
        ConfirmWhenApprenticeshipStoppedViewModel viewModel)
    {
        //Arrange
        viewModel.IsCorrectStopDate = true;
        viewModel.StopDate = new DateTime(2022, 1, 1);

        //Act
        var result = await Controller.ConfirmWhenApprenticeshipStopped(viewModel) as RedirectToActionResult;

        //Assert
        var redirect = result.VerifyReturnsRedirectToActionResult();

        Assert.Multiple(() =>
        {
            Assert.That(Controller.TempData.Values.Contains("Current stop date confirmed"), Is.True);
            Assert.That(Controller.TempData.Values.Contains("January 2022"), Is.True);
            Assert.That(redirect.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));
            MockCommitmentsApiClient
                .Verify(
                    x => x.ResolveOverlappingTrainingDateRequest(
                        It.Is<ResolveApprenticeshipOverlappingTrainingDateRequest>(x =>
                            x.ResolutionType ==
                            OverlappingTrainingDateRequestResolutionType.ApprenticeshipStopDateIsCorrect),
                        CancellationToken.None), Times.Once);
        });
    }
}