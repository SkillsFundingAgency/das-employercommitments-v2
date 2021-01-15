using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
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

    public class WhenCallingGetEnterNewTrainingProviderTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeOfProviderRequest _request;
        private readonly EnterNewTrainingProviderViewModel _viewModel;

        public WhenCallingGetEnterNewTrainingProviderTestsFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOfProviderRequest>();
            _viewModel = _autoFixture.Create<EnterNewTrainingProviderViewModel>();

            _mockMapper.Setup(m => m.Map<EnterNewTrainingProviderViewModel>(_request))
                .ReturnsAsync(_viewModel);
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
