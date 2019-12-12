using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
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
        private readonly string _linkGeneratorResult;

        public WhenGettingReadyForReviewFixture()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<CohortsByAccountRequest>();
            _viewModel = autoFixture.Create<ReviewViewModel>();

            var modelMapper = new Mock<IModelMapper>();
            modelMapper.Setup(x => x.Map<ReviewViewModel>(It.Is<CohortsByAccountRequest>(r => r == _request)))
                .ReturnsAsync(_viewModel);

            _linkGeneratorResult = autoFixture.Create<string>();
            var linkGenerator = new Mock<ILinkGenerator>();
            linkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>()))
                .Returns(_linkGeneratorResult);

            CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<CohortController>>(),
                linkGenerator.Object,
                modelMapper.Object,
                Mock.Of<IAuthorizationService>());
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

            Assert.IsInstanceOf<ReviewViewModel>(viewModel);
            var reviewViewModel = (ReviewViewModel)viewModel;

            Assert.AreEqual(_viewModel, reviewViewModel);
        }
    }
}
