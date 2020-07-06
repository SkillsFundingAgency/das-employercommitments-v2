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
    public class WhenGettingCohorts
    {
        [Test]
        public void ThenViewModelShouldBeMappedFromRequest()
        {
            var f = new WhenGettingCohortsFixture();
            f.GetCohorts();
            f.VerifyViewModelIsMappedFromRequest();
        }
    }

    public class WhenGettingCohortsFixture
    {
        public CohortController CohortController { get; set; }

        private readonly CohortsByAccountRequest _request;
        private readonly CohortsViewModel _viewModel;
        private IActionResult _result;

        public WhenGettingCohortsFixture()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<CohortsByAccountRequest>();
            _viewModel = autoFixture.Create<CohortsViewModel>();

            var modelMapper = new Mock<IModelMapper>();
            modelMapper.Setup(x => x.Map<CohortsViewModel>(It.Is<CohortsByAccountRequest>(r => r == _request))).ReturnsAsync(_viewModel);

            CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<CohortController>>(),
                Mock.Of<ILinkGenerator>(),
                modelMapper.Object,
                Mock.Of<IAuthorizationService>());
        }

        public void GetCohorts()
        {
            _result = CohortController.Cohorts(_request);
        }

        public void VerifyViewModelIsMappedFromRequest()
        {
            _result.VerifyReturnsRedirectToActionResult().WithActionName("Review");
        }
    }
}
