using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingWhoWillEnterTheDetails
    {
        private Fixture _autoFixture;
        private WhenPostingWhoWillEnterTheDetailsTestFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();

            _fixture = new WhenPostingWhoWillEnterTheDetailsTestFixture();
        }

        [Test]
        public void AndEmployerIsSelected_ThenRedirectToWhatIsTheNewStartDateRoute()
        {
            var viewModel = _autoFixture.Build<WhoWillEnterTheDetailsViewModel>()
                .With(vm => vm.EmployerWillAdd, true)
                .Create();

            var result = _fixture.WhoWillEnterTheDetails(viewModel);

            _fixture.VerifyRedirectsToWhatIsTheNewStartDateRoute(result);
        }

        [Test]
        public void AndProviderIsSelected_ThenRedirectToSendNewTrainingProviderRequest()
        {
            var viewModel = _autoFixture.Build<WhoWillEnterTheDetailsViewModel>()
                .With(vm => vm.EmployerWillAdd, false)
                .Create();

            var result = _fixture.WhoWillEnterTheDetails(viewModel);

            _fixture.VerifyRedirectsToSendRequestNewTrainingProviderRoute(result);
        }
    }

    public class WhenPostingWhoWillEnterTheDetailsTestFixture
    {

        private readonly ApprenticeController _controller;

        public WhenPostingWhoWillEnterTheDetailsTestFixture()
        {

            _controller = new ApprenticeController(Mock.Of<IModelMapper>(), 
                Mock.Of<ICookieStorageService<IndexRequest>>(), 
                Mock.Of<ICommitmentsApiClient>(), 
                Mock.Of<ILinkGenerator>(), 
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        public IActionResult WhoWillEnterTheDetails(WhoWillEnterTheDetailsViewModel viewModel)
        {
            return _controller.WhoWillEnterTheDetails(viewModel);
        }

        public void VerifyRedirectsToWhatIsTheNewStartDateRoute(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.WhatIsTheNewStartDate, redirectResult.RouteName);
        }

        public void VerifyRedirectsToSendRequestNewTrainingProviderRoute(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.SendRequestNewTrainingProvider, redirectResult.RouteName);
        }
    }
}
