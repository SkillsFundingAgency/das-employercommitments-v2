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
        public async Task ThenRedirectToTheWhatIsTheNewStopDatePage()
        {
            _viewModel.Edit = false;
            _fixture.SetupAdvanceToStopDate();

            var result = await _fixture.WhatIsTheNewStartDate(_viewModel);

            _fixture.VerifyRedirectsToTheWhatIsTheNewEndDatePage(result as RedirectToRouteResult);
        }

        [Test]
        public async Task AndUserIsChangingTheirAnswer_ThenRedirectToTheConfirmationPage()
        {
            _viewModel.Edit = true;
            _fixture.SetupReturnToCheckYourAnswers();
            var result = await _fixture.WhatIsTheNewStartDate(_viewModel);

            _fixture.VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(result as RedirectToRouteResult);
        }
    }

    public class WhenPostingWhatIsTheNewStartDateTestFixture : ApprenticeControllerTestFixtureBase
    {
        private ChangeOfProviderRequest _request;

        public WhenPostingWhatIsTheNewStartDateTestFixture() : base() { }

        public async Task<IActionResult> WhatIsTheNewStartDate(WhatIsTheNewStartDateViewModel viewModel)
        {
            return await _controller.WhatIsTheNewStartDate(viewModel);
        }

        public void VerifyRedirectsToTheWhatIsTheNewEndDatePage(IActionResult result)
        {
            var redirectResult = (RedirectToRouteResult)result;

            Assert.AreEqual(RouteNames.WhatIsTheNewEndDate, redirectResult.RouteName);

            var routeValues = redirectResult.RouteValues;

            Assert.AreEqual(_request.ProviderId, routeValues["ProviderId"]);
            Assert.AreEqual(_request.ApprenticeshipHashedId, routeValues["ApprenticeshipHashedId"]);
            Assert.AreEqual(_request.AccountHashedId, routeValues["AccountHashedId"]);
            Assert.AreEqual(_request.NewStartMonth, routeValues["NewStartMonth"]);
            Assert.AreEqual(_request.NewStartYear, routeValues["NewStartYear"]);
            Assert.AreEqual(null, routeValues["NewEndMonth"]);
            Assert.AreEqual(null, routeValues["NewEndYear"]);
            Assert.AreEqual(null, routeValues["NewPrice"]);
        }

        public void VerifyRedirectsBackToConfirmDetailsAndSendRequestPage(IActionResult result)
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

        public WhenPostingWhatIsTheNewStartDateTestFixture SetupAdvanceToStopDate()
        {
            _request = _autoFixture.Build<ChangeOfProviderRequest>()
                .With(x => x.NewStartMonth, 1)
                .With(x => x.NewStartYear, 2020)
                .Create();

            _request.NewEndMonth = null;
            _request.NewEndYear = null;
            _request.NewPrice = null;

            _mockMapper.Setup(m => m.Map<ChangeOfProviderRequest>(It.IsAny<WhatIsTheNewStartDateViewModel>()))
                .ReturnsAsync(_request);

            return this;
        }

        public WhenPostingWhatIsTheNewStartDateTestFixture SetupReturnToCheckYourAnswers()
        {
            _request = _autoFixture.Build<ChangeOfProviderRequest>()
                .With(x => x.NewStartMonth, 1)
                .With(x => x.NewStartYear, 2020)
                .With(x => x.NewEndMonth, 1)
                .With(x => x.NewEndYear, 2022)
                .With(x => x.NewPrice, 500)
                .Create();

            _mockMapper.Setup(m => m.Map<ChangeOfProviderRequest>(It.IsAny<WhatIsTheNewStartDateViewModel>()))
                .ReturnsAsync(_request);

            return this;
        }
    }
}
