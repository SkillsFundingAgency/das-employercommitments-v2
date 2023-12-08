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

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenGettingCohortsWithTrainingProvider
    {
        private WhenGettingCohortsWithTrainingProviderFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenGettingCohortsWithTrainingProviderFixture();
        }

        [Test]
        public async Task ThenViewModelShouldBeMappedFromRequest()
        {
            await _fixture.GetCohortsWithTrainingProvider();
            _fixture.VerifyViewModelIsMappedFromRequest();
        }
    }

    public class WhenGettingCohortsWithTrainingProviderFixture
    {
        private readonly CohortsByAccountRequest _request;
        private readonly WithTrainingProviderViewModel _viewModel;
        private IActionResult _result;

        public WhenGettingCohortsWithTrainingProviderFixture()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<CohortsByAccountRequest>();
            _viewModel = autoFixture.Create<WithTrainingProviderViewModel>();

            var modelMapper = new Mock<IModelMapper>();
            modelMapper.Setup(x => x.Map<WithTrainingProviderViewModel>(It.Is<CohortsByAccountRequest>(r => r == _request)))
                .ReturnsAsync(_viewModel);

            CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILogger<CohortController>>(),
                Mock.Of<ILinkGenerator>(),
                modelMapper.Object,
                Mock.Of<IEncodingService>(),
                Mock.Of<IApprovalsApiClient>());
        }

        public CohortController CohortController { get; set; }


        public async Task GetCohortsWithTrainingProvider()
        {
            _result = await CohortController.WithTrainingProvider(_request);
        }

        public void VerifyViewModelIsMappedFromRequest()
        {
            var viewResult = (ViewResult)_result;
            var viewModel = viewResult.Model;

            Assert.That(viewModel, Is.InstanceOf<WithTrainingProviderViewModel>());
            var WithTrainingProviderViewModel = (WithTrainingProviderViewModel)viewModel;

            Assert.That(WithTrainingProviderViewModel, Is.EqualTo(_viewModel));
        }
    }
}
