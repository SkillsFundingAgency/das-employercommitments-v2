using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingConfirmDeleteDetails
{
    [Test]
    public async Task ThenShouldCallCohortDeleteWithCorrectParams()
    {
        var fixture = new WhenPostingConfirmDeleteDetailsTestFixture().SetConfirmDelete(true);
        await fixture.CohortController.Delete(fixture.AuthenticationService.Object, fixture.ConfirmDeleteViewModel);
        fixture.VerifyDeleteCohortWasCalledCorrectly();
    }

    [Test]
    public async Task ThenShouldCallCohortDeleteAndDirectToBingoPage()
    {
        var fixture = new WhenPostingConfirmDeleteDetailsTestFixture().SetConfirmDelete(true);
        var result = await fixture.CohortController.Delete(fixture.AuthenticationService.Object, fixture.ConfirmDeleteViewModel);
        fixture.VerifyRedirectsToReviewPage(result);
    }

    [Test]
    public async Task ThenShouldRedirectToCohortDetails()
    {
        var fixture = new WhenPostingConfirmDeleteDetailsTestFixture().SetConfirmDelete(false);
        var result = await fixture.CohortController.Delete(fixture.AuthenticationService.Object, fixture.ConfirmDeleteViewModel);
        fixture.VerifyRedirectsToCohortDetailsPage(result);
    }

    public class WhenPostingConfirmDeleteDetailsTestFixture
    {
        public ConfirmDeleteViewModel ConfirmDeleteViewModel;
        public UserInfo UserInfo;
        public CohortController CohortController { get; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; }
        public Mock<IAuthenticationService> AuthenticationService { get; }

        public WhenPostingConfirmDeleteDetailsTestFixture()
        {
            var autoFixture = new Fixture();
            ConfirmDeleteViewModel = autoFixture.Create<ConfirmDeleteViewModel>();
            UserInfo = autoFixture.Create<UserInfo>();

            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            AuthenticationService = new Mock<IAuthenticationService>();
            AuthenticationService.Setup(x => x.UserInfo).Returns(UserInfo);

            CohortController = new CohortController(CommitmentsApiClient.Object,
                Mock.Of<ILogger<CohortController>>(),
                Mock.Of<ILinkGenerator>(),
                Mock.Of<IModelMapper>(),
                Mock.Of<IEncodingService>(),
                Mock.Of<IApprovalsApiClient>(), 
                Mock.Of<IReservationsService>());
        }

        public WhenPostingConfirmDeleteDetailsTestFixture SetConfirmDelete(bool confirmDeletion)
        {
            ConfirmDeleteViewModel.ConfirmDeletion = confirmDeletion;
            return this;
        }

        public void VerifyDeleteCohortWasCalledCorrectly()
        {
            CommitmentsApiClient.Verify(x => x.DeleteCohort(ConfirmDeleteViewModel.CohortId, UserInfo, It.IsAny<CancellationToken>()));
        }

        public void VerifyRedirectsToReviewPage(IActionResult result)
        {
            var redirect = (RedirectToActionResult)result;
            Assert.Multiple(() =>
            {
                Assert.That(redirect.ActionName, Is.EqualTo($"Review"));
                Assert.That(redirect.RouteValues["AccountHashedId"], Is.EqualTo(ConfirmDeleteViewModel.AccountHashedId));
            });
        }
        public void VerifyRedirectsToCohortDetailsPage(IActionResult result)
        {
            var redirect = (RedirectToActionResult)result;
            Assert.Multiple(() =>
            {
                Assert.That(redirect.ActionName?.ToLower(), Is.EqualTo("details"));
                Assert.That(redirect.RouteValues["CohortReference"], Is.EqualTo(ConfirmDeleteViewModel.CohortReference));
                Assert.That(redirect.RouteValues["AccountHashedId"], Is.EqualTo(ConfirmDeleteViewModel.AccountHashedId));
            });
        }
    }
}