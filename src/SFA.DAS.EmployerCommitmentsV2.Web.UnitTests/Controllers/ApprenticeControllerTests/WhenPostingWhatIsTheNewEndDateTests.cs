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
    public class WhenPostingWhatIsTheNewEndDateTests
    {
        private Fixture _autoFixture;
        private WhenPostingWhatIsTheNewEndDateTestsFixture _fixture;
        private WhatIsTheNewEndDateViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();
            _fixture = new WhenPostingWhatIsTheNewEndDateTestsFixture();
            _viewModel = _autoFixture.Build<WhatIsTheNewEndDateViewModel>().Create();
        }

        [Test]
        public void ThenRedirectToTheWhatIsTheNewStopDatePage()
        {
            _viewModel.Edit = false;

            var result = _fixture.WhatIsTheNewEndDate(_viewModel);

            _fixture.VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(result as RedirectToRouteResult);
        }

        [Test]
        public void AndUserIsChangingTheirAnswer_ThenRedirectToTheConfirmationPage()
        {
            _viewModel.Edit = true;

            var result = _fixture.WhatIsTheNewEndDate(_viewModel);

            _fixture.VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(result as RedirectToRouteResult);
        }
    }

    public class WhenPostingWhatIsTheNewEndDateTestsFixture
    {

        private readonly ApprenticeController _controller;

        public WhenPostingWhatIsTheNewEndDateTestsFixture()
        {

            _controller = new ApprenticeController(Mock.Of<IModelMapper>(), 
                Mock.Of<ICookieStorageService<IndexRequest>>(), 
                Mock.Of<ICommitmentsApiClient>(), 
                Mock.Of<ILinkGenerator>(), 
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        public IActionResult WhatIsTheNewEndDate(WhatIsTheNewEndDateViewModel viewModel)
        {
            return _controller.WhatIsTheNewEndDate(viewModel);
        }

        public void VerifyRedirectsToTheWhatIsTheNewPricePage(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.WhatIsTheNewPrice, redirectResult.RouteName);
        }

        public void VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.ConfirmDetailsAndSendRequest, redirectResult.RouteName);
        }
    }
}
