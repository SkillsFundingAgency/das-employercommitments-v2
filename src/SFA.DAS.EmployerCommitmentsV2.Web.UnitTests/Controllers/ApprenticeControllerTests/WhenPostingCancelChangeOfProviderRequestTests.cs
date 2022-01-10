using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingCancelChangeOfProviderRequestTests
    {
        private Fixture _autoFixture;
        private WhenPostingCancelChangeOfProviderRequestTestsFixture _fixture;
        private CancelChangeOfProviderRequestViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();

            _viewModel = _autoFixture.Build<CancelChangeOfProviderRequestViewModel>().Create();

            _fixture = new WhenPostingCancelChangeOfProviderRequestTestsFixture();
        }

        [Test]
        public async Task AndYesIsSelected_ThenRedirectToApprenticeDetailsPage()
        {
            _viewModel.CancelRequest = true;
            
            var result = await _fixture.CancelChangeOfProviderRequest(_viewModel);

            _fixture.VerifyRedirectsToApprenticeDetailsPage(result as RedirectToRouteResult);
        }

        [Test]
        public async Task AndNoIsSelected_ThenRedirectToConfirmAndSendPage()
        {
            _viewModel.CancelRequest = false;

            var result = await _fixture.CancelChangeOfProviderRequest(_viewModel);

            _fixture.VerifyRedirectsToConfirmAndSendPage(result as RedirectToRouteResult);
        }
    }

    public class WhenPostingCancelChangeOfProviderRequestTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
      
        public WhenPostingCancelChangeOfProviderRequestTestsFixture() : base() 
        {
            _request = _autoFixture.Build<ChangeOfProviderRequest>().Create();
            _mockMapper.Setup(m => m.Map<ChangeOfProviderRequest>(It.IsAny<CancelChangeOfProviderRequestViewModel>())).ReturnsAsync(_request);
        }

        public async Task<IActionResult> CancelChangeOfProviderRequest(CancelChangeOfProviderRequestViewModel viewModel)
        {
            return await _controller.CancelChangeOfProviderRequest(viewModel);
        }

        public void VerifyRedirectsToApprenticeDetailsPage(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual(RouteNames.ApprenticeDetail, redirectResult.RouteName);
        }

        public void VerifyRedirectsToConfirmAndSendPage(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.ConfirmDetailsAndSendRequest, redirectResult.RouteName);

            var routeValues = redirectResult.RouteValues;

            Assert.AreEqual(_request.ProviderId, routeValues["ProviderId"]);
            Assert.AreEqual(_request.ApprenticeshipHashedId, routeValues["ApprenticeshipHashedId"]);
            Assert.AreEqual(_request.AccountHashedId, routeValues["AccountHashedId"]);
            Assert.AreEqual(_request.NewStartMonth, routeValues["NewStartMonth"]);
            Assert.AreEqual(_request.NewStartYear, routeValues["NewStartYear"]);
            Assert.AreEqual(_request.NewEndMonth, routeValues["NewEndMonth"]);
            Assert.AreEqual(_request.NewEndYear, routeValues["NewEndYear"]);
            Assert.AreEqual(_request.NewPrice, routeValues["NewPrice"]);
        }
    }
}
