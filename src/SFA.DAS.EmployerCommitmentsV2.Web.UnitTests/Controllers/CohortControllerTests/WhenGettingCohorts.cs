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
        public async Task ThenViewModelShouldBeMappedFromRequest()
        {
            var f = new WhenGettingCohortsFixture();
            await f.GetCohorts();
            f.VerifyViewModelIsMappedFromRequest();
        }
    }

    public class WhenGettingCohortsFixture
    {
        private readonly CohortsRequest _request;
        private readonly CohortsViewModel _viewModel;
        private IActionResult _result;

        public WhenGettingCohortsFixture()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<CohortsRequest>();
            _viewModel = autoFixture.Create<CohortsViewModel>();

            var modelMapper = new Mock<IModelMapper>();
            modelMapper.Setup(x => x.Map<CohortsViewModel>(It.Is<CohortsRequest>(r => r == _request))).ReturnsAsync(_viewModel);

            CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<CohortController>>(),
                Mock.Of<ILinkGenerator>(),
                modelMapper.Object,
                Mock.Of<IAuthorizationService>());
        }

        public CohortController CohortController { get; set; }

        public async Task GetCohorts()
        {
            _result = await CohortController.Cohorts(_request);
        }

        public void VerifyViewModelIsMappedFromRequest()
        {
            var viewResult = (ViewResult)_result;
            Assert.IsInstanceOf<CohortsViewModel>(viewResult.Model);
            Assert.AreSame(_viewModel, viewResult.Model);
        }
    }
}
