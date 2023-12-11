using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingGetWhatIsTheNewPriceTests
{
    private WhenCallingGetWhatIsTheNewPriceTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingGetWhatIsTheNewPriceTestsFixture();
    }

    [Test]
    public async Task ThenTheCorrectViewIsReturned()
    {
        var result = await _fixture.WhatIsTheNewPrice();

        _fixture.VerifyViewModel(result as ViewResult);
    }

    [Test]
    [TestCase(true, RouteNames.ConfirmDetailsAndSendRequest, Description = "Should return to confirm changes")]
    [TestCase(false, RouteNames.WhatIsTheNewEndDate, Description = "Should return to previous question")]
    [TestCase(null, RouteNames.WhatIsTheNewEndDate, Description = "Should return to previous question")]
    public async Task BackNavigationSetCorrectly(bool? isEdit, string expectedBackNavigationUrl)
    {
        _fixture.ArrangeRequestEditFlag(isEdit);

        var result = await _fixture.WhatIsTheNewPrice();

        _fixture.VerifyBackNavigation(expectedBackNavigationUrl);
    }


    [Test]
    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public async Task BackNavigationSetCorrectlyInViewData(bool? isEdit)
    {
        _fixture.ArrangeRequestEditFlag(isEdit);

        var result = await _fixture.WhatIsTheNewPrice();

        _fixture.VerifyViewDataSet(result as ViewResult);
    }
}

public class WhenCallingGetWhatIsTheNewPriceTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ChangeOfProviderRequest _request;
    private readonly WhatIsTheNewPriceViewModel _viewModel;
    private const string ConfirmDetailsLink = "cofirm-details-link";
    private const string PreviousQuestionLink = "previous-question-link";
    private string _expectedBackLinkSet = "";


    public WhenCallingGetWhatIsTheNewPriceTestsFixture()
    {
        _request = AutoFixture.Create<ChangeOfProviderRequest>();
        _viewModel = AutoFixture.Create<WhatIsTheNewPriceViewModel>();

        MockMapper
            .Setup(m => m.Map<WhatIsTheNewPriceViewModel>(_request))
            .ReturnsAsync(_viewModel);

        MockUrlHelper
            .Setup(mock => mock.Link(RouteNames.ConfirmDetailsAndSendRequest, It.IsAny<object>()))
            .Returns(ConfirmDetailsLink)
            .Callback(() => _expectedBackLinkSet = ConfirmDetailsLink);

        MockUrlHelper
            .Setup(mock => mock.Link(RouteNames.WhatIsTheNewEndDate, It.IsAny<object>()))
            .Returns(PreviousQuestionLink)
            .Callback(() => _expectedBackLinkSet = PreviousQuestionLink);
    }

    internal void ArrangeRequestEditFlag(bool? isEdit) => _request.Edit = isEdit;

    public async Task<IActionResult> WhatIsTheNewPrice()
    {
        return await Controller.WhatIsTheNewPrice(_request);
    }

    internal void VerifyBackNavigation(string expectedBackNavigationUrl) => MockUrlHelper.Verify(mock => mock.Link(expectedBackNavigationUrl, It.IsAny<object>()), Times.Once);


    internal void VerifyViewDataSet(ViewResult viewResult) => Assert.That(viewResult.ViewData["BackUrl"], Is.EqualTo(_expectedBackLinkSet));


    internal void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as WhatIsTheNewPriceViewModel;

        Assert.That(viewModel, Is.InstanceOf<WhatIsTheNewPriceViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }
}