using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingChangeOption
    {
        WhenCallingChangeOptionTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingChangeOptionTestsFixture();
        }

        [Test]
        public async Task Then_ReturnView()
        {
            var result = await _fixture.ChangeOption();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingChangeOptionTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private ChangeOptionRequest _request;
        private ChangeOptionViewModel _viewModel;

        public WhenCallingChangeOptionTestsFixture() : base()
        {
            _request = _autoFixture.Create<ChangeOptionRequest>();
            _viewModel = _autoFixture.Create<ChangeOptionViewModel>();

            _mockMapper.Setup(m => m.Map<ChangeOptionViewModel>(It.IsAny<ChangeOptionRequest>()))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> ChangeOption()
        {
            return await _controller.ChangeOption(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ChangeOptionViewModel;

            viewModel.Should().BeEquivalentTo(_viewModel);
        }
    }
}
