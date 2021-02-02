using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingEnterNewTrainingProvider
    {
        private WhenPostingEnterNewTrainingProviderFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenPostingEnterNewTrainingProviderFixture();
        }

        [Test]
        public async Task AndTheFeatureToggleIsEnable_ThenRedirectToWhoWillEnterTheDetailsPage()
        {
            var result = await _fixture.EnterNewTrainingProvider(true);

            _fixture.VerifyRedirectsToWhoWillEnterTheDetailsPage(result);
        }

        [Test]
        public async Task AndTheFeatureToggleIsDisabled_ThenRedirectToSendRequestPage()
        {
            var result = await _fixture.EnterNewTrainingProvider(false);

            _fixture.VerifyRedirectsToSendNewTrainingProviderRequestPage(result);
        }
    }

    public class WhenPostingEnterNewTrainingProviderFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly EnterNewTrainingProviderViewModel _viewModel;

        public WhenPostingEnterNewTrainingProviderFixture()
        {
            _viewModel = _autoFixture.Create<EnterNewTrainingProviderViewModel>();

            _mockMapper.Setup(m => m.Map<WhoWillEnterTheDetailsRequest>(_viewModel))
                .ReturnsAsync(new WhoWillEnterTheDetailsRequest { AccountHashedId = _viewModel.AccountHashedId, ApprenticeshipHashedId = _viewModel.ApprenticeshipHashedId, ProviderId = _viewModel.ProviderId.Value });
            _mockMapper.Setup(m => m.Map<SendNewTrainingProviderRequest>(_viewModel))
                .ReturnsAsync(new SendNewTrainingProviderRequest { AccountHashedId = _viewModel.AccountHashedId, ApprenticeshipHashedId = _viewModel.ApprenticeshipHashedId, ProviderId = _viewModel.ProviderId.Value });
        }

        public async Task<IActionResult> EnterNewTrainingProvider(bool changeProviderFeatureToggleEnabled)
        {
            _mockAuthorizationService.Setup(a => a.IsAuthorized(EmployerFeature.ChangeOfProvider))
                .Returns(changeProviderFeatureToggleEnabled);

            return await _controller.EnterNewTrainingProvider(_viewModel);
        }

        public void VerifyRedirectsToWhoWillEnterTheDetailsPage(IActionResult result)
        {
            var redirect = (RedirectToRouteResult)result;
           
            Assert.AreEqual(RouteNames.WhoWillEnterTheDetails, redirect.RouteName);
        }

        public void VerifyRedirectsToSendNewTrainingProviderRequestPage(IActionResult result)
        {
            var redirect = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.SendRequestNewTrainingProvider, redirect.RouteName);
        }
    }
}
