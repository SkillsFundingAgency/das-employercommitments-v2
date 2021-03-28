using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingEditApprenticeshipTests
    {
        WhenCallingEditApprenticeshipTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingEditApprenticeshipTestsFixture();
        }

        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.EditApprenticeship();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingEditApprenticeshipTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly EditApprenticeshipRequest _request;
        private readonly EditApprenticeshipRequestViewModel _viewModel;

        public WhenCallingEditApprenticeshipTestsFixture() : base()
        {
            _request = _autoFixture.Create<EditApprenticeshipRequest>();
            _viewModel = _autoFixture.Create<EditApprenticeshipRequestViewModel>();
            

            _mockMapper.Setup(m => m.Map<EditApprenticeshipRequestViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> EditApprenticeship()
        {
            return await _controller.EditApprenticeship(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ApprenticeshipDetailsRequestViewModel;

            Assert.IsInstanceOf<ApprenticeshipDetailsRequestViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}
