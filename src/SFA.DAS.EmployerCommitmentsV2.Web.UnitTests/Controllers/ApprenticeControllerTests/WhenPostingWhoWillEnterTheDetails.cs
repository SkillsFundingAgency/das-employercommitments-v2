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
using System.Threading.Tasks;

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
        public async Task AndEmployerIsSelected_ThenRedirectToWhatIsTheNewStartDateRoute()
        {
            var viewModel = _autoFixture.Build<WhoWillEnterTheDetailsViewModel>()
                .With(vm => vm.EmployerWillAdd, true)
                .Create();

            var result = await _fixture.WhoWillEnterTheDetails(viewModel);

            _fixture.VerifyRedirectsToWhatIsTheNewStartDateRoute(result);
        }

        [Test]
        public async Task AndProviderIsSelected_ThenRedirectToSendNewTrainingProviderRequest()
        {
            var viewModel = _autoFixture.Build<WhoWillEnterTheDetailsViewModel>()
                .With(vm => vm.EmployerWillAdd, false)
                .Create();

            var result = await _fixture.WhoWillEnterTheDetails(viewModel);

            _fixture.VerifyRedirectsToSendRequestNewTrainingProviderRoute(result);
        }
    }

    public class WhenPostingWhoWillEnterTheDetailsTestFixture : ApprenticeControllerTestFixtureBase
    {
        
        public WhenPostingWhoWillEnterTheDetailsTestFixture() : base()
        {

        }

        public async Task<IActionResult> WhoWillEnterTheDetails(WhoWillEnterTheDetailsViewModel viewModel)
        {
            return await _controller.WhoWillEnterTheDetails(viewModel);
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
