using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenPostingConfirmDeleteDetails
    {
        [Test]
        public async Task ThenShouldCallCohortDeleteWithCorrectParams()
        {
            var f = new WhenPostingConfirmDeleteDetailsTestFixture().SetConfirmDelete(true);
            await f.CohortController.Delete(f.AuthenticationService.Object, f.ConfirmDeleteViewModel);
            f.VerifyDeleteCohortWasCalledCorrectly();
        }

        [Test]
        public async Task ThenShouldCallCohortDeleteAndDirectToBingoPage()
        {
            var f = new WhenPostingConfirmDeleteDetailsTestFixture().SetConfirmDelete(true);
            var result = await f.CohortController.Delete(f.AuthenticationService.Object, f.ConfirmDeleteViewModel);
            f.VerifyRedirectsToBingoPage(result);
        }

        [Test]
        public async Task ThenShouldRedirectToCohortDetails()
        {
            var f = new WhenPostingConfirmDeleteDetailsTestFixture().SetConfirmDelete(false);
            var result = await f.CohortController.Delete(f.AuthenticationService.Object, f.ConfirmDeleteViewModel);
            f.VerifyRedirectsToCohortDetailsPage(result);
        }

        public class WhenPostingConfirmDeleteDetailsTestFixture
        {
            public ConfirmDeleteViewModel ConfirmDeleteViewModel;
            public UserInfo UserInfo;
            public CohortController CohortController { get; }
            public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; }
            public Mock<IAuthenticationService> AuthenticationService { get; }
            public Mock<ILinkGenerator> LinkGenerator { get; }

            public WhenPostingConfirmDeleteDetailsTestFixture()
            {
                var autoFixture = new Fixture();
                ConfirmDeleteViewModel = autoFixture.Create<ConfirmDeleteViewModel>();
                UserInfo = autoFixture.Create<UserInfo>();

                CommitmentsApiClient = new Mock<ICommitmentsApiClient>();

                AuthenticationService = new Mock<IAuthenticationService>();
                AuthenticationService.Setup(x => x.UserInfo).Returns(UserInfo);

                LinkGenerator = new Mock<ILinkGenerator>();
                LinkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns<string>(s => s); 

                CohortController = new CohortController(CommitmentsApiClient.Object,
                    Mock.Of<ILogger<CohortController>>(),
                    LinkGenerator.Object,
                    Mock.Of<IModelMapper>(),
                    Mock.Of<IAuthorizationService>());
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

            public void VerifyRedirectsToBingoPage(IActionResult result)
            {
                var redirect = (RedirectToActionResult)result;
                Assert.AreEqual($"Cohorts", redirect.ActionName);
                Assert.AreEqual(ConfirmDeleteViewModel.AccountHashedId, redirect.RouteValues["AccountHashedId"]);
            }
            public void VerifyRedirectsToCohortDetailsPage(IActionResult result)
            {
                var redirect = (RedirectToActionResult)result;
                Assert.AreEqual("details", redirect.ActionName?.ToLower());
                Assert.AreEqual(ConfirmDeleteViewModel.CohortReference, redirect.RouteValues["CohortReference"]);
                Assert.AreEqual(ConfirmDeleteViewModel.AccountHashedId, redirect.RouteValues["AccountHashedId"]);
            }
        }
    }
}