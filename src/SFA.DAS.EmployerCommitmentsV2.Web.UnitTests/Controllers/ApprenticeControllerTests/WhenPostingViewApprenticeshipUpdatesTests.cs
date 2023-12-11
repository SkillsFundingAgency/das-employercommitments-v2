using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingViewApprenticeshipUpdatesTests
{
    private WhenPostingViewApprenticeshipUpdatesTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenPostingViewApprenticeshipUpdatesTestsFixture();
    }   

    [Test]
    public async Task Verify_UndoApprenticeshipUpdates_IsCalled_When_UserSelectsTo_UndoChanges()
    {
        _fixture.ViewModel.UndoChanges = true;
        await _fixture.ViewApprenticeshipUpdates();
        _fixture.Verify_UndoApprenticeshipUpdatesApi_Is_Called();
    }

    [Test]
    public async Task Verify_UndoApprenticeshipUpdates_IsNotCalled_When_UserSelects_Not_To_UndoChanges()
    {
        _fixture.ViewModel.UndoChanges = false;
        await _fixture.ViewApprenticeshipUpdates();
        _fixture.Verify_UndoApprenticeshipUpdatesApi_IsNot_Called();
    }


    [Test]
    public async Task VerifyIsRedirectedToApprenticeshipDetails()
    {
        _fixture.ViewModel.UndoChanges = false;
        var response = await _fixture.ViewApprenticeshipUpdates() as RedirectToActionResult;
        Assert.That(response.ActionName, Is.EqualTo("ApprenticeshipDetails"));  
    }
}

public class WhenPostingViewApprenticeshipUpdatesTestsFixture : ApprenticeControllerTestFixtureBase
{
    private Mock<IAuthenticationService> AuthenticationService { get; }
    private UserInfo UserInfo;

    public ViewApprenticeshipUpdatesViewModel ViewModel { get; set; }
    public WhenPostingViewApprenticeshipUpdatesTestsFixture()
    {
        ViewModel = new ViewApprenticeshipUpdatesViewModel { ApprenticeshipId = 1, AccountId = 1 };
        Controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());

        var autoFixture = new Fixture();
        UserInfo = autoFixture.Create<UserInfo>();
        AuthenticationService = new Mock<IAuthenticationService>();
        AuthenticationService.Setup(x => x.UserInfo).Returns(UserInfo);
    }

    public async Task<IActionResult> ViewApprenticeshipUpdates()
    {
        return await Controller.ViewApprenticeshipUpdates(AuthenticationService.Object, ViewModel);
    }     

    internal void Verify_UndoApprenticeshipUpdatesApi_Is_Called()
    {
        MockCommitmentsApiClient.Verify(x => x.UndoApprenticeshipUpdates(ViewModel.ApprenticeshipId, It.Is<UndoApprenticeshipUpdatesRequest>(o => o.UserInfo != null), It.IsAny<CancellationToken>()), Times.Once());
    }

    internal void Verify_UndoApprenticeshipUpdatesApi_IsNot_Called()
    {
        MockCommitmentsApiClient.Verify(x => x.UndoApprenticeshipUpdates(ViewModel.ApprenticeshipId, It.IsAny<UndoApprenticeshipUpdatesRequest>(), It.IsAny<CancellationToken>()), Times.Never());
    }
}