using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingApprenticeshipDetailsTests
    {
        WhenCallingApprenticeshipDetailsTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingApprenticeshipDetailsTestsFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.ApprenticeshipDetails();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingApprenticeshipDetailsTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ApprenticeshipDetailsRequest _request;
        private readonly ApprenticeshipDetailsRequestViewModel _viewModel;

        public WhenCallingApprenticeshipDetailsTestsFixture() : base()
        {
            _request = _autoFixture.Create<ApprenticeshipDetailsRequest>();
            _viewModel = _autoFixture.Create<ApprenticeshipDetailsRequestViewModel>();

            _mockMapper.Setup(m => m.Map<ApprenticeshipDetailsRequestViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> ApprenticeshipDetails()
        {
            return await _controller.ApprenticeshipDetails(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ApprenticeshipDetailsRequestViewModel;

            Assert.IsInstanceOf<ApprenticeshipDetailsRequestViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}
