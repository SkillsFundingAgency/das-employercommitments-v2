using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
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
    }
}
