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
    public class WhenPostingWhatIsTheNewStartDateTests
    {
        private Fixture _autoFixture;
        private WhenPostingWhatIsTheNewStartDateTestFixture _fixture;

        private WhatIsTheNewStartDateViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();

            _viewModel = _autoFixture.Build<WhatIsTheNewStartDateViewModel>().Create();

            _fixture = new WhenPostingWhatIsTheNewStartDateTestFixture();
        }

        [Test]
        public void ThenRedirectToTheWhatIsTheNewStopDatePage()
        {
            _viewModel.Edit = false;

            var result = _fixture.WhatIsTheNewStartDate(_viewModel);

            _fixture.VerifyRedirectsToTheWhatIsTheNewEndDatePage(result as RedirectToRouteResult);
        }

        [Test]
        public void AndUserIsChangingTheirAnswer_ThenRedirectToTheConfirmationPage()
        {
            _viewModel.Edit = true;

            var result = _fixture.WhatIsTheNewStartDate(_viewModel);

            _fixture.VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(result as RedirectToRouteResult);
        }
    }

    public class WhenPostingWhatIsTheNewStartDateTestFixture
    {
        private readonly ApprenticeController _controller;

        public WhenPostingWhatIsTheNewStartDateTestFixture()
        {
            _controller = new ApprenticeController(Mock.Of<IModelMapper>(),
               Mock.Of<ICookieStorageService<IndexRequest>>(),
               Mock.Of<ICommitmentsApiClient>(),
               Mock.Of<ILinkGenerator>(),
               Mock.Of<ILogger<ApprenticeController>>(),
               Mock.Of<IAuthorizationService>());
        }

        public IActionResult WhatIsTheNewStartDate(WhatIsTheNewStartDateViewModel viewModel)
        {
            return _controller.WhatIsTheNewStartDate(viewModel);
        }

        public void VerifyRedirectsToTheWhatIsTheNewEndDatePage(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.WhatIsTheNewEndDate, redirectResult.RouteName);
        }

        public void VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.ConfirmDetailsAndSendRequest, redirectResult.RouteName);
        }
    }
}
