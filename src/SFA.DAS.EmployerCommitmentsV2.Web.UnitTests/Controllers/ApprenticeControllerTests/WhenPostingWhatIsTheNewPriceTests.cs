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
    public class WhenPostingWhatIsTheNewPriceTests
    {
        private Fixture _autoFixture;
        private WhenPostingWhatIsTheNewPriceTestsFixture _fixture;
        private WhatIsTheNewPriceViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();
            _fixture = new WhenPostingWhatIsTheNewPriceTestsFixture();
            _viewModel = _autoFixture.Build<WhatIsTheNewPriceViewModel>().Create();
        }   

        [Test]
        public void AndUserIsChangingTheirAnswer_ThenRedirectToTheConfirmationPage()
        {
            _viewModel.Edit = true;

            var result = _fixture.WhatIsTheNewPrice(_viewModel);

            _fixture.VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(result as RedirectToRouteResult);
        }

    }

    public class WhenPostingWhatIsTheNewPriceTestsFixture
    {

        private readonly ApprenticeController _controller;

        public WhenPostingWhatIsTheNewPriceTestsFixture()
        {

            _controller = new ApprenticeController(Mock.Of<IModelMapper>(),
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILinkGenerator>(),
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        public IActionResult WhatIsTheNewPrice(WhatIsTheNewPriceViewModel viewModel)
        {
            return _controller.WhatIsTheNewPrice(viewModel);
        }     

        public void VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.ConfirmDetailsAndSendRequest, redirectResult.RouteName);
        }
    }
}
