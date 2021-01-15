using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

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

    public class WhenPostingWhatIsTheNewPriceTestsFixture : ApprenticeControllerTestFixtureBase
    {
        public WhenPostingWhatIsTheNewPriceTestsFixture() : base () { }

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
