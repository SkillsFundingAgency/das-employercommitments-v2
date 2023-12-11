using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingReviewApprenticeshipUpdatesTests
{
    private WhenPostingReviewApprenticeshipUpdatesTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenPostingReviewApprenticeshipUpdatesTestsFixture();
    }

    [Test]
    public async Task Verify_AcceptApprenticeshipUpdates_IsCalled_When_UserSelectsTo_ApproveChanges()
    {
        _fixture.ViewModel.ApproveChanges = true;
        await _fixture.ReviewApprenticeshipUpdates();
        _fixture.VerifyAcceptApprenticeshipUpdatesApiIsCalled();
    }

    [Test]
    public async Task Verify_RejectApprenticeshipUpdates_IsCalled_When_UserSelectsTo_ApproveChanges()
    {
        _fixture.ViewModel.ApproveChanges = false;
        await _fixture.ReviewApprenticeshipUpdates();
        _fixture.VerifyRejectApprenticeshipUpdatesApiIsCalled();
    }

    [Test]
    public async Task VerifyIsRedirectedToApprenticeshipDetails()
    {
        _fixture.ViewModel.ApproveChanges = false;
        var response = await _fixture.ReviewApprenticeshipUpdates() as RedirectToActionResult;
        Assert.That(response.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }
}

public class WhenPostingReviewApprenticeshipUpdatesTestsFixture : ApprenticeControllerTestFixtureBase
{
    private const string FlashMessageBodyTempDataKey = "FlashMessageBody";
    private const string FlashMessageLevelTempDataKey = "FlashMessageLevel";
    private const string FlashMessageTitleTempDataKey = "FlashMessageTitle";
    private const string ChangesApprovedMessage = "Changes approved";
    private const string ChangesRejectedMessage = "Changes rejected";

    public ReviewApprenticeshipUpdatesViewModel ViewModel { get; set; }

    private Mock<IAuthenticationService> AuthenticationService { get; }

    public WhenPostingReviewApprenticeshipUpdatesTestsFixture()
    {
        ViewModel = new ReviewApprenticeshipUpdatesViewModel { ApprenticeshipId = 1, AccountId = 1 };
        Controller.TempData = new TempDataDictionary(Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());

        var autoFixture = new Fixture();
        var userInfo = autoFixture.Create<UserInfo>();
        AuthenticationService = new Mock<IAuthenticationService>();
        AuthenticationService.Setup(x => x.UserInfo).Returns(userInfo);
    }

    public async Task<IActionResult> ReviewApprenticeshipUpdates()
    {
        return await Controller.ReviewApprenticeshipUpdates(AuthenticationService.Object, ViewModel);
    }

    internal void VerifyAcceptApprenticeshipUpdatesApiIsCalled()
    {
        Assert.Multiple(() =>
        {
            MockCommitmentsApiClient
                .Verify(
                    x => x.AcceptApprenticeshipUpdates(ViewModel.ApprenticeshipId,
                        It.Is<AcceptApprenticeshipUpdatesRequest>(o => o.UserInfo != null),
                        It.IsAny<CancellationToken>()),
                    Times.Once());

            Assert.That(Controller.TempData.Values.Contains(ChangesApprovedMessage), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBodyTempDataKey), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevelTempDataKey), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitleTempDataKey), Is.True);
        });
    }

    internal void VerifyRejectApprenticeshipUpdatesApiIsCalled()
    {
        Assert.Multiple(() =>
        {
            MockCommitmentsApiClient.Verify(
                x => x.RejectApprenticeshipUpdates(ViewModel.ApprenticeshipId,
                    It.Is<RejectApprenticeshipUpdatesRequest>(o => o.UserInfo != null), It.IsAny<CancellationToken>()),
                Times.Once());
            Assert.That(Controller.TempData.Values.Contains(ChangesRejectedMessage), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBodyTempDataKey), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevelTempDataKey), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitleTempDataKey), Is.True);
        });
    }
}