using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingChangeVersionTests
    {
        WhenCallingChangeVersionTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingChangeVersionTestsFixture();
        }

        [Test]
        public async Task ThenReturnView()
        {
            var result = await _fixture.ChangeVersion();

            _fixture.VerifyViewModel(result as ViewResult);
        }

        [Test]
        public async Task ThenSelectedVersionSet_WhenEditModelAvailable()
        {
            var version = "1.3";
            _fixture.SetEditApprenticeViewModel(version);
            var result = await _fixture.ChangeVersion();

            _fixture.VerifySelectedVersion(result as ViewResult, version);
        }
    }

    public class WhenCallingChangeVersionTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ChangeVersionRequest _request;
        private readonly ChangeVersionViewModel _viewModel;

        public WhenCallingChangeVersionTestsFixture() : base()
        {
            _request = _autoFixture.Create<ChangeVersionRequest>();
            _viewModel = _autoFixture.Create<ChangeVersionViewModel>();

            _mockMapper.Setup(m => m.Map<ChangeVersionViewModel>(_request))
                .ReturnsAsync(_viewModel);

            _controller.TempData = new TempDataDictionary(Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
        }

        public void SetEditApprenticeViewModel(string version)
        {
            var editApprenticeViewModel = new EditApprenticeshipRequestViewModel
            {
                Version = version
            };

            object serializedModel = JsonConvert.SerializeObject(editApprenticeViewModel);
            _tempDataDictionary.Setup(s => s.TryGetValue("EditApprenticeshipRequestViewModel", out serializedModel)).Returns(true);
        }

        public async Task<IActionResult> ChangeVersion()
        {
            return await _controller.ChangeVersion(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ChangeVersionViewModel;

            Assert.AreEqual(_viewModel, viewModel);
        }

        public void VerifySelectedVersion(ViewResult viewResult, string version)
        {
            var viewModel = viewResult.Model as ChangeVersionViewModel;

            viewModel.SelectedVersion.Should().Be(version);
        }
    }
}
