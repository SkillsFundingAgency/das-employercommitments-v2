using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
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
    }

    public class WhenCallingGetEnterNewTrainingProviderTestsFixture
    {
        private readonly Mock<IModelMapper> _mockMapper;

        private readonly ChangeOfProviderRequest _request;
        private readonly EnterNewTrainingProviderViewModel _viewModel;
        
        private ApprenticeController _controller;

        public WhenCallingGetEnterNewTrainingProviderTestsFixture()
        {
            var autoFixture = new Fixture();
            _request = autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = autoFixture.Create<EnterNewTrainingProviderViewModel>();

            _mockMapper = new Mock<IModelMapper>();
            _mockMapper.Setup(m => m.Map<EnterNewTrainingProviderViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _controller = new ApprenticeController(_mockMapper.Object, 
                Mock.Of<ICookieStorageService<IndexRequest>>(), 
                Mock.Of<ICommitmentsApiClient>(),
                Mock.Of<ILinkGenerator>(), 
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        public async Task<IActionResult> EnterNewTrainingProvider()
        {
            return await _controller.EnterNewTrainingProvider(_request);
        }

        public void VerifyViewModel(IActionResult actionResult)
        {
            var result = actionResult as ViewResult;
            var viewModel = result.Model;

            Assert.IsInstanceOf<EnterNewTrainingProviderViewModel>(viewModel);

            var enterNewTrainingProviderViewModel = (EnterNewTrainingProviderViewModel)viewModel;

            Assert.AreEqual(_viewModel, enterNewTrainingProviderViewModel);
        }
    }
}
