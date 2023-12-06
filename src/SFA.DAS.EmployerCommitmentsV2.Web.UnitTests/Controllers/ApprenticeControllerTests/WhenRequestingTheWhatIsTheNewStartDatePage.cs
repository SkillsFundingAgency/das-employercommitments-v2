using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingTheWhatIsTheNewStartDatePage 
    {
        private WhenRequestingTheWhatIsTheNewStartDatePageTestFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenRequestingTheWhatIsTheNewStartDatePageTestFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.WhatIsTheNewStartDate();

            _fixture.VerifyViewModel(result as ViewResult);
        }

        [Test]
        [TestCase(true, RouteNames.ConfirmDetailsAndSendRequest, Description = "Should return to confirm changes")]
        [TestCase(false , RouteNames.WhoWillEnterTheDetails, Description = "Should return to previous question")]
        [TestCase(null, RouteNames.WhoWillEnterTheDetails, Description = "Should return to previous question")]
        public async Task BackNavigationSetCorrectly(bool? isEdit, string expectedBackNavigationUrl)
        {
            _fixture.ArrangeRequestEditFlag(isEdit);

            var result = await _fixture.WhatIsTheNewStartDate();

            _fixture.VerifyBackNavigation(expectedBackNavigationUrl);
        }


        [Test]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(null)]
        public async Task BackNavigationSetCorrectlyInViewData(bool? isEdit)
        {
            _fixture.ArrangeRequestEditFlag(isEdit);

            var result = await _fixture.WhatIsTheNewStartDate();

            _fixture.VerifyViewDataSet(result as ViewResult);
        }
    }

    public class WhenRequestingTheWhatIsTheNewStartDatePageTestFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
        private readonly WhatIsTheNewStartDateViewModel _viewModel;
        private const string ConfirmDetailsLink = "cofirm-details-link";
        private const string PreviousQuestionLink = "previous-question-link";
        private string ExpectedBackLinkSet = "";

        public WhenRequestingTheWhatIsTheNewStartDatePageTestFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = _autoFixture.Create<WhatIsTheNewStartDateViewModel>();

            _mockMapper.Setup(m => m.Map<WhatIsTheNewStartDateViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _mockUrlHelper
               .Setup(mock => mock.Link(RouteNames.ConfirmDetailsAndSendRequest, It.IsAny<object>()))
               .Returns(ConfirmDetailsLink)
               .Callback(() => ExpectedBackLinkSet = ConfirmDetailsLink);

            _mockUrlHelper
               .Setup(mock => mock.Link(RouteNames.WhoWillEnterTheDetails, It.IsAny<object>()))
               .Returns(PreviousQuestionLink)
               .Callback(() => ExpectedBackLinkSet = PreviousQuestionLink);
        }

        internal void ArrangeRequestEditFlag(bool? isEdit) => _request.Edit = isEdit;

        internal async Task<IActionResult> WhatIsTheNewStartDate()
        {
            return await _controller.WhatIsTheNewStartDate(_request);
        }

        internal void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as WhatIsTheNewStartDateViewModel;

            Assert.That(viewModel, Is.InstanceOf<WhatIsTheNewStartDateViewModel>());
            Assert.That(viewModel, Is.EqualTo(_viewModel));
        }
        internal void VerifyBackNavigation(string expectedBackNavigationUrl)
        {
            _mockUrlHelper.Verify(mock => mock.Link(expectedBackNavigationUrl, It.IsAny<object>()), Times.Once);
        }

        internal void VerifyViewDataSet(ViewResult viewResult)
        {
            Assert.That(viewResult.ViewData["BackUrl"], Is.EqualTo(ExpectedBackLinkSet));
        }
    }
}
