﻿using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingGetEnterNewTrainingProviderTests
{
    WhenCallingGetEnterNewTrainingProviderTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingGetEnterNewTrainingProviderTestsFixture();
    }

    [Test]
    public async Task ThenViewIsReturned()
    {
        var result = await _fixture.EnterNewTrainingProvider();

        _fixture.VerifyViewModel(result);
    }

    [Test]
    [TestCase(true, RouteNames.ConfirmDetailsAndSendRequest, Description = "Should return to confirm changes")]
    [TestCase(false, RouteNames.ChangeProviderInform, Description = "Should return to previous question")]
    [TestCase(null, RouteNames.ChangeProviderInform, Description = "Should return to previous question")]
    public async Task BackNavigationSetCorrectly(bool? isEdit, string expectedBackNavigationUrl)
    {
        _fixture.ArrangeRequestEditFlag(isEdit);

        var result = await _fixture.EnterNewTrainingProvider();

        _fixture.VerifyBackNavigation(expectedBackNavigationUrl);
    }


    [Test]
    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public async Task BackNavigationSetCorrectlyInViewData(bool? isEdit)
    {
        _fixture.ArrangeRequestEditFlag(isEdit);

        var result = await _fixture.EnterNewTrainingProvider();

        _fixture.VerifyViewDataSet(result as ViewResult);
    }
}

public class WhenCallingGetEnterNewTrainingProviderTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ChangeOfProviderRequest _request;
    private readonly EnterNewTrainingProviderViewModel _viewModel;
    private const string ConfirmDetailsLink = "cofirm-details-link";
    private const string PreviousQuestionLink = "previous-question-link";
    private string ExpectedBackLinkSet = "";

    public WhenCallingGetEnterNewTrainingProviderTestsFixture() : base()
    {
        _request = AutoFixture.Create<ChangeOfProviderRequest>();
        _viewModel = AutoFixture.Create<EnterNewTrainingProviderViewModel>();

        MockMapper
            .Setup(m => m.Map<EnterNewTrainingProviderViewModel>(_request))
            .ReturnsAsync(_viewModel);

        MockUrlHelper
            .Setup(mock => mock.Link(RouteNames.ConfirmDetailsAndSendRequest, It.IsAny<object>()))
            .Returns(ConfirmDetailsLink)
            .Callback(() => ExpectedBackLinkSet = ConfirmDetailsLink);

        MockUrlHelper
            .Setup(mock => mock.Link(RouteNames.ChangeProviderInform, It.IsAny<object>()))
            .Returns(PreviousQuestionLink)
            .Callback(() => ExpectedBackLinkSet = PreviousQuestionLink);
    }

    internal void ArrangeRequestEditFlag(bool? isEdit) => _request.Edit = isEdit;

    internal void VerifyBackNavigation(string expectedBackNavigationUrl) => MockUrlHelper.Verify(mock => mock.Link(expectedBackNavigationUrl, It.IsAny<object>()), Times.Once);


    internal void VerifyViewDataSet(ViewResult viewResult) => Assert.That(viewResult.ViewData["BackUrl"], Is.EqualTo(ExpectedBackLinkSet));

    public async Task<IActionResult> EnterNewTrainingProvider()
    {
        return await Controller.EnterNewTrainingProvider(_request);
    }

    public void VerifyViewModel(IActionResult actionResult)
    {
        var result = actionResult as ViewResult;
        var viewModel = result.Model;

        Assert.That(viewModel, Is.InstanceOf<EnterNewTrainingProviderViewModel>());

        var enterNewTrainingProviderViewModel = (EnterNewTrainingProviderViewModel)viewModel;

        Assert.That(enterNewTrainingProviderViewModel, Is.EqualTo(_viewModel));
    }
}