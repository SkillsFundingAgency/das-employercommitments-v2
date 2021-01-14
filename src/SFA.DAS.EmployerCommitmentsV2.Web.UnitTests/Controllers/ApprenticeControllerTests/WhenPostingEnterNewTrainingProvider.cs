using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.EmployerUrlHelper;
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

    public class WhenPostingEnterNewTrainingProviderFixture
    {
        private readonly Mock<IModelMapper> _mockMapper;
        private readonly Mock<IAuthorizationService> _mockAuthorizationService;

        private readonly EnterNewTrainingProviderViewModel _viewModel;

        private readonly ApprenticeController _controller;

        public WhenPostingEnterNewTrainingProviderFixture()
        {
            var autoFixture = new Fixture();

            _viewModel = autoFixture.Create<EnterNewTrainingProviderViewModel>();

            _mockMapper = new Mock<IModelMapper>();
            _mockMapper.Setup(m => m.Map<WhoWillEnterTheDetailsRequest>(_viewModel))
                .ReturnsAsync(new WhoWillEnterTheDetailsRequest { AccountHashedId = _viewModel.AccountHashedId, ApprenticeshipHashedId = _viewModel.ApprenticeshipHashedId, ProviderId = _viewModel.ProviderId });
            _mockMapper.Setup(m => m.Map<SendNewTrainingProviderRequest>(_viewModel))
                            .ReturnsAsync(new SendNewTrainingProviderRequest { AccountHashedId = _viewModel.AccountHashedId, ApprenticeshipHashedId = _viewModel.ApprenticeshipHashedId, ProviderId = _viewModel.ProviderId });

            _mockAuthorizationService = new Mock<IAuthorizationService>();

            _controller = new ApprenticeController(_mockMapper.Object, 
                Mock.Of<ICookieStorageService<IndexRequest>>(), 
                Mock.Of<ICommitmentsApiClient>(), 
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>(),
                _mockAuthorizationService.Object);
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
