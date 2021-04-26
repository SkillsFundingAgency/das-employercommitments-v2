using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingGetWhatIsTheNewEndDateTests
    {
        WhenCallingGetWhatIsTheNewEndDateTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingGetWhatIsTheNewEndDateTestsFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.WhatIsTheNewEndDate();

            _fixture.VerifyViewModel(result as ViewResult);
        }

        [Test]
        [TestCase(true, RouteNames.ConfirmDetailsAndSendRequest, Description = "Should return to confirm changes")]
        [TestCase(false, RouteNames.WhatIsTheNewStartDate, Description = "Should return to previous question")]
        [TestCase(null, RouteNames.WhatIsTheNewStartDate, Description = "Should return to previous question")]
        public async Task BackNavigationSetCorrectly(bool isEdit, string expectedBackNavigationUrl)
        {
            _fixture.ArrangeRequestEditFlag(isEdit);

            var result = await _fixture.WhatIsTheNewEndDate();

            _fixture.VerifyBackNavigation(expectedBackNavigationUrl);
        }


        [Test]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(null)]
        public async Task BackNavigationSetCorrectlyInViewData(bool isEdit)
        {
            _fixture.ArrangeRequestEditFlag(isEdit);

            var result = await _fixture.WhatIsTheNewEndDate();

            _fixture.VerifyViewDataSet(result as ViewResult);
        }
    }

    public class WhenCallingGetWhatIsTheNewEndDateTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
        private readonly WhatIsTheNewEndDateViewModel _viewModel;
        private const string ConfirmDetailsLink = "cofirm-details-link";
        private const string PreviousQuestionLink = "previous-question-link";
        private string ExpectedBackLinkSet = "";

        public WhenCallingGetWhatIsTheNewEndDateTestsFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = _autoFixture.Create<WhatIsTheNewEndDateViewModel>();

            _mockMapper
                .Setup(m => m.Map<WhatIsTheNewEndDateViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _mockUrlHelper
               .Setup(mock => mock.Link(RouteNames.ConfirmDetailsAndSendRequest, It.IsAny<object>()))
               .Returns(ConfirmDetailsLink)
               .Callback(() => ExpectedBackLinkSet = ConfirmDetailsLink);

            _mockUrlHelper
               .Setup(mock => mock.Link(RouteNames.WhatIsTheNewStartDate, It.IsAny<object>()))
               .Returns(PreviousQuestionLink)
               .Callback(() => ExpectedBackLinkSet = PreviousQuestionLink);
        }

        internal void ArrangeRequestEditFlag(bool isEdit) => _request.Edit = isEdit;

        internal async Task<IActionResult> WhatIsTheNewEndDate()
        {
            return await _controller.WhatIsTheNewEndDate(_request);
        }

        internal void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as WhatIsTheNewEndDateViewModel;

            Assert.IsInstanceOf<WhatIsTheNewEndDateViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }

        internal void VerifyBackNavigation(string expectedBackNavigationUrl)
        {
            _mockUrlHelper.Verify(mock => mock.Link(expectedBackNavigationUrl, It.IsAny<object>()), Times.Once);
        }

        internal void VerifyViewDataSet(ViewResult viewResult)
        {
            Assert.AreEqual(ExpectedBackLinkSet, viewResult.ViewData["BackUrl"]);
        }
    }
}
