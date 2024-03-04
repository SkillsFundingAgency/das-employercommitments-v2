using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingGetWhatIsTheNewEndDateTests
{
    private WhenCallingGetWhatIsTheNewEndDateTestsFixture _fixture;

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
    public async Task BackNavigationSetCorrectly(bool? isEdit, string expectedBackNavigationUrl)
    {
        _fixture.ArrangeRequestEditFlag(isEdit);

        var result = await _fixture.WhatIsTheNewEndDate();

        _fixture.VerifyBackNavigation(expectedBackNavigationUrl);
    }


    [Test]
    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public async Task BackNavigationSetCorrectlyInViewData(bool? isEdit)
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
    private string _expectedBackLinkSet = "";

    public WhenCallingGetWhatIsTheNewEndDateTestsFixture()
    {
        _request = AutoFixture.Create<ChangeOfProviderRequest>();
        _viewModel = AutoFixture.Create<WhatIsTheNewEndDateViewModel>();

        MockMapper
            .Setup(m => m.Map<WhatIsTheNewEndDateViewModel>(_request))
            .ReturnsAsync(_viewModel);

        MockUrlHelper
            .Setup(mock => mock.Link(RouteNames.ConfirmDetailsAndSendRequest, It.IsAny<object>()))
            .Returns(ConfirmDetailsLink)
            .Callback(() => _expectedBackLinkSet = ConfirmDetailsLink);

        MockUrlHelper
            .Setup(mock => mock.Link(RouteNames.WhatIsTheNewStartDate, It.IsAny<object>()))
            .Returns(PreviousQuestionLink)
            .Callback(() => _expectedBackLinkSet = PreviousQuestionLink);
    }

    internal void ArrangeRequestEditFlag(bool? isEdit) => _request.Edit = isEdit;

    internal async Task<IActionResult> WhatIsTheNewEndDate()
    {
        return await Controller.WhatIsTheNewEndDate(_request);
    }

    internal void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as WhatIsTheNewEndDateViewModel;

        Assert.That(viewModel, Is.InstanceOf<WhatIsTheNewEndDateViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }

    internal void VerifyBackNavigation(string expectedBackNavigationUrl)
    {
        MockUrlHelper.Verify(mock => mock.Link(expectedBackNavigationUrl, It.IsAny<object>()), Times.Once);
    }

    internal void VerifyViewDataSet(ViewResult viewResult)
    {
        Assert.That(viewResult.ViewData["BackUrl"], Is.EqualTo(_expectedBackLinkSet));
    }
}