using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using System;
using System.Collections.Generic;
using System.Text;
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

            _fixture.VerifyRedirectsToApprenticeDetailsPage(result as RedirectResult);
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
        private const string ApprenticeDetailsV1Url = "https://employercommitmentsv1/apprentice/details";

        public WhenPostingCancelChangeOfProviderRequestTestsFixture() : base() 
        {
            _request = _autoFixture.Build<ChangeOfProviderRequest>().Create();

            _mockMapper.Setup(m => m.Map<ChangeOfProviderRequest>(It.IsAny<CancelChangeOfProviderRequestViewModel>()))
                .ReturnsAsync(_request);

            _mockLinkGenerator.Setup(g => g.CommitmentsLink(It.IsAny<string>())).Returns(ApprenticeDetailsV1Url);
        }

        public async Task<IActionResult> CancelChangeOfProviderRequest(CancelChangeOfProviderRequestViewModel viewModel)
        {
            return await _controller.CancelChangeOfProviderRequest(viewModel);
        }

        public void VerifyRedirectsToApprenticeDetailsPage(RedirectResult result)
        {
            Assert.AreEqual(ApprenticeDetailsV1Url, result.Url);
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
