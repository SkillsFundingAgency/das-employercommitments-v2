using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGettingReadyForReview
{
    private WhenGettingReadyForReviewFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenGettingReadyForReviewFixture();
    }

    [Test]
    public async Task ThenViewModelShouldBeMappedFromRequest()
    {
        await _fixture.GetReviews();
        _fixture.VerifyViewModelIsMappedFromRequest();
    }
}

public class WhenGettingReadyForReviewFixture
{
    private readonly CohortsByAccountRequest _request;
    private readonly ReviewViewModel _viewModel;
    private IActionResult _result;

    public WhenGettingReadyForReviewFixture()
    {
        var autoFixture = new Fixture();

        _request = autoFixture.Create<CohortsByAccountRequest>();
        _viewModel = autoFixture.Create<ReviewViewModel>();

        var modelMapper = new Mock<IModelMapper>();
        modelMapper.Setup(x => x.Map<ReviewViewModel>(It.Is<CohortsByAccountRequest>(r => r == _request)))
            .ReturnsAsync(_viewModel);

        CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            Mock.Of<ILinkGenerator>(),
            modelMapper.Object,
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>());
    }

    public CohortController CohortController { get; set; }


    public async Task GetReviews()
    {
        _result = await CohortController.Review(_request);
    }

    public void VerifyViewModelIsMappedFromRequest()
    {
        var viewResult = (ViewResult)_result;
        var viewModel = viewResult.Model;

        Assert.That(viewModel, Is.InstanceOf<ReviewViewModel>());
        var reviewViewModel = (ReviewViewModel)viewModel;

        Assert.That(reviewViewModel, Is.EqualTo(_viewModel));
    }
}